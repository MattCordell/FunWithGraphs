using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using System.Diagnostics;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new Stopwatch();
            w.Start();
            //var NormalisedGraph = ReadActiveStatedRelationships(@"C:\sct2_Relationship_Snapshot_AU1000036_20180731.txt");
            var NormalisedGraph = new Graph();
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());

            NormalisedGraph.Add(new DirectedEdge('B', 'A'));
            NormalisedGraph.Add(new DirectedEdge('C', 'A'));
            NormalisedGraph.Add(new DirectedEdge('D', 'C'));
            NormalisedGraph.Add(new DirectedEdge('E', 'C'));
            var rootNode = 'A';

            Console.WriteLine(NormalisedGraph.First().ToString());
                

            //long rootNode = 138875005;
            w.Restart();
            var TransitiveClosure = NormalisedGraph.CalculateTransitiveClosure(rootNode);
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());

            Console.WriteLine("There are {0} edges in the original graph", NormalisedGraph.Count.ToString());
            NormalisedGraph.OutputGraph();

            Console.WriteLine("There are {0} edges in the transitive closure", TransitiveClosure.Count.ToString());
            TransitiveClosure.OutputGraph();

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
            var G = new Graph();
            

            var relationships = new FileHelperAsyncEngine<Relationship>();            

            using (relationships.BeginReadFile(path))
            {                
                foreach (Relationship r in relationships)
                {
                    if (r.active && r.typeId == 116680003) //active IS A relationships
                    {
                        G.Add(new DirectedEdge(r.sourceId, r.destinationId));
                    }                   
                }
            }
            Console.WriteLine("Stated Relationships Read");
            return G;
        }
        
    }

    class DirectedEdge
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

    class Graph : HashSet<DirectedEdge>
    {
        public void OutputGraph()
        {
            //output graph
            foreach (var item in this)
            {
                Console.WriteLine(item.ToString());
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
                foreach (var childEdge in this.Where(e => e.destination.Equals(parent)).ToArray())
                {
                    TransitiveClosure.Add(childEdge);       //add the statedEdge
                    var childNode = childEdge.source;

                    //add all the parents TC edges to the childNode                
                    foreach (var ancestorEdge in TransitiveClosure.Where(e => e.source.Equals(parent)).ToArray())
                    {
                        TransitiveClosure.Add(new DirectedEdge(childNode, ancestorEdge.destination)); // AncestorEdges                            
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