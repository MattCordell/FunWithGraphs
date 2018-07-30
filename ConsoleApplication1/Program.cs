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
            //var NormalisedGraph = ReadActiveStatedRelationships(@"C:\sct2_StatedRelationship_Snapshot_INT_20180131.txt");
            //long rootNode = 30515011000036103;

            //"Progeny" Adjacency list with Key = parent nodes, and value = children
            var Progeny = new AdjacenyList();
            Progeny.AddEdge('A', 'B');
            Progeny.AddEdge('A', 'C');
            Progeny.AddEdge('C', 'D');
            Progeny.AddEdge('C', 'E');
            Progeny.AddEdge('B', 'D');
            var rootNode = 'A';

            Console.WriteLine("There are {0} edges in the original graph", Progeny.CountEdges());

            Progeny.OutPutGraph();

            var tc = new AdjacenyList();
            var nodesToExplore = new Stack<object>();
            var Explored = new HashSet<object>();
            var path = new Stack<object>();
            var found = false;
            nodesToExplore.Push(rootNode);

            while (nodesToExplore.Count > 0)
            {
                var n = nodesToExplore.Pop();
                

                if (Explored.Contains(n))
                {
                    //Add it to everything in the path

                    if (Progeny.HasChildren(n))
                    {                        
                        //Add its subtypes to every node in path
                    }
                }

                // if not explored, queue up the children for exploration, and add parent to path
                else 
                {
                    if (Progeny.HasChildren(n))
                    {
                        path.Push(n);
                        //add all children to nodesToExplore
                        //add n to path
                    }
                    //if a terminal node is encountered, wind back the path.
                    //(keep popping until peek is in toExplore
                    else if (!Progeny.HasChildren(n))
                    {
                        
                    }
                    Explored.Add(n);
                }                               
                
                
            }

            Console.WriteLine("Found D : {0}", found.ToString());





            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        //AdjacenyList Key = sourceNode, value=hashSet of destinationNodes.
        //Class is for a directed Graph Key:
        //      Key = ParentNode for source-HasChild-destination ("progeny")
        //      Key = ChildNode  for source-hasParent-destination ("lineage")
        //Undirected graph, can be created by overiding AddEdge(s,d)
        public class AdjacenyList
        {
            public Dictionary<object, HashSet<object>> AL;

            public AdjacenyList()
            {                          
                AL = new Dictionary<object, HashSet<object>>();
            }

            public virtual void AddEdge(object source, object destination)
            {
                //create a new index if not already there
                if (!AL.ContainsKey(source))
                {
                    AL.Add(source, new HashSet<object>());                    
                }
                AL[source].Add(destination);
            }

            public int CountEdges()
            {
                int i = 0;
                foreach (var item in AL)
                {
                    i += item.Value.Count();
                }
                return i;
            }

            public void OutPutGraph()
            {
                foreach (var item in AL)
                {
                    foreach (var edge in item.Value)
                    {
                        Console.WriteLine("{0} - {1}", item.Key.ToString(), edge.ToString());
                    }

                }
            }

            internal IEnumerable<object> ReturnChildren(object parentNode)
            {
                    return AL[parentNode];                         
            }

            public bool HasChildren(object node)
            {
                return AL.ContainsKey(node);
            }
        }

        //For undirectedGraph
        public class UndirectedAdjacencyList : AdjacenyList
        {
            public override void AddEdge(object node1, object node2)
            {
                //create a new index if not already there
                if (!AL.ContainsKey(node1))
                {
                    AL.Add(node1, new HashSet<object>());
                }
                if (!AL.ContainsKey(node2))
                {
                    AL.Add(node2, new HashSet<object>());
                }

                AL[node1].Add(node2);
                AL[node2].Add(node1);
            }
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


        //internal static Graph ReadActiveStatedRelationships(string path)
        //{

        //    var relationships = new FileHelperAsyncEngine<Relationship>();
        //    var G = new Graph();

        //    using (relationships.BeginReadFile(path))
        //    {
        //        foreach (Relationship r in relationships)
        //        {
        //            if (r.active && r.typeId == 116680003) //active IS A relationships
        //            {
        //                G.Add(r.sourceId, r.destinationId);
        //            }
        //        }
        //    }
        //    Console.WriteLine("Stated Relationships Read");
        //    return G;
        //}

    }

        //DFS method
        //public Graph CalculateTransitiveClosure(object root)
        //{
        //    var TransitiveClosure = new Graph();
        //    var nodesToExplore = new Stack<object>();
        //    var exploredNodes = new HashSet<object>();

        //    nodesToExplore.Push(root);

        //    while (nodesToExplore.Count > 0)
        //    {
        //        var parent = nodesToExplore.Pop();
        //        exploredNodes.Add(parent);

        //        foreach (var childEdge in this.table.Select().Where(e => e["destination"].Equals(parent)))
        //        {
        //            TransitiveClosure.Add(childEdge["source"], childEdge["destination"]);       //add the statedEdge
        //            var childNode = childEdge["source"];

        //            //add all the parents TC edges to the childNode                
        //            foreach (var ancestorEdge in TransitiveClosure.table.Select().Where(e => e["source"].Equals(parent)))
        //            {
        //                TransitiveClosure.Add(childNode, ancestorEdge["destination"]); // AncestorEdges 


        //                if (exploredNodes.Contains(childNode))
        //                {
        //                    // for all the childnodes descendents, add new ancestor parents too
        //                    foreach (var descendentEdge in TransitiveClosure.table.Select().Where(e => e["destination"].Equals(childNode)))
        //                    {
        //                        TransitiveClosure.Add(descendentEdge["source"], ancestorEdge["destination"]); // AncestorEdges
        //                    }
        //                }
        //            }

        //            if (!exploredNodes.Contains(childNode))
        //            {
        //                nodesToExplore.Push(childNode);
        //            }



        //        }

        //    }

        //    return TransitiveClosure;
        //}

    //}
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