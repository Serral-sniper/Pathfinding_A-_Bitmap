﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding_Astar
{
    abstract class MapModel
    {
        public static int _Size = 1000;
        // Image représentant la carte
        private static char[,] image = new char[_Size + 1,_Size + 1];
        // Point de départ
        public static (int x, int y) _Departure { get; private set; }
        // Point d'arrivée
        public static (int x, int y) _End { get; private set; }
        public static Bitmap BMP = new Bitmap(_Size, _Size);

        // Génère la carte avec un nombre spécifié de murs
        public static char[,] mapGeneration(int Wall)
        {
            SetBlankSpaces();
            GenerateSquare();
            DeparturePoint();
            EndPoint();
            WallGeneration(Wall);
            return image;
        }

        // Définit le point de départ aléatoirement
        private static void DeparturePoint()
        {
            int x = new Random().Next(1, _Size - 2);
            int y = new Random().Next(1, _Size - 2);
            image[x, y] = 'D';
            MapModel._Departure = (x, y);
        }

        // Définit le point d'arrivée aléatoirement
        private static void EndPoint()
        {
            int x = new Random().Next(1, _Size - 2);
            int y = new Random().Next(1, _Size - 2);
            image[x, y] = 'E';
            MapModel._End = (x, y);
        }

        // Réinitialise l'image en mettant tous les pixels en transparent
        private static void SetBlankSpaces()
        {
            for (int i = 0; i < _Size; i++)
            {
                for (int j = 0; j < _Size; j++)
                {
                    image[i, j] = ' ';
                }
            }
        }

        // Génère les bordures de la carte
        private static void GenerateSquare()
        {
            for (int i = 0; i < image.GetLength(0) - 1; i++)
            {
                image[i, 0] = 'B';
                image[i, _Size - 1] = 'B';
                image[0, i] = 'B';
                image[_Size - 1, i] = 'B';
            }
        }

        // Vérifie si une cellule est traversable
        public static bool IsTraversable(char[,] imageCP, int x, int y)
        {
            if (imageCP[x, y] != 'B' && imageCP[x, y] != 'W')
            {
                return true;
            }
            return false;
        }

        // Vérifie si une cellule est fermée (déjà explorée)
        public static bool IsClosed(int x, int y, List<Node> Closed)
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                if ((x, y) == (Closed[i]._X, Closed[i]._Y))
                {
                    return true;
                }
            }
            return false;
        }

        // Génère des murs aléatoires sur la carte
        private static void WallGeneration(int Wall)
        {
            for (int i = 0; i < Wall; i++)
            {
                int x = new Random().Next(1, _Size - 1);
                int y = new Random().Next(1, _Size - 1);
                if ((x, y) != _Departure && (x, y) != _End)
                {
                    image[x, y] = 'W';
                }
            }
        }
        public static void GenerateBMP()
        {
            for (int i = 0; i < _Size; i++)
            {
                for (int j = 0; j < _Size; j++)
                {
                   switch (image[i, j])
                   {
                        case 'B': 
                            BMP.SetPixel(i, j, Color.White);
                            break;
                        case 'W':
                            BMP.SetPixel(i, j, Color.Silver);
                            break;
                        case 'V':
                            BMP.SetPixel(i, j, Color.Red);
                            break;
                        case 'N':
                            BMP.SetPixel(i, j, Color.Green);
                            break;
                        case 'P':
                            BMP.SetPixel(i, j, Color.Blue);
                            break;
                        default:
                            BMP.SetPixel(i, j, Color.Empty);
                            break;

                   }
                }
            }
            BMP.SetPixel(MapModel._Departure.x, MapModel._Departure.y, Color.Yellow);
            BMP.SetPixel(MapModel._End.x, MapModel._End.y, Color.Yellow);
            BMP.Save(@"C:/Users/Utilisateur/Documents/Pathfinding Astar/SavedPath.bmp");
        }
    }
}
