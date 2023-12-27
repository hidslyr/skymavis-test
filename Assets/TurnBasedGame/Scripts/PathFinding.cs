using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pathfinding
{
    private List<List<int>> grid;
    private int gridSize;

    public Pathfinding(List<List<int>> grid)
    {
        this.grid = grid;
        gridSize = grid.Count;
    }

    public List<(int, int)> FindPath(int startX, int startY, int targetX, int targetY)
    {
        bool[][] visited = new bool[gridSize][];
        for (int i = 0; i < gridSize; i++)
        {
            visited[i] = new bool[gridSize];
        }

        Stack<(int, int)> stack = new Stack<(int, int)>();
        Dictionary<(int, int), (int, int)> cameFrom = new Dictionary<(int, int), (int, int)>();

        stack.Push((startX, startY));
        visited[startX][startY] = true;

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            int x = current.Item1;
            int y = current.Item2;

            if (x == targetX && y == targetY)
            {
                return ReconstructPath(cameFrom, startX, startY, targetX, targetY);
            }

            foreach (var neighbor in GetNeighbors(x, y))
            {
                int nx = neighbor.Item1;
                int ny = neighbor.Item2;

                if (!visited[nx][ny])
                {
                    stack.Push((nx, ny));
                    visited[nx][ny] = true;
                    cameFrom[(nx, ny)] = (x, y);
                }
            }
        }

        return null;
    }

    private List<(int, int)> ReconstructPath(Dictionary<(int, int), (int, int)> cameFrom, int startX, int startY, int targetX, int targetY)
    {
        List<(int, int)> path = new List<(int, int)>();
        (int, int) current = (targetX, targetY);

        while (current != (startX, startY))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add((startX, startY));
        path.Reverse();
        return path;
    }

    private List<(int, int)> GetNeighbors(int x, int y)
    {
        List<(int, int)> neighbors = new List<(int, int)>();

        int[] xOffset = { 0, 0, 1, -1 };
        int[] yOffset = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int neighborX = x + xOffset[i];
            int neighborY = y + yOffset[i];

            if (neighborX >= 0 && neighborX < gridSize && neighborY >= 0 && neighborY < gridSize && grid[neighborX][neighborY] == 0)
            {
                neighbors.Add((neighborX, neighborY));
            }
        }

        return neighbors;
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

