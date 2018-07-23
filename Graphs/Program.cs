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
            var NormalisedGraph = ReadActiveStatedRelationships(@"C:\sct2_Relationship_Snapshot_AU1000036_20180731.txt");
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());

            //NormalisedGraph.Add(new DirectedEdge('B', 'A'));
            //NormalisedGraph.Add(new DirectedEdge('C', 'A'));
            //NormalisedGraph.Add(new DirectedEdge('D', 'C'));
            //NormalisedGraph.Add(new DirectedEdge('E', 'C'));
            //var rootNode = 'A';

            Console.WriteLine(NormalisedGraph.First().ToString());
                

            long rootNode = 138875005;
            w.Restart();
            var TransitiveClosure = NormalisedGraph.CalculateTransitiveClosure(rootNode);
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds.ToString());

            Console.WriteLine("There are {0} edges in the original graph", NormalisedGraph.Count.ToString());
            Console.WriteLine("There are {0} edges in the transitive closure", TransitiveClosure.Count.ToString());

            //output graph
            //foreach (var item in TransitiveClosure)
            //{
            //    Console.WriteLine(item.ToString());
            //}

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


        //DFS method
        public Graph CalculateTransitiveClosure(object root)
        {
            var TransitiveClosure = new Graph();
            var UnexploredNodes = new Stack<object>();
            var exploredNodes = new List<object>();
           

            UnexploredNodes.Push(root);

            while (UnexploredNodes.Count > 0)
            {
                var parent = UnexploredNodes.Pop();
                foreach (var edge in this.Where(e => e.destination.Equals(parent)).ToArray())
                {
                    UnexploredNodes.Push(edge.source); //child node
                    TransitiveClosure.Add(edge);       //add the statedEdge
                    foreach (var transitiveEdge in TransitiveClosure.Where(e => e.source.Equals(parent)).ToArray())
                    {
                        TransitiveClosure.Add(new DirectedEdge(edge.source, transitiveEdge.destination)); // AncestorEdges
                    }
                }
                exploredNodes.Add(parent);
            }

            return TransitiveClosure;
        }

    }
}
