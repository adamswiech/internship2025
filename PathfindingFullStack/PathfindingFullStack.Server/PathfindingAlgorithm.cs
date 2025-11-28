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

            
            foreach (var obs in obstacle)
            {
                var idx = board.FindIndex(p => p.XPosition == obs.XPosition && p.YPosition == obs.YPosition);
                if (idx != -1)
                    board[idx].value = 1; 
            }
            
            if (board.First(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition).value == 1 ||
                board.First(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition).value == 1)
            {
                return board;
            }

            List<nod> openList = new List<nod>();
            List<nod> closedList = new List<nod>();

            nod startNode = new nod(
                startingPosition.XPosition,
                startingPosition.YPosition,
                0,
                0,
                Math.Abs(targetPosition.XPosition - startingPosition.XPosition) +
                Math.Abs(targetPosition.YPosition - startingPosition.YPosition),
                null
            );

            startNode.f = startNode.g + startNode.h;
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
                        if (obstacle.Any(p => p.XPosition == currentNode.x + directions[i, 0] && p.YPosition == currentNode.y) ||
                            obstacle.Any(p => p.XPosition == currentNode.x && p.YPosition == currentNode.y + directions[i, 1]))
                        {
                            continue;
                        }
                    }

                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                        continue;

                    var boardItem = board.First(p => p.XPosition == newX && p.YPosition == newY);

                    if (boardItem.value == 1) 
                        continue;

                    if (closedList.Any(n => n.x == newX && n.y == newY))
                        continue;

                    int terrainZ = boardItem.ZPosition;

                    int gCost = currentNode.g + 1 + terrainZ;
                    int hCost = Math.Abs(targetPosition.XPosition - newX) + Math.Abs(targetPosition.YPosition - newY);

                    nod neighborNode = new nod(newX, newY, terrainZ, gCost, hCost, currentNode);
                    neighborNode.f = neighborNode.g + neighborNode.h;

                    var openNode = openList.FirstOrDefault(n => n.x == newX && n.y == newY);

                    if (openNode == null)
                    {
                        openList.Add(neighborNode);
                    }
                    else if (gCost < openNode.g)
                    {
                        openNode.g = gCost;
                        openNode.parent = currentNode;
                        openNode.f = openNode.g + openNode.h;
                    }
                }
            }

            
            if (pathFound)
            {
                var goalNode = closedList.First(n => n.x == targetPosition.XPosition && n.y == targetPosition.YPosition);

                while (goalNode != null)
                {
                    var b = board.First(p => p.XPosition == goalNode.x && p.YPosition == goalNode.y);
                    b.value = 2; 
                    goalNode = goalNode.parent;
                }

                board.First(p => p.XPosition == startingPosition.XPosition && p.YPosition == startingPosition.YPosition).value = 0;
                board.First(p => p.XPosition == targetPosition.XPosition && p.YPosition == targetPosition.YPosition).value = 0;
            }

            return board;
        }
    }


}
