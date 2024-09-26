using SuperLinq.Collections;
using System.Diagnostics;



namespace Pathfinding_Astar
{
    class Astar
    {
        private static int _Size = MapModel._Size;
        private char[,] Image;
        private Node[,] Grid = new Node[_Size, _Size];
        private UpdatablePriorityQueue<Node, double> OpenSet = new UpdatablePriorityQueue<Node, double>();
        private Node _Current;
        private ulong calculs = 0;
        private Stopwatch _StopWatch = null;
        public bool IsWay { get; private set; }

        public Astar(char[,] image)
        {
            // Initialisation
            Image = image;
            Node startNode = new Node(MapModel._Departure.x, MapModel._Departure.y);
            OpenSet.Enqueue(startNode, startNode._FCost);
            Grid[MapModel._Departure.x, MapModel._Departure.y] = startNode;
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
                _Current = OpenSet.Dequeue();

                // Marque le nœud actuel comme exploré dans le tableau
                Image[_Current._X, _Current._Y] = 'V';

                if ((_Current._X, _Current._Y) == MapModel._End)
                {
                    ReconstructPath();
                    IsWay = true;
                    break;
                }

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if ((i, j) != (0, 0))
                        {
                            int newX = _Current._X + i;
                            int newY = _Current._Y + j;
                            // Vérifie si le nœud est traversable et non déjà exploré
                            if (MapModel.IsTraversable(Image, newX, newY) && Image[newX, newY] != 'V')
                            {
                                ProcessNeighbor(newX, newY);
                                calculs++;
                            }
                        }
                    }
                }
            }

            _StopWatch.Stop();
            if (!IsWay)
                Console.WriteLine("Chemin impossible à réaliser");
            Console.WriteLine("Nodes calculés : " + calculs);
            Console.WriteLine("Temps du chemin : " + _StopWatch.ElapsedMilliseconds + " ms");
            MapModel.GenerateBMP();
        }

        private void ProcessNeighbor(int x, int y)
        {
            Node neighbor;
            if (Grid[x, y] == null)
            {
                neighbor = new Node(x, y);
                Grid[x, y] = neighbor;
            }
            else
            {
                neighbor = Grid[x, y];
            }

            int newGCost = _Current._GCost + ((x != _Current._X && y != _Current._Y) ? 14 : 10);
            if (newGCost < neighbor._GCost)
            {
                neighbor._GCost = newGCost;
                neighbor._Parent = _Current;
                neighbor._FCost = neighbor._GCost + HCostCalculate(x, y);
                OpenSet.Enqueue(neighbor, neighbor._FCost);
            }
        }

        private static double HCostCalculate(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x - MapModel._End.x, 2) + Math.Pow(y - MapModel._End.y, 2));
        }

        private void ReconstructPath()
        {
            while ((_Current._X, _Current._Y) != MapModel._Departure)
            {
                Image[_Current._X, _Current._Y] = 'P';
                _Current = _Current._Parent;
            }
        }
    }

}