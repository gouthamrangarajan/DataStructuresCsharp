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
                 {'D','C','B','A' },
                 {'D','X','M','A' },
            };

            var present = CheckWordExists(board, "EKABCX");
            Console.WriteLine(present.ToString());
            Console.ReadKey();
        }

        private static bool CheckWordExists(char[,] board, string word)
        {
            var present = false;
            for(var rowNum = 0; rowNum <= board.GetUpperBound(0); rowNum++)
            {
                for(var colNum = 0; colNum <= board.GetUpperBound(1); colNum++)
                {                    
                    present = recursiveCheckWordExists(board, rowNum, colNum,word,0,Move.Right);
                    if (present)
                        break;
                }
                if (present)
                    break;
            }

            return present;
        }

        private enum Move
        {            
            Right,
            Left,
            Bottom,            
        }
        private static bool recursiveCheckWordExists(char[,] board, int rowNum, int colNum, string word,int index,Move currentMovement)
        {
            if (rowNum > board.GetUpperBound(0) || colNum > board.GetUpperBound(1))
                return false;

            if (rowNum < 0 || colNum < 0)
                return false;

            if (index >= word.Length)
                return true;

            if (board[rowNum, colNum] == word[index])
            {
                var present = true;

                switch (currentMovement)
                {
                    case Move.Bottom:
                        {
                            present = recursiveCheckWordExists(board, rowNum, colNum + 1, word, index + 1, Move.Right);
                            if (!present)
                                present = recursiveCheckWordExists(board, rowNum + 1, colNum, word, index + 1, Move.Bottom);
                            if (!present)
                                present = recursiveCheckWordExists(board, rowNum, colNum - 1, word, index + 1, Move.Left);
                            break;
                        }
                    case Move.Right:
                        {
                            present = recursiveCheckWordExists(board, rowNum, colNum + 1, word, index + 1, Move.Right);
                            if (!present)
                                present = recursiveCheckWordExists(board, rowNum + 1, colNum, word, index + 1, Move.Bottom);
                            break;
                        }
                    case Move.Left:
                        {
                            present = recursiveCheckWordExists(board, rowNum, colNum - 1, word, index + 1, Move.Left);
                            if (!present)
                                present = recursiveCheckWordExists(board, rowNum + 1, colNum, word, index + 1, Move.Bottom);
                            break;
                        }
                }
                
                return present;
            }

            return false;
        }
    }
}
