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
            tr.AddStrings(new List<string> { "Cat","Calf", "Call", "Cab","Calf","Calves" ,"BAt","bat","Ball","ball","bumper"});
            var ran = 1000000;
            var lclList = new List<string>();
            for(var i=1;i<=ran; i++)
            {
                lclList.Add("test"+i);
            }
            Console.WriteLine($"Start Time for Trie construction {DateTime.Now.ToString("HH:mm:ss")}");
            tr.AddStrings(lclList);
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

    public class Node
    {
        public char Value { get; set; }

        public Node Parent { get; set; }

        public int Depth { get; set; }

        public List<Node> Children { get; set; }

        public Node(char value,Node parent,int depth)
        {
            Children = new List<Node>();        
            Value = value;
            Parent = parent;
            Depth = depth;
        }

        public Node FindChildNode(char c)
        {
            return Children.Find(e => e.Value.ToString().ToLower() == c.ToString().ToLower());
        }
    }

    public class Trie
    {
        private readonly Node _root;
        public Trie()
        {
            _root = new Node('^',null,0);            
        }

        private Node FindCommon(string data)
        {
            var retNode = _root;
            foreach(var c in data)
            {
                var fndNode=retNode.FindChildNode(c);
                if (fndNode == null)
                    break;
                retNode = fndNode;
            }
            return retNode;

        }

        public void AddStrings(IEnumerable<string> data)
        {
            foreach(var dt in data)
            {
                var crNode = FindCommon(dt);
                for(var i = crNode.Depth; i < dt.Length; i++)
                {
                    var nd = new Node(dt[i], crNode, crNode.Depth + 1);
                    crNode.Children.Add(nd);
                    crNode = nd;
                }
                crNode.Children.Add(new Node('$', crNode, crNode.Depth + 1));
            }
        }

        public IEnumerable<string> FindAllMatch(string value)
        {
            IEnumerable<string> retData=null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                var crNode = FindCommon(value);
                if (crNode != null)
                {
                    if (crNode.Depth == value.Length)
                    {
                        var fstValue = new StringBuilder(value.TrimEnd(crNode.Value, crNode.Value.ToString().ToLower()[0]));
                        var prefinalList = MergeChildValues(crNode, fstValue);
                        prefinalList.Add(fstValue);
                        retData = prefinalList.Select(el => el.ToString()).Distinct().OrderBy(el => el);
                    }
                }
            }
            if (retData == null)
            {
                retData=new List<string>();
            }
            return retData;
        }

        private List<StringBuilder> MergeChildValues(Node curNode,StringBuilder prValue)
        {
            var retBuilders = new List<StringBuilder>();
            if (prValue == null)
            {
                prValue = new StringBuilder();
            }
            if (curNode != null)
            {
                if (curNode.Value != '$')
                {
                    prValue.Append(curNode.Value.ToString());
                    var prValueBeforeLoop = prValue.ToString();
                    for(var ind = 0; ind < curNode.Children.Count; ind++)
                    {
                        if (ind == 0)
                        {
                            //for the first child append value 
                            retBuilders.AddRange(MergeChildValues(curNode.Children[0], prValue));
                        }
                        else
                        {
                            //Create new sb for all child except first one
                            var newSb = new StringBuilder(prValueBeforeLoop);
                            retBuilders.Add(newSb);
                            retBuilders.AddRange(MergeChildValues(curNode.Children[ind], newSb));
                        }
                    }
                }

            }            
            return retBuilders;
        }
    }

}
