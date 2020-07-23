using System;
using System.Collections.Generic;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = PersonGenerator.Generate();
            var p2 = PersonGenerator.Generate();
            var p3 = PersonGenerator.Generate();
            var p4 = PersonGenerator.Generate();
            var p5 = PersonGenerator.Generate();
            var p6 = PersonGenerator.Generate();
            var p7 = PersonGenerator.Generate();

            var dual = true;
            Graph gp = new Graph(dual);
            Console.WriteLine($"Dual Graph ?: {dual}");
            Console.WriteLine($"Addition People Connection (Graph Edges): {DateTime.Now.ToShortTimeString()}");
            gp.AddConnection(p1, p3);        
            gp.AddConnection(p3, p5);
            gp.AddConnection(p2, p3);
            gp.AddConnection(p2, p6);
            gp.AddConnection(p5, p7);            
            gp.AddConnection(p7, p6);
            gp.AddConnection(p6, p4);

            Console.WriteLine($"Completed People Connection (Graph Edges): {DateTime.Now.ToShortTimeString()}");
            Console.WriteLine("Checking p1 to p6 connection");
            Console.WriteLine($"Using DFS Exists?: {gp.ConnectionExists(p1, p6)}");
            Console.WriteLine($"Using BFS Shortest: {gp.ShortestConnection(p1, p6)}");

            Console.WriteLine("Press Key to Exit");
            Console.ReadKey();
        }
        
    }
    public static class PersonGenerator
    {
        private static long personId;
        public static Person Generate()
        {
            personId++;
            return new Person(personId, "P" + personId, "");
        }
    }
    public class Person
    {
        public long Id { get; }

        public string FirstName { get; }

        public string LastName { get; }     

        public Person(long id,string firstName,string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
    public class Graph
    {
        private bool _dual;
        public Graph(bool dual)
        {
            _dual = dual;
        }
        private Dictionary<long, Node> nodeLookup = new Dictionary<long, Node>();

        private Node GetNode(long id)
        {
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id];
            else
                return null;
        }
        public void AddConnection(Person p1,Person p2)
        {
            var node = GetNode(p1.Id);
            if (node == null)
            {
                node = new Node(p1.Id, p1);
                nodeLookup.Add(p1.Id, node);
            }
            var node2= GetNode(p2.Id);
            if (node2 == null)
            {
                node2 = new Node(p2.Id, p2);
                nodeLookup.Add(p2.Id, node2);
            }
            node.Adjacent.Add(node2);
            if(_dual) node2.Adjacent.Add(node);                        
        }
        public bool ConnectionExists(Person p1,Person p2)
        {
            HashSet<long> visited = new HashSet<long>();
            if (p1 == null || p2 == null || p1.Id == p2.Id)
                return false;

            return DFSConnectionExits(GetNode(p1.Id), GetNode(p2.Id), visited);
        }
        private static bool DFSConnectionExits(Node node1, Node node2, HashSet<long> visited)
        {
            if (node1 == null || node2 == null)
                return false;
            if (visited.Contains(node1.Id))
                return false;
            visited.Add(node1.Id);
            if (node1.Id == node2.Id)
                return true;

            foreach (var ad in node1.Adjacent)
            {
                if (DFSConnectionExits(ad, node2, visited))
                    return true;
            }
            return false;

        }
        public string ShortestConnection(Person p1,Person p2)
        {
            var ret = "";
            if (p1 == null || p2 == null || p1.Id == p2.Id)
                return ret;
            if(BFSGetShortConnection(GetNode(p1.Id), GetNode(p2.Id),out ret)) ret=ret.Trim();
            else ret = "No Connection";
                       
            return ret;
        }

        private static bool BFSGetShortConnection(Node node1,Node node2,out string shortestConnection)
        {
            shortestConnection = "";
            if (node1 == null || node2 == null)
                return false;
            HashSet<long> visited = new HashSet<long>();
            Queue <Tuple<string,Node>> queue = new Queue<Tuple<string,Node>>();
            queue.Enqueue(new Tuple<string,Node>(node1.Person.FirstName ,node1));

            while (queue.Count>0)
            {
                var nd = queue.Dequeue();
                visited.Add(nd.Item2.Id);               
                if (nd.Item2.Id == node2.Id)
                {
                    shortestConnection = nd.Item1;
                    return true;
                }
                else
                {                   
                    nd.Item2.Adjacent.ForEach(adj =>
                    {
                        if(!visited.Contains(adj.Id))
                            queue.Enqueue(new Tuple<string,Node>(nd.Item1+" => "+adj.Person.FirstName, adj));
                    });
                }
            }

            return false;
        }

        private class Node
        {            
            public Person Person;
            public long Id;            
            public List<Node> Adjacent = new List<Node>();
            public Node(long id, Person person)
            {
                Id = id;
                Person = person;
            }
        }
    }
}
