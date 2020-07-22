using System ;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trie
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var tr = new Trie();
            Console.WriteLine($"Start Time for Trie construction {DateTime.Now.ToString("HH:mm:ss")}");
            tr.AddData(new List<string> { "Cat","Calf", "Call", "Cab","Calf","Calves" ,"BAt","bat","Ball","ball","bumper","No","Now"});
            //var ran = new Random().Next(100,1000);
            //var lclList = new List<string>();
            //for (var i = 1; i <= ran; i++)
            //{
            //    lclList.Add("test" + i);
            //}

            //tr.AddData(lclList);
            Console.WriteLine($"End Time {DateTime.Now.ToString("HH:mm:ss")}");
            var readValue = "";
            Console.WriteLine("Type search text and press enter...(esc to quit)");
            while (true)
            {                
                var info=Console.ReadKey();               
                if (info.Key == ConsoleKey.Enter)
                {
                    
                    Console.WriteLine($"Searching Text...{readValue}");
                    var stTm = DateTime.Now.ToString("HH:mm:ss");                                       
                    var res = tr.FindAllMatch(readValue);
                    var enTm = DateTime.Now.ToString("HH:mm:ss");                    
                    readValue = "";
                    Console.WriteLine($"Total count {res.Count()}");
                    foreach (var r in res)
                    {
                        Console.Write($"{r},");
                    }
                    Console.WriteLine();
                    if (res.Count() == 0)
                    {
                        Console.WriteLine("No Results found...");
                    }
                    Console.WriteLine($"Start Time for Trie Search {stTm}");
                    Console.WriteLine($"End Time for Trie Search {enTm}");
                    Console.WriteLine("Type search text and press enter...(esc to quit)");
                }
                else if(info.Key==ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    readValue += info.KeyChar.ToString();
                }
            }
            
        }
    }

  
    public class Trie
    {
        private Node _root = new Node();

        public void AddData(IList<string> data)
        {
            foreach (var dt in data) _root.Add(dt.ToLower());
        }

        public IList<String> FindAllMatch(string query)
        {
            var retList = new List<String>();
            _root.Find(query.ToLower(), retList);
            return retList;
        }
        private class Node
        {
            public Dictionary<char, Node> Children { get; set; } = new Dictionary<char, Node>();
            public bool IsCompleted { get; set; }

            private Node GetChildNode(char c)
            {
                if (!Children.ContainsKey(c))
                    Children.Add(c, new Node());

                return Children[c];
            }

            private Node TryGetChildNode(char c)
            {
                if (Children.ContainsKey(c))
                    return Children[c];
                else
                    return null;
            }

            public void Add(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                    return;

                var child = GetChildNode(s[0]);
                if (s.Length >  1)  child.Add(s.Substring(1));
                else child.IsCompleted = true;
            }           
            public void Find(string query, IList<String> result)
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length == 0)
                    return;
                if (result == null)
                    result = new List<string>();
                FindDepthAndCollect(query, 0, result);
            }

            private void FindDepthAndCollect(string query, int index, IList<string> result)
            {
                var child = TryGetChildNode(query[index]);
                if (child != null)
                {
                    if (query.Length > index + 1)
                        child.FindDepthAndCollect(query, index + 1, result);
                    else
                    {
                        string prefixString = query.Substring(0, index + 1);
                        RecursiveCollect(child, prefixString, result);
                    }
                }
            }
            private static void RecursiveCollect(Node node, string prefixString, IList<string> result)
            {
                if (node.IsCompleted)
                    result.Add(prefixString);
                foreach (var kvp in node.Children) RecursiveCollect(kvp.Value, prefixString + kvp.Key, result);
            }

        }
    }

    

}
