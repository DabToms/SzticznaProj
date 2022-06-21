using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSI
{
    public static class Astar
    {
        public static matrixNode FindPath(Node[][] matrix, int fromX, int fromY, int toX, int toY, char znakPrzeszkody)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // in this version an element in a matrix can move left/up/right/down in one step, two steps for a diagonal move.
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //the keys for greens and reds are x.ToString() + y.ToString() of the matrixNode 
            Dictionary<string, matrixNode> greens = new Dictionary<string, matrixNode>(); //open 
            Dictionary<string, matrixNode> reds = new Dictionary<string, matrixNode>(); //closed 

            matrixNode startNode = new matrixNode { x = fromX, y = fromY };
            string key = startNode.x.ToString() + startNode.x.ToString();
            greens.Add(key, startNode);

            Func<KeyValuePair<string, matrixNode>> smallestGreen = () =>
            {
                KeyValuePair<string, matrixNode> smallest = greens.ElementAt(0);

                foreach (KeyValuePair<string, matrixNode> item in greens)
                {
                    if (item.Value.sum < smallest.Value.sum)
                        smallest = item;
                    else if (item.Value.sum == smallest.Value.sum
                            && item.Value.to < smallest.Value.to)
                        smallest = item;
                }

                return smallest;
            };


            //add these values to current node's x and y values to get the left/up/right/bottom neighbors
            List<KeyValuePair<int, int>> fourNeighbors = new List<KeyValuePair<int, int>>()
                                            { new KeyValuePair<int, int>(-1,0),
                                              new KeyValuePair<int, int>(0,1),
                                              new KeyValuePair<int, int>(1, 0),
                                              new KeyValuePair<int, int>(0,-1) };

            int maxX = matrix.GetLength(0);
            if (maxX == 0)
                return null;
            int maxY = matrix[0].Length;

            while (true)
            {
                if (greens.Count == 0)
                    return null;

                KeyValuePair<string, matrixNode> current = smallestGreen();
                if (current.Value.x == toX && current.Value.y == toY)
                    return current.Value;

                greens.Remove(current.Key);
                reds.Add(current.Key, current.Value);

                foreach (KeyValuePair<int, int> plusXY in fourNeighbors)
                {
                    int nbrX = current.Value.x + plusXY.Key;
                    int nbrY = current.Value.y + plusXY.Value;
                    string nbrKey = nbrX.ToString() + nbrY.ToString();
                    if (nbrX < 0 || nbrY < 0 || nbrX >= maxX || nbrY >= maxY
                        || matrix[nbrX][nbrY].znak == znakPrzeszkody //obstacles marked by 'X'
                        || reds.ContainsKey(nbrKey))
                        continue;

                    if (greens.ContainsKey(nbrKey))
                    {
                        matrixNode curNbr = greens[nbrKey];
                        int from = Math.Abs(nbrX - fromX) + Math.Abs(nbrY - fromY);
                        if (from < curNbr.fr)
                        {
                            curNbr.fr = from;
                            curNbr.sum = curNbr.fr + curNbr.to;
                            curNbr.parent = current.Value;
                        }
                    }
                    else
                    {
                        matrixNode curNbr = new matrixNode { x = nbrX, y = nbrY };
                        curNbr.fr = Math.Abs(nbrX - fromX) + Math.Abs(nbrY - fromY);
                        curNbr.to = Math.Abs(nbrX - toX) + Math.Abs(nbrY - toY);
                        curNbr.sum = curNbr.fr + curNbr.to;
                        curNbr.parent = current.Value;
                        greens.Add(nbrKey, curNbr);
                    }
                }
            }
        }
    }
}
