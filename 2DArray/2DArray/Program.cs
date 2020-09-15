using System;

namespace _2DArray
{
    /*
     * check if word is present in 2d character board
     * allowed movements sequence (with repetition / midway)
     * right,bottom,left,bottom
     * right,bottom,right,bottom
     * bottom,right,bottom,left
     * bottom,right,bottom,right
     * bottom,left,bottom,left
     * bottom,left,bottom,right
     * not allowed diagonal,top,right to left 
     */
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Char[,]
            {
                { 'T','E','S','T' },
                {'C','H','E','K' },
                 {'D','C','A','A' },
                 {'D','X','M','A' },
            };

            var present = CheckWordExists(board, "EKAAM");
            Console.WriteLine(present.ToString());
            Console.ReadKey();
        }

        private enum Direction{
            Right,
            Bottom,
            Left
        }
        private static bool CheckWordExists(char[,] board, string word)
        {
            var present = false;
            if(!string.IsNullOrWhiteSpace(word)){
                for(var rowNum=0;rowNum<=board.GetUpperBound(0);rowNum++){
                    for(var colNum=0;colNum<=board.GetUpperBound(1);colNum++){
                        if(word[0]==board[rowNum,colNum]){
                            present=checkRecursiveWordExist(word,1,board,rowNum,colNum+1,Direction.Right);
                            if(present)
                                break;
                        }
                    }
                    if(present)
                        break;
                }
            }
            return present;
        }

        private static bool checkRecursiveWordExist(string word, int wordIndex, char[,] board, int rowNum, int colNum, Direction currentDirection)
        {
            var present=false;
            if(wordIndex>=word.Length){
                return true;
            }
            if(rowNum>board.GetUpperBound(0) || colNum>board.GetUpperBound(1) || rowNum<0 || colNum <0){
                return false;
            }
            if(board[rowNum,colNum].ToString().ToLower()==word[wordIndex].ToString().ToLower()){
                present=true;
            }
            if(present){
                switch(currentDirection){
                    case Direction.Right:{
                        present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum,colNum+1,Direction.Right);
                        if(!present)
                            present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum+1,colNum,Direction.Bottom);
                        break;
                    }
                    case Direction.Bottom:{
                        present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum,colNum+1,Direction.Right);
                        if(!present)
                            present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum+1,colNum,Direction.Bottom);
                        if(!present)
                            present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum,colNum-1,Direction.Left);
                        break;
                    }
                    case Direction.Left:{
                        present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum,colNum-1,Direction.Left);
                        if(!present)
                            present=checkRecursiveWordExist(word,wordIndex+1,board,rowNum+1,colNum,Direction.Bottom);                        
                        break;
                    }
                }
            }
            return present;
        }
    }
        
}
