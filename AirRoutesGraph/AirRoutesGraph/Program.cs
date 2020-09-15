using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Find all routes from Point A to point B
/// </summary>
namespace AirRoutesGraph
{
   static class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!");
            AirMap graph = new AirMap();
            graph.AddRoute("A", "C");
            graph.AddRoute("C", "D");
            graph.AddRoute("A", "B");
            graph.AddRoute("B", "A");                        
            graph.AddRoute("B", "C");
            graph.AddRoute("C", "B");
            graph.AddRoute("D", "B");
            graph.AddRoute("C", "A");
            graph.AddRoute("D", "A");
            graph.AddRoute("D", "E");
            graph.AddRoute("E", "B");            
            graph.AddRoute("C", "E");
            var check= graph.GetAllRoutes("A", "D");
            foreach(var dt in check){                        
                Console.WriteLine(dt);
            }
            if (check==null || check.Count() == 0)
            {
                Console.WriteLine("No routes available");
            }            
        }
    }
    public class AirMap{

        private Dictionary<string,Node> _nodeLookUp=new Dictionary<string, Node>();

        private Node GetNode(string route){           
            var routeLoweCase=route.ToLower();
            if(_nodeLookUp.ContainsKey(routeLoweCase)){
                return _nodeLookUp[routeLoweCase];
            }
            return null;
        }

        private Node AddNode(string route){           
            var routeLoweCase=route.ToLower();
            if(!_nodeLookUp.ContainsKey(routeLoweCase)){
                _nodeLookUp.Add(routeLoweCase,new Node(route));    
            }
            return _nodeLookUp[routeLoweCase];
        }
        public void AddRoute(string source,string destination){
            if(string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
                return;
            
            var srcNd=GetNode(source);
            if(srcNd==null)
                srcNd=AddNode(source);

            var destNd=GetNode(destination);
            if(destNd==null)
                destNd=AddNode(destination);

            srcNd.AddAdjacent(destNd);
        }
        public IEnumerable<string> GetAllRoutes(string source,string destination){
            if(string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
                return null;
            List<string> allRoutes=new List<string>();
            CollectAllRoutesRecursive(GetNode(source),GetNode(destination),new HashSet<Node>(),allRoutes,source);
            return allRoutes;
        }

        private void CollectAllRoutesRecursive(Node srcNode,Node destNode,HashSet<Node> visited,List<string> routes,string currentRoute){
            if(srcNode==null || destNode==null)
                return;
            
            if(visited.Contains(srcNode))
                return;

            if(srcNode==destNode)
                routes.Add(currentRoute);
            
            visited.Add(srcNode);

            foreach(var adj in srcNode.Adjacents){
                CollectAllRoutesRecursive(adj,destNode,visited,routes,currentRoute+" => "+adj.Location);
            }
            visited.Remove(srcNode);
        }

        private class Node{
            private List<Node> _adj= new List<Node>();
            private string _location;
            internal string Location{
                get{
                    return _location;
                }
            }
            internal IEnumerable<Node> Adjacents=>_adj;
            internal Node(string location){
                _location=location;
            }

            internal void AddAdjacent(Node nd){
                if(nd!=null && !_adj.Contains(nd)){
                    _adj.Add(nd);
                }

            }
        }
    }
}
        