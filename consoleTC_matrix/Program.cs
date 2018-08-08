using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleTC_matrix
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private void CreateGraph()
        {
            Vertex v1 = new Vertex(0, "1", null);
            Vertex v2 = new Vertex(1, "2", null);
            Vertex v3 = new Vertex(2, "3", null);
            Vertex v4 = new Vertex(3, "4", null);

            List<Vertex> e2 = new List<Vertex>();
            List<Vertex> e3 = new List<Vertex>();
            List<Vertex> e4 = new List<Vertex>();

            e2.Add(v3);
            e2.Add(v4);

            e3.Add(v2);

            e4.Add(v1);
            e4.Add(v3);

            v2.Edge = e2;
            v3.Edge = e3;
            v4.Edge = e4;

            List<Vertex> G = new List<Vertex>();

            G.Add(v1);
            G.Add(v2);
            G.Add(v3);
            G.Add(v4);

            bool[,] T = Vertex.TransitiveClosure(G, textBox1);
        }
    }
}
