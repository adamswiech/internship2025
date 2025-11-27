namespace PathfindingFullStack.Server
{
    public class Data
    {
        public int width { get; set; }
        public int height { get; set; }
        public Point start { get; set; }
        public Point end { get; set; }
        public List<Point> allfields { get; set; }
        public List<Point> obstacles { get; set; }
    }
}
