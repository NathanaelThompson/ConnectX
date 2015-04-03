using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ConnectX
{
    public class CXAgent
    {
        int maxDepth = 3;
        int currentDepth = 0;
        ReturnColumn bestColumn = new ReturnColumn();

        //If I wanted to mess with the difficulty of the AI, I could pass
        //maxDepth as a parameter to the constructor
        public CXAgent()
        {

        }
        public int Update(Board currentBoard)
        {
            bestColumn = MiniMax(currentBoard);
            ConnectXMainGame.TurnNumber++;
            return bestColumn.columnIndex;
        }
        public ReturnColumn MiniMax(Board currentBoard)
        {
            bestColumn = MaxMove(currentBoard, Int32.MinValue, Int32.MaxValue, currentDepth);
            return bestColumn;
        }
        public struct ReturnColumn
        {
            public int score, columnIndex;
        }

        //Returns a score (and it's location) to the MaxMove or MinMove function
        public ReturnColumn Heuristic(Board currentBoard)
        {
            GridTile[,] tiles = currentBoard.GetTiles();
            ReturnColumn currentMove = new ReturnColumn();
            ReturnColumn maxMove = new ReturnColumn();
            currentMove.score = 0;
            Random rand = new Random();
            currentMove.columnIndex = rand.Next(0, ConnectXMainGame.BoardWidth);
            maxMove.score = 0;
            int consecutiveBlkPieces, consecutiveRedPieces;
            PieceColor playerColor = PieceColor.Black;

            //Vertical check
            for (int i = tiles.GetLength(0) - 1; i >= 0; i--)
            {
                consecutiveRedPieces = 0;
                consecutiveBlkPieces = 0;
                for (int j = tiles.GetLength(1) - 1; j >= 0; j--)
                {
                    //if the piece is black and the original square isn't empty
                    if (tiles[i, j].Piece.Color == playerColor && tiles[i, j].Empty == false)
                    {
                        consecutiveBlkPieces++;
                        //Connect x number of black pieces
                        for (int x = 1; x <= consecutiveBlkPieces; x++)
                        {
                            if (consecutiveBlkPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MaxValue;
                                currentMove.columnIndex = i;
                                return currentMove;
                            }
                            else if (consecutiveBlkPieces == ConnectXMainGame.NumToConnect - 1)
                            {
                                if (j -1 < 0)
                                {
                                    break;
                                }
                                else
                                    currentMove.columnIndex++;

                                if (tiles[i, j - 1].Empty == false)
                                {
                                    currentMove.score = 0;
                                    
                                    break;
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score += x*x;
                                currentMove.columnIndex = i;
                            }
                        }
                    }
                    else if (tiles[i, j].Piece.Color != playerColor && tiles[i, j].Empty == false)
                    {
                        consecutiveRedPieces++;
                        //Connect x number of red piees
                        for (int x = 1; x <= consecutiveRedPieces; x++)
                        {
                            if (consecutiveRedPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MinValue;
                                currentMove.columnIndex = i;
                                return currentMove;
                            }
                            else if(consecutiveRedPieces == ConnectXMainGame.NumToConnect - 1)
                            {
                                if (j -1 < 0)
                                    break;
                                
                                if (tiles[i, j - 1].Empty == false)
                                {
                                    currentMove.score = 0;
                                    break;

                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score += x * x;
                                currentMove.columnIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (currentMove.score > maxMove.score)
                        {
                            maxMove.score = currentMove.score;
                            maxMove.columnIndex = currentMove.columnIndex;
                        }
                        currentMove.score = 0;
                        consecutiveBlkPieces = 0;
                        consecutiveRedPieces = 0;
                    }
                }
            }

            //Check horizontal
            for (int iHorizontal = tiles.GetLength(1) - 1; iHorizontal >= 0; iHorizontal--)
            {
                consecutiveBlkPieces = 0;
                consecutiveRedPieces = 0;
                for (int jHorizontal = tiles.GetLength(0) - 1; jHorizontal >= 0; jHorizontal--)
                {
                    //if the piece is black and the original square isn't empty
                    if (tiles[jHorizontal, iHorizontal].Piece.Color == playerColor && tiles[jHorizontal, iHorizontal].Empty == false)
                    {
                        consecutiveBlkPieces++;
                        //Connect x number of black pieces
                        for (int x = 1; x <= consecutiveBlkPieces; x++)
                        {
                            if (consecutiveBlkPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MaxValue;
                                currentMove.columnIndex = jHorizontal;
                                return currentMove;
                            }
                            else if (consecutiveBlkPieces == ConnectXMainGame.NumToConnect - 1)
                            {
                                if (jHorizontal - 1 < 0)
                                    break;
                                if (tiles[jHorizontal-1, iHorizontal].Empty == false)
                                {
                                    currentMove.score = 0;
                                   
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = jHorizontal-1;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score = Int32.MaxValue;
                                currentMove.columnIndex = jHorizontal;
                            }
                        }
                    }
                    else if (tiles[jHorizontal, iHorizontal].Piece.Color != playerColor && tiles[jHorizontal, iHorizontal].Empty == false)
                    {
                        consecutiveRedPieces++;
                        //Connect x number of red piees
                        for (int x = 1; x <= consecutiveRedPieces; x++)
                        {
                            if (consecutiveRedPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MinValue;
                                currentMove.columnIndex = jHorizontal;
                                return currentMove;
                            }
                            else if (consecutiveRedPieces == ConnectXMainGame.NumToConnect - 1)
                            {
                                if (jHorizontal - 1 < 0)
                                    break;
                                if (tiles[jHorizontal - 1, iHorizontal].Empty == false)
                                {
                                    currentMove.score = 0;
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = jHorizontal-1;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score += x * x;
                                currentMove.columnIndex = jHorizontal;
                            }
                        }
                    }
                    else
                    {

                        if (currentMove.score > maxMove.score)
                        {
                            maxMove.score = currentMove.score;
                            maxMove.columnIndex = currentMove.columnIndex;
                        }
                        currentMove.score = 0;
                        consecutiveBlkPieces = 0;
                        consecutiveRedPieces = 0;
                    }
                }
            }
            //Check horizontal left to right
            for (int iHorizontal = tiles.GetLength(1) - 1; iHorizontal >= 0; iHorizontal--)
            {
                consecutiveBlkPieces = 0;
                consecutiveRedPieces = 0;
                for (int jHorizontal = 0; jHorizontal < tiles.GetLength(0)-1; jHorizontal++)
                {
                    //if the piece is black and the original square isn't empty
                    if (tiles[jHorizontal, iHorizontal].Piece.Color == playerColor && tiles[jHorizontal, iHorizontal].Empty == false)
                    {
                        consecutiveBlkPieces++;
                        //Connect x number of black pieces
                        for (int x = 1; x <= consecutiveBlkPieces; x++)
                        {
                            //win detected
                            if (consecutiveBlkPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MaxValue;
                                currentMove.columnIndex = jHorizontal;
                                return currentMove;
                            }
                            else if (consecutiveBlkPieces == ConnectXMainGame.NumToConnect - 1)
                            {//almost a win
                                //but the next piece needed to win is not empty and not the player color
                                if (tiles[jHorizontal - 1, iHorizontal].Empty == false)
                                {
                                    currentMove.score = 0;
                                    if (jHorizontal + 1 > ConnectXMainGame.BoardWidth - 1)
                                        break;
                                }
                                else
                                {//place in the winning spot
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = jHorizontal - 1;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score += x * x; 
                                currentMove.columnIndex = jHorizontal;
                            }
                        }
                    }
                    else if (tiles[jHorizontal, iHorizontal].Piece.Color != playerColor && tiles[jHorizontal, iHorizontal].Empty == false)
                    {
                        consecutiveRedPieces++;
                        //Connect x number of red piees
                        for (int x = 1; x <= consecutiveRedPieces; x++)
                        {
                            if (consecutiveRedPieces >= ConnectXMainGame.NumToConnect)
                            {
                                currentMove.score = Int32.MinValue;
                                currentMove.columnIndex = jHorizontal;
                                return currentMove;
                            }
                            else if (consecutiveRedPieces == ConnectXMainGame.NumToConnect - 1)
                            {
                                if (jHorizontal + 1 > ConnectXMainGame.BoardWidth - 1)
                                    break;
                                if (tiles[jHorizontal + 1, iHorizontal].Empty == false)
                                {
                                    currentMove.score = 0;   
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = jHorizontal + 1;
                                    return currentMove;
                                }
                            }
                            else
                            {
                                currentMove.score += x * x;
                                currentMove.columnIndex = jHorizontal;
                            }
                        }
                    }
                    else
                    {

                        if (currentMove.score > maxMove.score)
                        {
                            maxMove.score = currentMove.score;
                            maxMove.columnIndex = currentMove.columnIndex;
                        }
                        currentMove.score = 0;
                        consecutiveBlkPieces = 0;
                        consecutiveRedPieces = 0;
                    }
                }
            }

            //run through down left diagonal tiles
            for (int iDiagRight = 0; iDiagRight < tiles.GetLength(0) - 1; iDiagRight++)
            {
                for (int jDiagRight = 0; jDiagRight < tiles.GetLength(1) - 1; jDiagRight++)
                {
                    consecutiveBlkPieces = 0;
                    consecutiveRedPieces = 0;
                    for (int k = 1; k <= ConnectXMainGame.NumToConnect; k++)
                    {
                        //if the indicies will be outside the bounds of the board after the next operation
                        if (iDiagRight < k  || jDiagRight + k > tiles.GetLength(1) -2)
                        {
                            //score lost, break;
                            consecutiveBlkPieces = 0;
                            consecutiveRedPieces = 0;
                            currentMove.score = 0;
                            break;
                        }
                        if (tiles[iDiagRight - k, jDiagRight + k].Piece.Color == playerColor && tiles[iDiagRight, jDiagRight].Empty == false)
                        {//if the placed piece color is black and the starting GridTile isn't empty
                            consecutiveBlkPieces++;
                            //Connect x number of black pieces
                            for (int x = 1; x <= consecutiveBlkPieces; x++)
                            {
                                //win found
                                if (consecutiveBlkPieces >= ConnectXMainGame.NumToConnect)
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = iDiagRight;
                                    return currentMove;
                                }
                                else if (consecutiveBlkPieces == ConnectXMainGame.NumToConnect - 1)
                                {//if almost a win
                                    //but the pattern continues outside of the board
                                    if (iDiagRight - k - 1 < 0 || jDiagRight + k + 1 > ConnectXMainGame.BoardHeight - 1)
                                        break;
                                    if (tiles[iDiagRight-k-1, jDiagRight+k+1].Empty == false)
                                    {
                                        currentMove.score = 0;
                                    }
                                    else
                                    {
                                        currentMove.score = Int32.MaxValue;
                                        currentMove.columnIndex = jDiagRight+k;
                                        return currentMove;
                                    }
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = iDiagRight;
                                }
                            }
                        }
                        else if (tiles[iDiagRight - k, jDiagRight + k].Piece.Color != playerColor && tiles[iDiagRight, jDiagRight].Empty == false)
                        {
                            consecutiveRedPieces++;
                            //Connect x number of red piees
                            for (int x = 1; x <= consecutiveRedPieces; x++)
                            {
                                //lose detected
                                if (consecutiveRedPieces >= ConnectXMainGame.NumToConnect)
                                {
                                    currentMove.score = Int32.MinValue;
                                    currentMove.columnIndex = iDiagRight - k;
                                    return currentMove;
                                }
                                else if (consecutiveRedPieces == ConnectXMainGame.NumToConnect - 1)
                                {//almost a lose
                                    //but out of bounds
                                    if (iDiagRight - k - 1 < 0 || jDiagRight + k +1 > ConnectXMainGame.BoardHeight-1)
                                        break;
                                    if (tiles[iDiagRight - k - 1, jDiagRight + k + 1].Empty == false)
                                    {
                                        if (iDiagRight > ConnectXMainGame.BoardWidth - 2)
                                            currentMove.columnIndex = 0;
                                        else
                                            currentMove.columnIndex++;
                                        break;
                                    }
                                    else
                                    {//attempt to block
                                        currentMove.score = Int32.MaxValue;
                                        currentMove.columnIndex = jDiagRight + k;
                                        return currentMove;
                                    }
                                }
                                else
                                {
                                    currentMove.score -= x * x;
                                    currentMove.columnIndex = iDiagRight - k;
                                }
                            }
                        }
                        else
                        {
                            //set the max score
                            if (currentMove.score > maxMove.score)
                            {
                                maxMove.score = currentMove.score;
                                maxMove.columnIndex = currentMove.columnIndex;
                            }
                            currentMove.score = 0;
                            consecutiveBlkPieces = 0;
                            consecutiveRedPieces = 0;
                        }
                    }
                }
            }

            //check up left diagonal
            for (int i = tiles.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = tiles.GetLength(1) - 1; j >= 0; j--)
                {
                    consecutiveBlkPieces = 0;
                    consecutiveRedPieces = 0;
                    for (int k = 0; k < ConnectXMainGame.NumToConnect+1; k++)
                    {
                        if(i < k || j < k)
                        {
                            consecutiveBlkPieces = 0;
                            consecutiveRedPieces = 0;
                            currentMove.score = 0;
                            break;
                        }
                        if (tiles[i - k, j - k].Piece.Color == playerColor && tiles[i, j].Empty == false)
                        {
                            consecutiveBlkPieces++;
                            for (int x = 1; x <= consecutiveBlkPieces; x++)
                            {
                                if (consecutiveBlkPieces >= ConnectXMainGame.NumToConnect)
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i-k;
                                    return currentMove;
                                }
                                else if (consecutiveBlkPieces == ConnectXMainGame.NumToConnect - 1)
                                {
                                    if (i - k - 1 < 0 || j - k - 1 < 0)
                                        break;
                                    if (tiles[i - k - 1, j - k - 1].Empty == false)
                                    {
                                        currentMove.score = 0;
                                    }
                                    else
                                    {
                                        currentMove.score = Int32.MaxValue;
                                        currentMove.columnIndex = j - k;
                                        return currentMove;
                                    }
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i-k;
                                }
                            }
                        }
                        else if(tiles[i-k, j-k].Piece.Color != playerColor && tiles[i,j].Empty == false)
                        {
                            consecutiveRedPieces++;
                            for (int x = 1; x <= consecutiveRedPieces; x++)
                            {
                                if (consecutiveRedPieces >= ConnectXMainGame.NumToConnect)
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i-k;
                                    return currentMove;
                                }
                                else if (consecutiveRedPieces == ConnectXMainGame.NumToConnect - 1)
                                {
                                    if (i - k - 1 < 0 || j - k - 1 < 0)
                                        break;
                                    if (tiles[i - k - 1, j - k - 1].Empty == false)
                                    {
                                        currentMove.score = 0;
                                    }
                                    else
                                    {
                                        currentMove.score = Int32.MaxValue;
                                        currentMove.columnIndex = j - k;
                                        return currentMove;
                                    }
                                }
                                else
                                {
                                    currentMove.score = Int32.MaxValue;
                                    currentMove.columnIndex = i-k;
                                }
                            }
                        }
                        else
                        {   
                            if (currentMove.score > maxMove.score)
                            {
                                maxMove.score = currentMove.score;
                                maxMove.columnIndex = currentMove.columnIndex;
                            }
                            currentMove.score = 0;
                            consecutiveBlkPieces = 0;
                            consecutiveRedPieces = 0;
                        }
                    }
                }
            }
            return maxMove;
        }
        public ReturnColumn MaxMove(Board currentBoard, int alpha, int beta, int depth)
        {
            depth++;
            ReturnColumn bestMove = new ReturnColumn();
            ReturnColumn currentMove = new ReturnColumn();
            int[] moves;
            PieceColor playerColor;
            int turnNumber = ConnectXMainGame.TurnNumber;
            if ((turnNumber + depth) % 2 == 1)
            {
                playerColor = PieceColor.Black;
            }
            else
            {
                playerColor = PieceColor.Red;
            }
            if (depth > maxDepth)
            {
                return Heuristic(currentBoard);
            }
            else
            {
                bestMove.score = 0;
                moves = GenerateMoves(currentBoard);
                for (int i = 0; i < moves.Length - 1; i++)
                {
                    ApplyMove(currentBoard, moves[i], playerColor);
                    currentMove = MinMove(currentBoard, alpha, beta, depth);
                    if (currentMove.score > bestMove.score)
                    {
                        bestMove.score = currentMove.score;
                        bestMove.columnIndex = currentMove.columnIndex;
                        alpha = currentMove.score;
                    }
                    if (beta > alpha)
                        return bestMove;
                }
                return bestMove;
            }

        }
        public ReturnColumn MinMove(Board currentBoard, int alpha, int beta, int depth)
        {
            depth++;
            ReturnColumn bestMove = new ReturnColumn();
            ReturnColumn currentMove;
            int[] moves;
            PieceColor playerColor;
            int turnNumber = ConnectXMainGame.TurnNumber;
            if ((turnNumber + depth) % 2 == 1)
            {
                playerColor = PieceColor.Black;
            }
            else
            {
                playerColor = PieceColor.Red;
            }
            if (depth > maxDepth)
            {
                return Heuristic(currentBoard);
            }
            else
            {
                bestMove.score = 0;
                moves = GenerateMoves(currentBoard);
                for (int i = 0; i < moves.Length - 1; i++)
                {
                    ApplyMove(currentBoard, moves[i], playerColor);
                    ReturnColumn tempColumn = new ReturnColumn();
                    tempColumn = MaxMove(currentBoard, alpha, beta, depth);
                    currentMove.score = tempColumn.score;
                    currentMove.columnIndex = tempColumn.columnIndex;
                    if (currentMove.score > bestMove.score)
                    {
                       
                        bestMove.score = currentMove.score;
                        bestMove.columnIndex = currentMove.columnIndex;
                        beta = bestMove.score;
                    }

                    if (beta < alpha)
                        return tempColumn;
                }
                return bestMove;
            }
        }

        //Generates current legal moves
        public int[] GenerateMoves(Board currentBoard)
        {
            int[] moves = new int[ConnectXMainGame.BoardWidth];
            GridTile[,] tiles = currentBoard.GetTiles();
            for (int i = 0; i < ConnectXMainGame.BoardWidth; i++)
            {
                for (int j = ConnectXMainGame.BoardHeight-1; j >= 0; j--)
                {
                    if(tiles[i,j].Empty)
                    {
                        moves[i] = j;
                        break;
                    }
                }
            }
            return moves;
        }
        public void ApplyMove(Board currentBoard, int column, PieceColor playerColor)
        {
            GridTile[,] tiles = currentBoard.GetTiles();
            GridTile gTile = new GridTile();
            gTile.LogicalX = column;
            gTile.Piece = new BoardPiece();
            gTile.Piece.Color = playerColor;
            for (int i = tiles.GetLength(1)-1; i >= 0; i--)
            {
                if (tiles[column, i].Empty)
                {
                    gTile.LogicalY = i;
                    gTile.Empty = false;
                    tiles[column, i] = gTile;
                    break;
                }
            }
            
        }
    }
}
