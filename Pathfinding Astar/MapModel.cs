using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding_Astar
{
    abstract class MapModel
    {
        private static int _Size = 100;
        // Image représentant la carte
        private static Bitmap image = new Bitmap(_Size, _Size);
        // Point de départ
        public static (int x, int y) _Departure { get; private set; }
        // Point d'arrivée
        public static (int x, int y) _End { get; private set; }

        // Génère la carte avec un nombre spécifié de murs
        public static Bitmap mapGeneration(int Wall)
        {
            ResetImage();
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
            image.SetPixel(x, y, Color.Green);
            MapModel._Departure = (x, y);
        }

        // Définit le point d'arrivée aléatoirement
        private static void EndPoint()
        {
            int x = new Random().Next(1, _Size - 2);
            int y = new Random().Next(1, _Size - 2);
            image.SetPixel(x, y, Color.Green);
            MapModel._End = (x, y);
        }

        // Réinitialise l'image en mettant tous les pixels en transparent
        private static void ResetImage()
        {
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    image.SetPixel(i, j, Color.Empty);
                }
            }
        }

        // Génère les bordures de la carte
        private static void GenerateSquare()
        {
            for (int i = 0; i < image.Width; i++)
            {
                image.SetPixel(i, 0, Color.White);
                image.SetPixel(i, _Size - 1, Color.White);
                image.SetPixel(0, i, Color.White);
                image.SetPixel(_Size - 1, i, Color.White);
            }
        }

        // Vérifie si une cellule est traversable
        public static bool IsTraversable(Bitmap imageCP, int x, int y)
        {
            if (imageCP.GetPixel(x, y).Name != "ffffffff" && imageCP.GetPixel(x, y).Name != "ffffb6c1")
            {
                return true;
            }
            return false;
        }

        // Vérifie si une cellule est fermée (déjà explorée)
        public static bool IsClosed(Bitmap imageCP, int x, int y, List<Node> Closed)
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
                    image.SetPixel(x, y, Color.LightPink);
                }
            }
        }
    }
}
