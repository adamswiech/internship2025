namespace PathfindingFullStack.Server
{
    public class nod
    {
        public int x;
        public int y;
        public int z;
        public int g;
        public int h;
        public int f;
        //public int a;
        public nod parent;
        public nod(int x, int y,int z, int g, int h, nod parent)
        {
            this.x = x;
            this.y = y;
            this.g = g;
            this.h = h;
            this.f = g + h;
            this.z = z;
           // this.a = z + g + h;
            this.parent = parent;
        }
    }
}
