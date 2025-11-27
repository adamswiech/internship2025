using System.Numerics;

namespace PathfindingFullStack.Server
{
    public static class PathfindingAlgorithm
    {
        public static List<Point> FindPath(int width, int height, Point startingPosition, Point targetPosition, List<Point> board, List<Point> obstacle)
        {
            int[,] directions = new int[,] {
                {-1,-1},{0,-1},{-1,0},{1,0},{-1,1},{0,1},{1,-1},{1,1}
            };

            // Ustawienie przeszkód jako value = 1
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (obstacle.Any(p => p.XPosition == i && p.YPosition == j))
                    {
                        board[board.FindIndex(p => p.XPosition == i && p.YPosition == j)].value = 1;
                    }
                }
            }

            Console.WriteLine($"{startingPosition.XPosition} {startingPosition.YPosition}");

            // Jeśli start i meta nie są przeszkodami
            if (board[board.FindIndex(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition)].value == 0
                && board[board.FindIndex(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition)].value == 0)
            {

                List<nod> openList = new List<nod>();
                List<nod> closedList = new List<nod>();

                nod startNode = new nod(
                    startingPosition.XPosition,
                    startingPosition.YPosition,
                    0,
                    0,
                    Math.Abs(targetPosition.XPosition - startingPosition.XPosition)
                    + Math.Abs(targetPosition.YPosition - startingPosition.YPosition),
                    null
                );

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

                        // Blokada "przenikania" przez rogowe przeszkody
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

                        // Sprawdzenie czy w granicach
                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                        {
                            int boardIndex = board.FindIndex(p => p.XPosition == newX && p.YPosition == newY);

                            // 1 = przeszkoda
                            if (board[boardIndex].value == 1)
                                continue;

                            if (closedList.Any(n => n.x == newX && n.y == newY))
                                continue;

                            // NOWY KOSZT TERENOWY Z
                            int terrainZ = board[boardIndex].value;

                            // Z dodany do kosztu
                            int gCost = currentNode.g + 1 + terrainZ;

                            int hCost = Math.Abs(targetPosition.XPosition - newX) + Math.Abs(targetPosition.YPosition - newY);

                            nod neighborNode = new nod(newX, newY, terrainZ, gCost, hCost, currentNode);

                            var openNode = openList.FirstOrDefault(n => n.x == newX && n.y == newY);

                            if (openNode == null)
                            {
                                openList.Add(neighborNode);
                            }
                            else if (gCost < openNode.g)
                            {
                                openNode.g = gCost;
                                openNode.f = gCost + openNode.h + openNode.z;
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
                        var b = board[board.FindIndex(p => p.XPosition == pathNode.x && p.YPosition == pathNode.y)];
                        b.value = 2; // zaznaczanie ścieżki
                        pathNode = pathNode.parent;
                    }

                    // Czyszczenie start/meta do 0
                    board[board.FindIndex(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition)].value = 0;
                    board[board.FindIndex(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition)].value = 0;
                }

            }

            // Debug board
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
