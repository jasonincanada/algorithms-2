using System;
using System.Collections.Generic;

namespace jrh.Algorithms.Kosaraju
{
    class Kosaraju
    {        
        private Graph _graph;
        private Node _leader;
        private List<Node> _finishingOrder;

        Func<Node, List<Node>> _targetGetter;
        private bool _firstPass;

        public int NodesMarked { get; set;}

        public Kosaraju(Graph graph)
        {
            _graph = graph;
        }

        public Dictionary<Node, List<Node>> FindSCCs()
        {
            // The initial processing order is arbitrary... just use the order they were added to the graph
            IEnumerable<Node> processingOrder = _graph.EnumerableNodes();

            Console.WriteLine("DFS Pass 1...");
            _firstPass = true;
            _targetGetter = (n => n.ArrowTargets);
            DFSLoop(processingOrder);

            Console.WriteLine("DFS Pass 2...");
            _firstPass = false;
            _graph.SetUnexplored();            
            _finishingOrder.Reverse();
            _targetGetter = (n => n.ArrowSources);
            DFSLoop(_finishingOrder);

            return _graph.GetSCCs();
        }

        void DFSLoop(IEnumerable<Node> processingOrder)
        {             
            _leader = null;
            _finishingOrder = new List<Node>();

            foreach (Node node in processingOrder)
            {
                if (node.Explored)
                    continue;
                
                _leader = node;
                
                DFS(node);
            }            
        }

        void DFS(Node node)
        {
            node.SetExplored();
            
            NodesMarked++;
            if (NodesMarked % 1000 == 0)
                Console.WriteLine("Marked the {0}th node", NodesMarked);

            if (!_firstPass)
               _graph.AddNodeToSCC(_leader, node);

            foreach (Node target in _targetGetter(node))
            {
                if (target.Explored)
                    continue;
                
                DFS(target);
            }

            if (_firstPass)
                _finishingOrder.Add(node);
        }
    } 
}