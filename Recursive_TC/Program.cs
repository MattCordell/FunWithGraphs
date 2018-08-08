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
            //w.Start();            
            //var Progeny = ReadActiveStatedRelationships(@"C:\sct2_Relationship_Snapshot_AU1000036_20180731.txt");
            //long rootNode = 30515011000036103; // AU Qualifier
            //long rootNode = 30561011000036101; //AU concept
            //long rootNode = 138875005;// SCT

            //"Progeny" Adjacency list with Key = parent nodes, and value = children
            var Progeny = new AdjacenyList();

            /*
               A
              / \
              B  C
              \ / \
               D   E
            
            */
            //aNCESTRY child, parent
            Progeny.AddEdge('B', 'A');
            Progeny.AddEdge('C', 'A');
            Progeny.AddEdge('D', 'B');
            Progeny.AddEdge('D', 'B');
            Progeny.AddEdge('E', 'C');
            var rootNode = 'A';

            Console.WriteLine("There are {0} edges in the original graph", Progeny.CountEdges());
            Console.WriteLine("Took {0}", w.ElapsedMilliseconds.ToString());
            w.Restart();
            //Progeny.OutPutGraph();

            var tc = new AdjacenyList();
            var nodesToExplore = new Stack<object>();
            var Explored = new HashSet<object>();
            var path = new Stack<object>();

            nodesToExplore.Push(rootNode);
            //path.Push(rootNode);
            int i = 0;

            while (nodesToExplore.Count > 0)
            {
                //
            }

            Console.WriteLine("TC calculation took {0}", w.ElapsedMilliseconds.ToString());
            w.Restart();
            //Console.WriteLine("Here's the TC:");
            //tc.OutPutGraph();
            Console.WriteLine("It's got {0} edges", tc.CountEdges().ToString());
            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        //try looping through dictionary until every hashtable contains root node.
        //would use a <source>,hasTable<ancestor>
        //for each entry
        //if !entry.hashtable.contains(root)
        //workToDo = true
        //     for each ancestor of entry
        //        entry.union.ancestor.values

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

        internal static AdjacenyList ReadActiveStatedRelationships(string path)
        {
            var relationships = new FileHelperAsyncEngine<Relationship>();
            var G = new AdjacenyList();
            using (relationships.BeginReadFile(path))
            {
                foreach (Relationship r in relationships)
                {
                    if (r.active && r.typeId == 116680003) //active IS A relationships
                    {
                        G.AddEdge(r.destinationId, r.sourceId);
                    }
                }
            }
            Console.WriteLine("Stated Relationships Read");
            return G;
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

    }
}
