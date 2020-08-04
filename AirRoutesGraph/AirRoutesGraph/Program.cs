using System;
using System.Collections.Generic;

/// <summary>
/// Find all routes from Point A to point B
/// </summary>
namespace AirRoutesGraph
{
    class AirMap
    {
        private Dictionary<string, Node> _nodeLookUp = new Dictionary<string, Node>();
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!");
            AirMap graph = new AirMap();
            graph.AddRoute("A", "B");
            graph.AddRoute("B", "A");
            graph.AddRoute("C", "D");
            graph.AddRoute("A", "C");
            graph.AddRoute("B", "C");
            graph.AddRoute("C", "B");
            graph.AddRoute("D", "B");
            graph.AddRoute("C", "A");
            graph.AddRoute("D", "A");
            graph.AddRoute("D", "E");
            graph.AddRoute("E", "B");            
            graph.AddRoute("C", "E");
            var check= graph.GetAllRoutes("A", "E");
            check.ForEach(dt =>
            {
                Console.WriteLine(dt);
            });
            if (check.Count == 0)
            {
                Console.WriteLine("No routes available");
            }
            Console.ReadLine();
        }

        private List<string> GetAllRoutes(string source, string destination)
        {
            List<string> allRoutes = new List<string>();
            var srcNode = GetNode(source);
            var destNode = GetNode(destination);
            //GetAllRouteRecursive(srcNode, destNode, allRoutes,new HashSet<string>(),source);
            GetAllRoutesBFS(srcNode, destNode, allRoutes);
            return allRoutes;
        }

        private void GetAllRoutesBFS(Node srcNode,Node destNode,List<string> allRoutes)
        {
            if (srcNode == null || destNode == null)
                return;

            Queue<KeyValuePair<string, Node>> queue = new Queue<KeyValuePair<string, Node>>();
            HashSet<string> visited = new HashSet<string>();

            queue.Enqueue(new KeyValuePair<string, Node>(srcNode.Route, srcNode));
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                
                if (curr.Value == destNode)
                {
                    allRoutes.Add(curr.Key);
                    
                }
                else
                {                    

                    visited.Add(curr.Value.Route);

                    foreach (var adj in curr.Value.Adjacent)
                    {
                        if (!visited.Contains(adj.Route))
                            queue.Enqueue(new KeyValuePair<string, Node>(curr.Key + "," + adj.Route, adj));
                    }                 
                }
                
            }

        }
        private void GetAllRouteRecursive(Node srcNode, Node destNode, List<string> allRoutes, HashSet<string> visited, string prefix)
        {
            if (srcNode == null || destNode == null)
                return;

            if (visited.Contains(srcNode.Route))
                return;

            visited.Add(srcNode.Route);

            if (srcNode == destNode)
            {
                allRoutes.Add(prefix);
            }
            else
            {
                foreach (var adj in srcNode.Adjacent)                
                    GetAllRouteRecursive(adj, destNode, allRoutes, visited, prefix + "," + adj.Route);                                   
            }
            visited.Remove(srcNode.Route);
        }

        void AddRoute(string source,string destination)
        {
            var srcnode=GetNode(source);
            if (srcnode == null)
            {
                srcnode = new Node(source);
                _nodeLookUp.Add(source, srcnode);
            }
            var destNode = GetNode(destination);
            if (destNode == null)
            {
                destNode = new Node(destination);
                _nodeLookUp.Add(destination, destNode);
            }
            srcnode.AddAdjacent(destNode);

        }

        private Node GetNode(string route)
        {
            if (_nodeLookUp.ContainsKey(route))
                return _nodeLookUp[route];
            else
                return null;
        }
    }

    public class Node
    {
        public string Route;
        public List<Node> Adjacent = new List<Node>();
        public Node(string route)
        {
            Route = route;
        }

        internal void AddAdjacent(Node node)
        {
            if (!Adjacent.Contains(node))
                Adjacent.Add(node);
        }
    }
    
}
