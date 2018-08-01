using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs.Tests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void BasicGraphTC()
        {   //Arrange


            //Act

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void BasicPolyHeirarchyTC()
        {   //Arrange
            //"Progeny" Adjacency list with Key = parent nodes, and value = children
            var Progeny = new AdjacenyList();

            /*
               A
              / \
              B  C
              \ / \
               D   E
            
            */

            Progeny.AddEdge('A', 'B');
            Progeny.AddEdge('A', 'C');
            Progeny.AddEdge('C', 'D');
            Progeny.AddEdge('C', 'E');
            Progeny.AddEdge('B', 'D');
            var rootNode = 'A';

            //Act

            //Assert
            Assert.Fail();//count of normalised edges = 5
            Assert.Fail();//count of transitive edges = 7
        }

        [TestMethod()]
        public void SomeOtherTest()
        {   //Arrange

            //Act

            //Assert
            Assert.Fail();
        }


    }
}