//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Pathfinding_Astar
//{
//    abstract class QuickSort
//    {
//        // Méthode de tri rapide pour trier les nœuds par leur coût F
//        public static Node Sort(List<Node> array, int leftIndex, int rightIndex)
//        {
//            int i = leftIndex;
//            int j = rightIndex;
//            double pivot = array[leftIndex]._FCost;

//            while (i <= j)
//            {
//                // Trouve un élément à gauche qui devrait être à droite
//                while (array[i]._FCost < pivot)
//                {
//                    i++;
//                }

//                // Trouve un élément à droite qui devrait être à gauche
//                while (array[j]._FCost > pivot)
//                {
//                    j--;
//                }

//                // Échange les éléments et déplace les index
//                if (i <= j)
//                {
//                    Node temp = array[i];
//                    array[i] = array[j];
//                    array[j] = temp;
//                    i++;
//                    j--;
//                }
//            }

//            // Tri récursif des sous-parties
//            if (leftIndex < j)
//                Sort(array, leftIndex, j);

//            if (i < rightIndex)
//                Sort(array, i, rightIndex);

//            // Retourne le premier élément (celui avec le coût F le plus bas)
//            return array[0];
//        }
//    }
//}