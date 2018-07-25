using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using System.Diagnostics;
using System.Data;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new Stopwatch();
            w.Start();
            //var NormalisedGraph = ReadActiveStatedRelationships(@"C:\sct2_Relationship_Snapshot_AU1000036_20180731.txt");
            var NormalisedGraph = ReadActiveStatedRelationships(@"C:\sct2_StatedRelationship_Snapshot_INT_20180131.txt");
            long rootNode = 30515011000036103;

            //var NormalisedGraph = new Graph();
            //NormalisedGraph.Add('B', 'A');
            //NormalisedGraph.Add('C', 'A');
            //NormalisedGraph.Add('D', 'C');
            //NormalisedGraph.Add('E', 'C');
            //NormalisedGraph.Add('D', 'B');
            //var rootNode = 'A';

            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());           


            
            w.Restart();
            var TransitiveClosure = NormalisedGraph.CalculateTransitiveClosure(rootNode);
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());

            Console.WriteLine("There are {0} edges in the original graph", NormalisedGraph.table.Rows.Count.ToString());
           

            Console.WriteLine("There are {0} edges in the transitive closure", TransitiveClosure.table.Rows.Count.ToString());
        

            Console.WriteLine("Done.");
            Console.ReadKey();
        }


        [DelimitedRecord("\t")]
        [IgnoreFirst]
        public class Relationship
        {
            public long id;
            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public DateTime effectiveTime;
            public bool active;
            public long moduleId;
            public long sourceId;
            public long destinationId;
            public long relationshipGroup;
            public long typeId;
            public long characteristicTypeId;
            public long modifierId;
        }


        internal static Graph ReadActiveStatedRelationships(string path)
        {

            var relationships = new FileHelperAsyncEngine<Relationship>();
            var G = new Graph();

            using (relationships.BeginReadFile(path))
            {
                foreach (Relationship r in relationships)
                {
                    if (r.active && r.typeId == 116680003) //active IS A relationships
                    {
                        G.Add(r.sourceId, r.destinationId);
                    }
                }
            }
            Console.WriteLine("Stated Relationships Read");
            return G;
        }

    }

    public class DirectedEdge
    {
        public object source;
        public object destination;

        public DirectedEdge(object source, object destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override string ToString()
        {
            return source.ToString() + ' ' + destination.ToString();
        }
    }

    public class Graph
    {
        internal DataTable table;

        public Graph()
        {   
            // Create a new DataTable.
            table = new DataTable("Graph");
            // Declare variables for DataColumn and DataRow objects.
            
            DataColumn source = new DataColumn("source", Type.GetType("System.Object"));
            DataColumn dest = new DataColumn("destination", Type.GetType("System.Object"));
            table.Columns.Add(source);
            table.Columns.Add(dest);            
        }

        public void Add(object s, object d)
        {
            var row = this.table.NewRow();
            row["source"] = s;
            row["destination"] = d;
            table.Rows.Add(row);
        }

        public void OutputGraph()
        {
            //output graph
            foreach (var item in this.table.AsEnumerable())
            {
                Console.WriteLine(item["source"].ToString() + " " + item["destination"].ToString());
            }
        }

        //DFS method
        public Graph CalculateTransitiveClosure(object root)
        {
            var TransitiveClosure = new Graph();
            var nodesToExplore = new Stack<object>();
            var exploredNodes = new HashSet<object>();

            nodesToExplore.Push(root);

            while (nodesToExplore.Count > 0)
            {
                var parent = nodesToExplore.Pop();
                exploredNodes.Add(parent);

                foreach (var childEdge in this.table.Select().Where(e => e["destination"].Equals(parent)))
                {
                    TransitiveClosure.Add(childEdge["source"], childEdge["destination"]);       //add the statedEdge
                    var childNode = childEdge["source"];

                    //add all the parents TC edges to the childNode                
                    foreach (var ancestorEdge in TransitiveClosure.table.Select().Where(e => e["source"].Equals(parent)))
                    {
                        TransitiveClosure.Add(childNode, ancestorEdge["destination"]); // AncestorEdges 


                        if (exploredNodes.Contains(childNode))
                        {
                            // for all the childnodes descendents, add new ancestor parents too
                            foreach (var descendentEdge in TransitiveClosure.table.Select().Where(e => e["destination"].Equals(childNode)))
                            {
                                TransitiveClosure.Add(descendentEdge["source"], ancestorEdge["destination"]); // AncestorEdges
                            }
                        }
                    }

                    if (!exploredNodes.Contains(childNode))
                    {
                        nodesToExplore.Push(childNode);
                    }



                }

            }

            return TransitiveClosure;
        }

    }
}
////if the node has been explored, add all its edges to its descedents
//     if (exploredNodes.Contains(childNode))
//     {
//         foreach (var descendentEdge in TransitiveClosure.Where(e => e.destination.Equals(childNode)).ToArray())
//         {
//             TransitiveClosure.Add(new DirectedEdge(descendentEdge.source, ancestorEdge.destination)); // AncestorEdges
//         }
//     }
//     else
//     {
//         nodesToExplore.Push(childNode);
//     }