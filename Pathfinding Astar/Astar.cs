using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Pathfinding_Astar
{
    class Astar
    {
        private char[,] image;
        private ulong calculs = 0;
        private static int _Size = MapModel._Size;
        // Changement : Utilisation de SortedSet au lieu de List pour OpenSet
        // Raison : Permet une insertion et une extraction plus rapides du nœud avec le coût F le plus bas (O(log n) au lieu de O(n))
        private SortedSet<Node> OpenSet;
        // Changement : Utilisation de HashSet<(int, int)> au lieu de List<Node> pour ClosedNodes
        // Raison : Permet une vérification plus rapide si un nœud a déjà été exploré (O(1) au lieu de O(n))
        private HashSet<(int, int)> ClosedNodes;
        private Node[,] Grid = new Node[_Size, _Size];
        private Node _Current;
        private bool _isWay = false;
        private Stopwatch _StopWatch = null;
        public bool isWay {
            get
            {
                return _isWay;
            }
            private set
            {
                _isWay = value;
            }
        }

        public Astar(char[,] image)
        {
            this.image = image;
            // Initialisation de OpenSet avec un comparateur personnalisé pour trier par coût F
            OpenSet = new SortedSet<Node>(Comparer<Node>.Create((a, b) => a._FCost.CompareTo(b._FCost)));
            ClosedNodes = new HashSet<(int, int)>();
            var startNode = new Node(MapModel._Departure.x, MapModel._Departure.y);
            OpenSet.Add(startNode);
            Grid[MapModel._Departure.x, MapModel._Departure.y] = startNode;
            Grid[MapModel._End.x, MapModel._End.y] = new Node(MapModel._End.x, MapModel._End.y);
            startNode._GCost = 0;
            startNode._FCost = HCostCalculate(MapModel._Departure.x, MapModel._Departure.y);
            _StopWatch = new Stopwatch();
            _StopWatch.Start();
            Path();
        }

        public void Path()
        {
            while (OpenSet.Count > 0)
            {
                // Changement : Utilisation de OpenSet.Min au lieu de QuickSort.Sort
                // Raison : Plus efficace pour obtenir le nœud avec le coût F le plus bas
                _Current = OpenSet.Min;
                OpenSet.Remove(_Current);

                image[_Current._X, _Current._Y] = 'V';
                ClosedNodes.Add((_Current._X, _Current._Y));

                if ((_Current._X, _Current._Y) == MapModel._End)
                {
                    ReconstructPath();
                    break;
                }

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if ((i, j) != (0, 0))
                        {
                            // Précalcul des nouvelles coordonnées pour éviter des calculs répétitifs
                            int newX = _Current._X + i;
                            int newY = _Current._Y + j;
                            if (MapModel.IsTraversable(image, newX, newY) && !ClosedNodes.Contains((newX, newY)))
                            {
                                image[newX, newY] = 'N';
                                // Changement : Utilisation d'une nouvelle méthode ProcessNeighbor
                                // Raison : Combine les fonctionnalités de GCostCalculate et NeighbourChecking en une seule méthode plus efficace
                                ProcessNeighbor(newX, newY);
                                calculs++;
                            }
                        }
                    }
                }
            }
            if (!isWay)
            {
                Console.WriteLine("Chemin impossible");
            }
            _StopWatch.Stop();
            Console.WriteLine("Nodes calculé : " + calculs);
            Console.WriteLine("Temps du chemin : " + _StopWatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Taille de la cart en Pixels : " + _Size + "*" + _Size);
            MapModel.GenerateBMP();

        }

        // Nouvelle méthode : ProcessNeighbor
        // Cette méthode combine les fonctionnalités de GCostCalculate et NeighbourChecking
        private void ProcessNeighbor(int x, int y)
        {
            Node neighbor = new Node(x, y);
            int newGCost = _Current._GCost + ((x != _Current._X && y != _Current._Y) ? 14 : 10);

            if (newGCost < neighbor._GCost)
            {
                neighbor._GCost = newGCost;
                neighbor._Parent = _Current;
                neighbor._FCost = neighbor._GCost + HCostCalculate(x, y);

                // Gestion efficace de la mise à jour des nœuds existants dans OpenSet
                if (!OpenSet.Contains(neighbor))
                {
                    OpenSet.Add(neighbor);
                }
                else
                {
                    OpenSet.Remove(neighbor);
                    OpenSet.Add(neighbor);
                }

                Grid[x, y] = neighbor;
            }
        }

        // Cette méthode n'a pas changé
        private static double HCostCalculate(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x - MapModel._End.x, 2) + Math.Pow(y - MapModel._End.y, 2));
        }

        // Cette méthode n'a pas changé
        private void ReconstructPath()
        {
            isWay = true;
            while ((_Current._X, _Current._Y) != MapModel._Departure)
            {
                image[_Current._X, _Current._Y] = 'P';
                _Current = _Current._Parent;
            }
        }
    }
}