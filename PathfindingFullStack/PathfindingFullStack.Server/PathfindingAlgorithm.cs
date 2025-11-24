using System.Numerics;

namespace PathfindingFullStack.Server
{
    public static class PathfindingAlgorithm
    {
        public static List<Point> FindPath(int width, int height)
        {
            List<Point> board = new List<Point>();
            //int[][] board = new int[10][];
            int[,] directions = new int[,] {
                {-1,-1},{0,-1},{-1,0},{1,0},{-1,1},{0,1},{1,-1},{1,1}
            };

            int[] startingPosition = new int[2];
            int[] targetPosition = new int[2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board.Add(new Point(i, j));
                }
            }
            startingPosition[0] = 0;
            startingPosition[1] = 0;
            targetPosition[0] = 8;
            targetPosition[1] = 6;

            
            if (board[board.FindIndex(p => p.XPosition == startingPosition[0] && p.YPosition == startingPosition[1])].value == 0 
                && board[board.FindIndex(p => p.XPosition == targetPosition[0] && p.YPosition == targetPosition[1])].value == 0)
            {

                List<nod> openList = new List<nod>();
                List<nod> closedList = new List<nod>();
                nod startNode = new nod(startingPosition[0], startingPosition[1], 0, Math.Abs(targetPosition[0] - startingPosition[0]) + Math.Abs(targetPosition[1] - startingPosition[1]), null);
                openList.Add(startNode);
                bool pathFound = false;
                while (openList.Count > 0)
                {
                    openList = openList.OrderBy(n => n.f).ToList();
                    nod currentNode = openList[0];
                    openList.RemoveAt(0);
                    closedList.Add(currentNode);
                    if (currentNode.x == targetPosition[0] && currentNode.y == targetPosition[1])
                    {
                        pathFound = true;
                        break;
                    }
                    for (int i = 0; i < directions.GetLength(0); i++)
                    {
                        int newX = currentNode.x + directions[i, 0];
                        int newY = currentNode.y + directions[i, 1];
                        if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10 && 
                            board[board.FindIndex(p => p.XPosition == newX && p.YPosition == newY)].value == 0)
                        {
                            if (closedList.Any(n => n.x == newX && n.y == newY))
                                continue;
                            int gCost = currentNode.g + 1;
                            int hCost = Math.Abs(targetPosition[0] - newX) + Math.Abs(targetPosition[1] - newY);
                            nod neighborNode = new nod(newX, newY, gCost, hCost, currentNode);
                            var openNode = openList.FirstOrDefault(n => n.x == newX && n.y == newY);
                            if (openNode == null)
                            {
                                openList.Add(neighborNode);
                            }
                            else if (gCost < openNode.g)
                            {
                                openNode.g = gCost;
                                openNode.f = gCost + hCost;
                                openNode.parent = currentNode;
                            }
                        }
                    }
                }
                if (pathFound)
                {
                    nod pathNode = closedList.First(n => n.x == targetPosition[0] && n.y == targetPosition[1]);
                    while (pathNode != null)
                    {
                        board[board.FindIndex(p => p.XPosition == pathNode.x && p.YPosition == pathNode.y)].value = 2;
                        pathNode = pathNode.parent;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(board[board.FindIndex(p => p.XPosition == i && p.YPosition == j)].value + " ");
                    }
                    Console.WriteLine();
                }
            }
            return board;
        }
    }
}
