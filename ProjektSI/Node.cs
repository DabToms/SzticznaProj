using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSI
{
    public class Node
    {
        public char znak;
        public int x;
        public int y;

        public Node(int x, int y, char znak)
        {
            this.znak=znak;
            this.x=x;
            this.y=y;
        }
    }
}
