using System;
using System.Collections.Generic;

namespace jrh.Algorithms.Kosaraju
{
    class Kosaraju
    {
        private Graph _graph;
        private List<Node> _finishingOrder;

        Func<Node, List<Node>> _targetGetter;
        private bool _firstPass;

        public Kosaraju(Graph graph)
        {
            _graph = graph;
        }

        public Dictionary<Node, List<Node>> FindSCCs()
        {
            // The initial processing order is arbitrary... just use the order they are stored in the list
            IEnumerable<Node> processingOrder = _graph.EnumerableNodes();

            Console.WriteLine("DFS Pass 1...");
            _firstPass = true;
            _targetGetter = (n => n.ArrowSources);
            DFSLoop(processingOrder);

            Console.WriteLine("DFS Pass 2...");
            _firstPass = false;
            _graph.SetUnexplored();
            _finishingOrder.Reverse();
            _targetGetter = (n => n.ArrowTargets);
            DFSLoop(_finishingOrder);

            return _graph.GetSCCs();
        }

        void DFSLoop(IEnumerable<Node> processingOrder)
        {
            _finishingOrder = new List<Node>();

            foreach (Node node in processingOrder)
            {
                if (node.Explored)
                    continue;

                DFS(node);
            }
        }

        void DFS(Node leader)
        {
            var stack = new Stack<Tuple<Node, int>>();

            leader.SetExplored();
            stack.Push(new Tuple<Node, int>(leader, 0));

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var node = current.Item1;
                var index = current.Item2;
                var targets = _targetGetter(node);

                // Finished trying all of this node's outgoing arrows?
                if (index >= targets.Count)
                {
                    if (_firstPass)
                        _finishingOrder.Add(node);

                    continue;
                }

                // The next time we consider this node, look at the next arrow
                stack.Push(new Tuple<Node, int>(node, index + 1));

                var target = targets[index];

                if (target.Explored)
                    continue;

                // We've found an unexplored target node
                target.SetExplored();

                if (!_firstPass)
                    _graph.AddNodeToSCC(leader, target);

                stack.Push(new Tuple<Node, int>(target, 0));
            }
        }
    }
}
