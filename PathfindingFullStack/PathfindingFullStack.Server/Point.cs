namespace PathfindingFullStack.Server
{
    public class Point
    {
        public int XPossiton { get; set; }
        public int YPossiton { get; set; }

        public int value { get; set; } 
        public Point(int x, int y)
        {
            XPossiton   = x;
            YPossiton = y;
            value = 0;
        }
    }
}
