namespace PathfindingFullStack.Server
{
    public class Point
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public int value { get; set; } 
        public Point(int x, int y)
        {
            XPosition   = x;
            YPosition = y;
            value = 0;
        }
    }
}
