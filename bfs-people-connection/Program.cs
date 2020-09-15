using System;
using System.Collections.Generic;

namespace bfs_people_connection
{
    static class Program
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
            var test=cp.GetShortestConnection("simon","sam");
            if(string.IsNullOrWhiteSpace(test) || !test.Contains("=>"))
                Console.WriteLine("No connection exits");
            else
                Console.WriteLine($"Shortest connection: {test}");
            Console.ReadLine();
        }
    }
    public class ConnectPeople{
        private Dictionary<string,Node> _nodeLookup=new Dictionary<string, Node>();
        private Node GetNode(string name){
            if(string.IsNullOrWhiteSpace(name))
                return null;
            var nameLowerCase=name.Trim().ToLower();
            if(_nodeLookup.ContainsKey(nameLowerCase)){
                return _nodeLookup[nameLowerCase];
            }
            return null;
        }
        private Node AddNode(string name){
            if(string.IsNullOrWhiteSpace(name))
                return null;
            var nameLowerCase=name.Trim().ToLower();
            if(!_nodeLookup.ContainsKey(nameLowerCase)){
                _nodeLookup.Add(nameLowerCase,new Node(name));
            }
            return _nodeLookup[nameLowerCase];
        }
        public void AddConnection(string name1,string name2){
            if(string.IsNullOrWhiteSpace(name1) || string.IsNullOrWhiteSpace(name2))
                return;
             var nd1=GetNode(name1);
             if(nd1==null)
                nd1=AddNode(name1);
            
            var nd2=GetNode(name2);
            if(nd2==null)
                nd2=AddNode(name2);

            nd1.AddAdjacent(nd2);
            nd2.AddAdjacent(nd1);
        }

        public string GetShortestConnection(string name1,string name2){
            if(string.IsNullOrWhiteSpace(name1) || string.IsNullOrWhiteSpace(name2))
                return null;
            
            var srcNode=GetNode(name1);
            var destNode=GetNode(name2);

            if(srcNode==null || destNode==null){
                return null;
            }
            string ret="";
            HashSet<Node> visited=new HashSet<Node>();
            Queue<KeyValuePair<string,Node>> queueToVisit=new Queue<KeyValuePair<string, Node>>();
            queueToVisit.Enqueue(new KeyValuePair<string, Node>(srcNode.Name,srcNode));

            while(queueToVisit.Count>0){
                var item=queueToVisit.Dequeue();
                if(item.Value==destNode){
                    ret=item.Key;
                    break;
                }
                if(visited.Contains(item.Value)){
                    continue;
                }
                foreach(var adj in item.Value.Adjacents){
                    queueToVisit.Enqueue(new KeyValuePair<string, Node>(item.Key+" => "+adj.Name,adj));
                }
            }
            return ret;
        }

        private class Node{
            public string Name=> _name;
            public IEnumerable<Node> Adjacents=>_adjacents;
            private string _name;
            private List<Node> _adjacents=new List<Node>();
            internal Node(string name){
                _name=name;
            }

            internal void AddAdjacent(Node nd){
                if(!_adjacents.Contains(nd)){
                    _adjacents.Add(nd);
                }
            }
        }
    }
  
}
