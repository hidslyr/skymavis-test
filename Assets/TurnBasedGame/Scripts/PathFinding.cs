using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseGame
{
    class Pathfinding
    {
        private List<List<int>> grid;
        private int gridSizeX, gridSizeY;

        public Pathfinding(List<List<int>> grid)
        {
            this.grid = grid;
            gridSizeX = grid.Count;
            gridSizeY = grid[0].Count;
        }

        public List<(int, int)> FindPath((int, int) start, (int, int) end)
        {
            HashSet<(int, int)> closedSet = new HashSet<(int, int)>();
            HashSet<(int, int)> openSet = new HashSet<(int, int)>();
            openSet.Add(start);

            Dictionary<(int, int), (int, int)> cameFrom = new Dictionary<(int, int), (int, int)>();

            Dictionary<(int, int), int> gScore = new Dictionary<(int, int), int>();
            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    gScore[(row.IndexOf(cell), grid.IndexOf(row))] = int.MaxValue;
                }
            }
            gScore[start] = 0;

            Dictionary<(int, int), int> fScore = new Dictionary<(int, int), int>();
            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    fScore[(row.IndexOf(cell), grid.IndexOf(row))] = int.MaxValue;
                }
            }
            fScore[start] = HeuristicCostEstimate(start, end);

            while (openSet.Count > 0)
            {
                (int, int) current = GetLowestFScore(openSet, fScore);

                if (current == end)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbors(current, end))
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end);
                }
            }

            return null;
        }

        private List<(int, int)> ReconstructPath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
        {
            List<(int, int)> path = new List<(int, int)>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
            return path;
        }

        private int HeuristicCostEstimate((int, int) start, (int, int) end)
        {
            return Mathf.Abs(start.Item1 - end.Item1) + Mathf.Abs(start.Item2 - end.Item2);
        }

        private (int, int) GetLowestFScore(HashSet<(int, int)> openSet, Dictionary<(int, int), int> fScore)
        {
            int min = int.MaxValue;
            (int, int) minNode = (0, 0);
            foreach (var node in openSet)
            {
                if (fScore[node] < min)
                {
                    min = fScore[node];
                    minNode = node;
                }
            }
            return minNode;
        }

        private List<(int, int)> GetNeighbors((int, int) cell, (int, int) end)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            List<(int, int)> neighbors = new List<(int, int)>();

            for (int i = 0; i < 4; i++)
            {
                int x = cell.Item1 + dx[i];
                int y = cell.Item2 + dy[i];

                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY &&
                    ((x == end.Item1 && y == end.Item2) || grid[x][y] == 0))
                {
                    neighbors.Add((x, y));
                }
            }
            return neighbors;
        }
    }
}
// Usage Example:
//class Program
//{
//    static void Main()
//    {
//        int[,] grid = new int[,]
//        {
//            { 0, 0, 0, 0, 0 },
//            { 0, 1, 1, 1, 0 },
//            { 0, 0, 0, 0, 0 },
//            { 0, 1, 1, 1, 0 },
//            { 0, 0, 0, 0, 0 }
//        };

//        Pathfinding pathfinder = new Pathfinding(grid);
//        List<Node> path = pathfinder.FindPath(0, 0, 4, 4);

//        if (path != null)
//        {
//            foreach (Node node in path)
//            {
//                Console.WriteLine($"({node.x}, {node.y})");
//            }
//        }
//        else
//        {
//            Console.WriteLine("No path found.");
//        }
//    }
//}

