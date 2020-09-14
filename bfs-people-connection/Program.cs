using System;
using System.Collections.Generic;

namespace bfs_people_connection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConnectPeople cp=new ConnectPeople();
            cp.AddConnection("Sam","Adam");
            cp.AddConnection("Adam","Mark");
            cp.AddConnection("Sam","Mark");
            cp.AddConnection("Adam","Simon");
            cp.AddConnection("Timothy","Simon");
            var test=cp.GetShortestConnection("Mark","Timothy");
            if(string.IsNullOrWhiteSpace(test) || !test.Contains("=>"))
                Console.WriteLine("No connection exits");
            else
                Console.WriteLine($"Shortest connection: {test}");
            Console.ReadLine();
        }
    }

    public class ConnectPeople{
        private Dictionary<string,Node> graph=new Dictionary<string, Node>();

        private Node GetNode(string name){
            if(graph.ContainsKey(name.ToLower()))
                return graph[name.ToLower()];
            else
                return null;
        }
        private Node AddNode(string name){
            graph.Add(name.ToLower(),new Node(name));
            return graph[name.ToLower()];
        }
        public void AddConnection(string name1,string name2){
            if(string.IsNullOrWhiteSpace(name1)|| string.IsNullOrWhiteSpace(name2))
                return;            
            Node nd1=GetNode(name1);
            if(nd1==null)
                nd1=AddNode(name1);
            Node nd2=GetNode(name2);
            if(nd2==null)
                nd2=AddNode(name2);
            nd1.AddConnection(nd2);
            nd2.AddConnection(nd1);
        }
        public string GetShortestConnection(string name1,string name2){
            if(string.IsNullOrWhiteSpace(name1)|| string.IsNullOrWhiteSpace(name2))
                return null;            
            var nd1=GetNode(name1);
            var nd2=GetNode(name2);
            if(nd1==null || nd2==null)
                return null;
            var connection=nd1.Name;
            Queue<KeyValuePair<string,Node>> queue=new Queue<KeyValuePair<string,Node>>();
            HashSet<Node> visited=new HashSet<Node>();
            queue.Enqueue(new KeyValuePair<string, Node>(connection,nd1));
            while(queue.Count>0){
                var kvp=queue.Dequeue();
                if(kvp.Value==nd2)
                {
                    connection = kvp.Key;
                    break;
                }
                if(visited.Contains(kvp.Value)){
                    continue;
                }
                visited.Add(kvp.Value);                
                foreach(var adj in kvp.Value.Adjacents){
                    queue.Enqueue(new KeyValuePair<string, Node>(kvp.Key+" => "+adj.Name,adj));
                }
            }
            return connection;
        }

         private class Node{
            internal IEnumerable<Node> Adjacents{
                get{
                    return _adj;
                }
            }
            internal string Name{
                get{
                    return _name;
                }
            }
            private List<Node> _adj=new List<Node>();
            private string _name;
            internal Node(string name){
                _name=name;
            }
            internal void AddConnection(Node nd){
                if(nd!=null){
                    if(!_adj.Contains(nd)){
                        _adj.Add(nd);
                    }
                }
            }
        }
    }
}
