namespace Pathfinding_Astar
{
    class Node
    {
        // Coordonnée X du nœud
        public int _X { get; private set; }
        // Coordonnée Y du nœud
        public int _Y { get; private set; }
        // Coût G : coût du chemin du point de départ à ce nœud
        public int _GCost { get; set; }
        // Coût F : somme du coût G et de l'estimation du coût jusqu'à la destination (heuristique)
        public double _FCost { get; set; }
        // Nœud parent dans le chemin
        public Node _Parent { get; set; }

        // Constructeur du nœud
        public Node(int x, int y)
        {
            _X = x;
            _Y = y;
            _GCost = int.MaxValue;
            _FCost = double.MaxValue;
            _Parent = this;
        }
    }
}
