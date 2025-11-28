namespace PathfindingFullStack.Server
{
    public class Point
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        
        public int ZPosition { get; set; }
        public int value { get; set; }
        public Point() { }
        public Point(int x, int y, int z = 0)
        {
            XPosition = x;
            YPosition = y;
            ZPosition = z;
        }
    }
}
