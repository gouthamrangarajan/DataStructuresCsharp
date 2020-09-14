using System;
using System.Collections.Generic;

namespace search_trie
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Trie trie=new Trie();
            trie.AddString("Ball");
            trie.AddString("bat");
            trie.AddString("Calf");
            trie.AddString("Caller");
            trie.AddString("Call");
            var res=trie.SearchString("call");
            if(res!=null){
               foreach(var r in res){
                   Console.WriteLine(r);
               }
            }
        }
    }
    public class Trie{
        Node _root=new Node('*');

        public void AddString(string s){
            if(string.IsNullOrWhiteSpace(s))
                return;
            s=s.Trim();
            _root.AddString(s,0);
        }
        public IEnumerable<string> SearchString(string srchTxt){
            return _root.SearchText(srchTxt);
        }
        private class Node{
            private char _c;
            internal char Character{
                get{
                    return _c;
                }
            }
            private bool _iscompleted;
            internal bool IsCompleted{
                get {
                    return _iscompleted;
                }
            }
            private Dictionary<char,Node> childrenLookup=new Dictionary<char, Node>();
            
            internal IEnumerable<Node> Children=>childrenLookup.Values;
                
            internal Node(char c){
                _c=c;
            }
            
            private Node GetNode(char c){
                char lowerC=c.ToString().ToLower()[0];
                if(childrenLookup.ContainsKey(lowerC))
                    return childrenLookup[lowerC];
                else
                    return null;
            }

            private Node AddNode(char c){
                char lowerC=c.ToString().ToLower()[0];
                childrenLookup.Add(lowerC,new Node(c));
                return childrenLookup[lowerC];
            }
            internal void AddString(string s,int index){
                if(index>=s.Length){
                    _iscompleted=true;
                    return;
                }
                 var child=GetNode(s[index]);
                 if(child==null){
                     child=AddNode(s[index]);
                 }
                 child.AddString(s,index+1);
            }
            internal Node FindCommondNode(string s,int index){                
                if(index==s.Length){
                    return this;
                }
                var child=GetNode(s[index]);
                if(child!=null){
                    return child.FindCommondNode(s,index+1);
                }
                else{
                    return null;
                }
            }
            internal IEnumerable<string> SearchText(string s){
                 Node commonNode=FindCommondNode(s,0);
                 if(commonNode==null)
                    return null;
                 List<string> retList=new List<string>();             
                 CollectAllWords(commonNode,s,retList);
                 return retList;
            }

            private static void CollectAllWords(Node commonNode,string prefix, List<string> list)
            {
                if(commonNode.IsCompleted){
                     list.Add(prefix);
                 }

                foreach(var child in commonNode.Children){
                    CollectAllWords(child,prefix+child.Character.ToString(),list);
                }
            }
        }
    }
}
