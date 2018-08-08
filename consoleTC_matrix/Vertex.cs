using System;
using System.Collections.Generic;


namespace consoleTC_matrix
{
    class Vertex
    {
        int id;
        string name;
        List<Vertex> edge;

        public List<Vertex> Edge
        {
            get
            {
                return edge;
            }
            set
            {
                edge = value;
            }
        }

        public Vertex(
            int id,
            string name,
            List<Vertex> edge)
        {
            this.id = id;
            this.name = name;
            this.edge = edge;
        }

        public static bool[,] TransitiveClosure(
            List<Vertex> G, TextBox tb)
        {
            int n = G.Count;
            bool[,] T0 = new bool[n, n];
            bool[,] T1 = new bool[n, n];

            for (int i = 0; i < G.Count; i++)
            {
                Vertex vi = G[i];

                T0[vi.id, vi.id] = true;

                if (vi.Edge != null)
                {
                    for (int j = 0; j < vi.edge.Count; j++)
                    {
                        Vertex vj = vi.edge[j];

                        T0[vi.id, vj.id] = true;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                     if (T0[i, j])
                        tb.Text += "1";

                    else
                        tb.Text += "0";

                    tb.Text += "\t";
                }

                tb.Text += "\r\n";
            }
            
            tb.Text += "\r\n";

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        bool term = T0[i, k] && T0[k, j];

                        T1[i, j] = T0[i, j] || term;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        T0[i, j] = T1[i, j];

                        if (T0[i, j])
                            tb.Text += "1";

                        else
                            tb.Text += "0";

                        tb.Text += "\t";
                    }

                    tb.Text += "\r\n";
                }

                tb.Text += "\r\n";
            }

            return T0;
        }     
    }
}
