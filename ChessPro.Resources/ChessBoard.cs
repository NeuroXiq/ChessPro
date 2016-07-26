using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    /* Class represents chess board where left down 
     * position of private Piece[] array is a1 (white start pos)
     * and left top is a8 (black start position).
     *
     */
    public class ChessBoard
    {
        public class Field
        {
            public bool IsEmpty { get; private set; }
            public Piece PieceOn { get; private set; }
            public static Field Empty { get { return new Field(true); } }

            private Field(bool isEmpty)
            {
                this.IsEmpty = true;
            }

            public Field(Piece piece)
            {
                this.PieceOn = piece;
            }
        }

        public Field[,] Board;

        public ChessBoard()
        {
            this.Board = new Field[8, 8];
        }

        public static Field[,] GetDefaultPosition()
        {
            /*
             *  small -black/capital - white
             *  
             *     ----------------
             *   0|r n b q k b n r| a8 b8 c8 d8 e8 etc.
             *   1|p p p p p p p p| 
             *   2|               |
             *   3|               |
             *   4|               |
             *   5|               |
             *   6|P P P P P P P P|
             *   7|R N B Q K B N R| a1,b1,c1,d1 etc.
             *    -----------------
             *     0 1 2 3 4 5 6 7  
             *     A B C D E F G H 
             *     
             *    
             */



            Field[,] board = new Field[8, 8];
            board[0, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Rook)); //<- a8
            board[1, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Knight)); //<- b8
            board[2, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Bishop)); //<- c8
            board[3, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Queen)); //etc.
            board[4, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.King));
            board[5, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Bishop));
            board[6, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Knight));
            board[7, 0] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Rook));

            board[0, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Rook)); // <- a1
            board[1, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Knight)); // <-b1
            board[2, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Bishop)); // <- c1
            board[3, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Queen)); //etc.
            board[4, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.King));
            board[5, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Bishop));
            board[6, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Knight));
            board[7, 7] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Rook));

            //black pawns
            for (int i = 0; i < 8; i++)
            {
                board[i, 1] = new Field(new Piece(Piece.Color.Black, Piece.PieceType.Pawn));
            }
            //white pawns 
            for (int i = 0; i < 8; i++)
            {
                board[i, 6] = new Field(new Piece(Piece.Color.White, Piece.PieceType.Pawn));
            }
            //fields from a3 to a6 are empty.
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[j, i] = Field.Empty;
                }
            }

            return board;
        }

        public static ChessBoard GetStartPositionGameBoard()
        {
            ChessBoard b = new ChessBoard();
            b.Board = GetDefaultPosition();
            return b;
        }

        public ChessBoard Clone()
        {
            ChessBoard b = new ChessBoard();
            
            for (int i = 1; i < 9; i++)
            {
                for (char j = 'a'; j <= 'h'; j++)
                {
                    var p = this[j, i];
                    if (p.IsEmpty)
                        b[j, i] = Field.Empty;
                    else b[j, i] = new Field(new Piece(p.PieceOn.PieceColor, p.PieceOn.Type));
                }
            }
            return b;
        }

        public Field this[char charIndex, int numberIndex]
        {
            get
            {
                charIndex = charIndex.ToString().ToUpper()[0];

                int cInx = (charIndex - 65);
                int nInx = 8 - numberIndex;

                return Board[cInx,nInx];
            }
            set
            {
                charIndex = charIndex.ToString().ToUpper()[0];

                int cInx = charIndex - 65;
                int nInd = 8 - numberIndex;

                Board[cInx, nInd] = value;
            }
        }

        public Field this[int charIndex, int numberIndex]
        {
            get
            {
                return this[(char)charIndex, numberIndex];
            }
            set
            {
                this[(char)charIndex, numberIndex] = value;
            }
        }
        
        public Field this[PieceLocation location]
        {
            get
            {
                return this[location.CharIndex, location.NumberIndex];
            }
            set
            {
                this[location.CharIndex, location.NumberIndex] = value;
            }
        }

        public bool PieceOn(char charIndex, int numberIndex)
        {
            if (this[charIndex, numberIndex].IsEmpty)
                return false;
            else return true;
        }
    }
}
