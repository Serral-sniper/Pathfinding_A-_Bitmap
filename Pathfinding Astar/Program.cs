using Pathfinding_Astar;
using System.Drawing;
int wall = 0;
bool OK = false;
try
{
    Console.Write("Combien de mur voulez-vous sur la map? : ");
    wall = int.Parse(Console.ReadLine());
    Console.WriteLine();
    OK = true;
}
catch
{
    wall = 0;
}
Console.Clear();
Astar astar = new Astar(MapModel.mapGeneration(wall));
