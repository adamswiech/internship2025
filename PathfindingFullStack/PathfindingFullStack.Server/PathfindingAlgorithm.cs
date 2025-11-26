using System.Numerics;

namespace PathfindingFullStack.Server
{
    public static class PathfindingAlgorithm
    {
        public static List<Point> FindPath(int width, int height ,Point startingPosition,Point targetPosition,List<Point> obstacle)
        {
            List<Point> board = new List<Point>();
            //int[][] board = new int[10][];
            int[,] directions = new int[,] {
                {-1,-1},{0,-1},{-1,0},{1,0},{-1,1},{0,1},{1,-1},{1,1}
            };

            //int[] startingPosition = new int[2];
            //int[] targetPosition = new int[2];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(obstacle.Any(p=>p.XPosition==i && p.YPosition==j))
                    {
                        board.Add(new Point { XPosition = i, YPosition = j, value = 1 });
                        continue;
                    }
                    board.Add(new Point(i, j));
                }
            }
            //startingPosition[0] = 0;
            //startingPosition[1] = 0;
            //targetPosition[0] = 9;
            //targetPosition[1] = 6;

            Console.WriteLine(startingPosition.XPosition + " " + startingPosition.YPosition);
            if (board[board.FindIndex(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition)].value == 0 
                && board[board.FindIndex(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition)].value == 0)
            {

                List<nod> openList = new List<nod>();
                List<nod> closedList = new List<nod>();
                nod startNode = new nod(startingPosition.XPosition, startingPosition.YPosition, 0, Math.Abs(targetPosition.XPosition - startingPosition.XPosition) + Math.Abs(targetPosition.YPosition - startingPosition.YPosition), null);
                openList.Add(startNode);
                bool pathFound = false;
                while (openList.Count > 0)
                {
                    openList = openList.OrderBy(n => n.f).ToList();
                    nod currentNode = openList[0];
                    openList.RemoveAt(0);
                    closedList.Add(currentNode);
                    if (currentNode.x == targetPosition.XPosition && currentNode.y == targetPosition.YPosition)
                    {
                        pathFound = true;
                        break;
                    }
                    for (int i = 0; i < directions.GetLength(0); i++)
                    {
                        int newX = currentNode.x + directions[i, 0];
                        int newY = currentNode.y + directions[i, 1];

                       
                        bool isDiagonal = Math.Abs(directions[i, 0]) == 1 && Math.Abs(directions[i, 1]) == 1;

                        if (isDiagonal)
                        {
                            Point check1 = new Point(currentNode.x + directions[i, 0], currentNode.y);
                            Point check2 = new Point(currentNode.x, currentNode.y + directions[i, 1]);
                       
                            if (obstacle.Any(p => p.XPosition == check1.XPosition && p.YPosition == check1.YPosition) ||
                                obstacle.Any(p => p.XPosition == check2.XPosition && p.YPosition == check2.YPosition))
                            {
                                continue;
                            }
                        }    
                        if (newX >= 0 && newX < width && newY >= 0 && newY < height && 
                            board[board.FindIndex(p => p.XPosition == newX && p.YPosition == newY)].value == 0)
                        {
                            if (closedList.Any(n => n.x == newX && n.y == newY))
                                continue;
                            int gCost = currentNode.g + 1;
                            int hCost = Math.Abs(targetPosition.XPosition - newX) + Math.Abs(targetPosition.YPosition - newY);
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
                    nod pathNode = closedList.First(n => n.x == targetPosition.XPosition && n.y == targetPosition.YPosition);
                    while (pathNode != null)
                    {
                        board[board.FindIndex(p => p.XPosition == pathNode.x && p.YPosition == pathNode.y)].value = 2;
                        pathNode = pathNode.parent;
                    }
                    board[board.FindIndex(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition)].value = 0;
                    board[board.FindIndex(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition)].value = 0;
                }
                
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.Write(board[board.FindIndex(p => p.XPosition == i && p.YPosition == j)].value + " ");
                }
                Console.WriteLine();
            }
            return board;
        }
    }
}
