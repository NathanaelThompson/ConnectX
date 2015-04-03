using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ConnectX
{
    public class Board
    {
        GridTile[,] tiles;
        static MouseState mouseState = new MouseState();
        static MouseState lastMouseState = new MouseState();
        private static bool winner;
        CXAgent agent;
        
        public GridTile[,] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        public static bool WinnerExists
        {
            get { return winner; }
            set { winner = value; }
        }
        public Board(Texture2D tileTexture, Texture2D clearPiece)
        {
           //on new board creation, we must decide on logical and graphical x and y values
            //and other bits of information
            tiles = new GridTile[ConnectXMainGame.BoardWidth, ConnectXMainGame.BoardHeight];
            for (int i = 0; i < ConnectXMainGame.BoardWidth; i++)
            {
                for (int j = 0; j < ConnectXMainGame.BoardHeight; j++)
                {
                    Vector2 destVect = new Vector2((60 + i * 60), (60 + j * 60));
                    tiles[i, j] = new GridTile(destVect.X, destVect.Y, tileTexture);
                    tiles[i, j].LogicalX = i;
                    tiles[i, j].LogicalY = j;
                    BoardPiece bPiece = new BoardPiece(tiles[i, j].xLocation + 5,
                                tiles[i, j].yLocation + 5, PieceColor.None, clearPiece);
                    bPiece.Visible = false;
                    tiles[i, j].SetPiece(bPiece);
                }
            }

            //and creating a board agent isn't a half bad idea
            agent = new CXAgent();
        }
        
        //This one had me stumped for a while and created many annoying errors.
        //As it turns out, objects in C# are passed EXPLICITLY by reference. 
        //So the only way to pass an object value is to break down into the primitives,
        //and assign the necessary values one at a time.
        public GridTile[,] GetTiles(){
            GridTile[,] arrToReturn = new GridTile[tiles.GetLength(0), tiles.GetLength(1)];
            for (int i = 0; i < arrToReturn.GetLength(0); i++)
            {
                for (int j = 0; j < arrToReturn.GetLength(1); j++)
                {
                    arrToReturn[i, j] = new GridTile(tiles[i, j].xLocation, tiles[i, j].yLocation, tiles[i, j].Texture);
                    arrToReturn[i, j].Piece = new BoardPiece(tiles[i, j].xLocation, tiles[i, j].yLocation, tiles[i, j].Piece.Color, tiles[i, j].Piece.Texture);
                    if (tiles[i, j].Empty == false)
                        arrToReturn[i, j].Empty = false;

                    
                }
            }
                return arrToReturn;
        }
        public Board(GridTile[,] allTiles)
        {
            tiles = allTiles;
        }
        public void Update(Texture2D redTexture, Texture2D blackTexture, GameMode gameMode)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            //if winner has already been found, do nothing and return;
            if (gameMode == GameMode.Complete)
            {
                return;
            }

            //if the mouse was clicked and the game mode is PVP or the gameMode is PvAI and the player's turn...
            if (lastMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released &&
                (gameMode == GameMode.PvP || 
                (gameMode == GameMode.PvAI && ConnectXMainGame.TurnNumber % 2 == 0)))
            {
                //Attempt to place a piece at the mouse click location
                PlacePieceAttempt(lastMouseState.X, lastMouseState.Y, redTexture, blackTexture);
                if (ConnectXMainGame.TurnNumber >= 7)
                {
                    WinnerExists = CheckForWin(tiles, false);
                }
            }
            //If not the player's turn and gameMode = PvAI, update the agent
            else if(ConnectXMainGame.TurnNumber % 2 == 1 && gameMode == GameMode.PvAI)
            {
                int columnReturned = agent.Update(this);
                LogicalPlaceAttempt(columnReturned, blackTexture);
                WinnerExists = CheckForWin(tiles, true);
                ConnectXMainGame.TurnNumber--;
            }
        }

        //Moves through the grid vertically, horizontally and diagonally to check for a win
        public bool CheckForWin(GridTile[,] allTiles, bool isAI)
        {
            PieceColor playerColor;
            int consecutivePieces = 0;
            bool winFound = false;
            
            //Since the turn is advanced immediately following a legal, successful move,
            //We must compensate in this function to find the correct player
            int turnConfusion = ConnectXMainGame.TurnNumber;
            turnConfusion -= 1;
            if(!isAI || ConnectXMainGame.TurnNumber % 2 == 0)
            {
                playerColor = PieceColor.Red;
            }
            else
            {
                playerColor = PieceColor.Black;
            }

            //Check vertically
            for (int i = allTiles.GetLength(0) - 1; i >= 0; i--)
            {
                if (winFound)
                    break;
                consecutivePieces = 0;
                for (int j = allTiles.GetLength(1) - 1; j >= 0; j--)
                {
                    //Potential win
                    if (allTiles[i, j].Piece.Color == playerColor && allTiles[i,j].Empty == false)
                    {
                        consecutivePieces++;
                        if (consecutivePieces >= ConnectXMainGame.NumToConnect)
                        {
                            //win
                            winFound = true;
                            break;
                        }
                    }
                    else
                    {
                        consecutivePieces = 0;
                    }
                }
            }

            //Check horizontally
            for (int iHorizontal = allTiles.GetLength(1) - 1; iHorizontal >= 0; iHorizontal--)
            {
                if (winFound)
                    break;
                consecutivePieces = 0;
                for (int jHorizontal = allTiles.GetLength(0) - 1; jHorizontal >= 0; jHorizontal--)
                {
                    //Potential win
                    if (allTiles[jHorizontal, iHorizontal].Piece.Color == playerColor && allTiles[jHorizontal, iHorizontal].Empty == false)
                    {
                        consecutivePieces++;
                        if (consecutivePieces >= 4)
                        {
                            //win
                            winFound = true;
                            break;
                        }
                    }
                    else
                    {
                        consecutivePieces = 0;
                    }
                }
            }

            
            //check diagonally left
            for (int i = allTiles.GetLength(0) - 1; i >= 0; i--)
            {
                if (winFound)
                    break;
                for (int j = allTiles.GetLength(1) - 1; j >= 0; j--)
                {
                    consecutivePieces = 0;
                    if (winFound)
                        break;
                    //check left
                    for (int k = 0; k < 5; k++)
                    {
                        //Potential win
                        if ((i < k || j < k))
                        {
                            consecutivePieces = 0;
                            break;
                        }
                        if (allTiles[i - k, j - k].Piece.Color == playerColor && allTiles[i,j].Empty == false)
                        {
                            consecutivePieces++;
                            if (consecutivePieces >= 4)
                            {
                                winFound = true;
                                break;
                            }
                        }
                        else
                        {
                            consecutivePieces = 0;
                        }
                        if (winFound)
                            break;
                    }                    
                }
            }

            //check diagonally right
            for (int iRightIndex = 0; iRightIndex <= allTiles.GetLength(0)-1; iRightIndex++)
            {
                if (winFound)
                    break;
                for (int jRightIndex = 0; jRightIndex <= allTiles.GetLength(1)-1; jRightIndex++)
                {
                    consecutivePieces = 0;
                    if (winFound)
                        break;
                    for (int k = 0; k < 5; k++)
                    {
                        
                        if (iRightIndex < k || jRightIndex + k > allTiles.GetLength(1)-1)
                        {
                            consecutivePieces = 0;
                            break;
                        }
                        if (allTiles[iRightIndex - k, jRightIndex + k].Piece.Color == playerColor && allTiles[iRightIndex, jRightIndex].Empty == false)
                        {
                            consecutivePieces++;
                            if (consecutivePieces >= 4)
                            {
                                winFound = true;
                                break;
                            }
                        }
                        else
                        {
                            consecutivePieces = 0;
                        }
                        if (winFound)
                            break;
                    }
                }
            }
            tiles = allTiles;
            return winFound;
        }

        //Attempts to place a board piece at a mouse click location
        public void PlacePieceAttempt(float mouseX, float mouseY, Texture2D redTexture, Texture2D blackTexture)
        {
            //Works backwards through the board grid to find the piece the mouse clicked
            for (int i = tiles.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = tiles.GetLength(1) - 1; j >= 0; j--)
                {
                    GridTile currentTile = tiles[i,j];
                    float tileEnd = currentTile.xLocation + 60;
                    float tileBottom = currentTile.yLocation + 60;

                    //mouse click was inside tile at location i,j
                    //and the space was empty
                    if (mouseX > currentTile.xLocation && mouseX < tileEnd &&
                        mouseY > currentTile.yLocation && mouseY < tileBottom &&
                        currentTile.Empty)
                    {
                        //if not the bottom of the column, return
                        for (int k = tiles.GetLength(1) - 1; k > j; k--)
                        {
                            if (tiles[i, k].Empty)
                            {
                                return;
                            }
                        }

                        //else, valid move, assign piece to tile and return
                        if (ConnectXMainGame.TurnNumber % 2 == 0)
                        {//if red's turn
                            BoardPiece bPiece = new BoardPiece(currentTile.xLocation + 5,
                                currentTile.yLocation + 5, PieceColor.Red, redTexture);
                            bPiece.Visible = true;
                            tiles[i, j].SetPiece(bPiece);
                            tiles[i, j].Empty = false;
                            
                            ConnectXMainGame.TurnNumber++;
                            return;
                        }
                        else
                        {//if black's turn
                            BoardPiece bPiece = new BoardPiece(currentTile.xLocation + 5,
                                currentTile.yLocation + 5, PieceColor.Black, blackTexture);
                            bPiece.Visible = true;
                            tiles[i, j].SetPiece(bPiece);
                            tiles[i, j].Empty = false;
                            
                            ConnectXMainGame.TurnNumber++;
                            return;
                        }
                    }
                }
            }
        }

        //This function will attempt to place a piece returned from the agent
        public void LogicalPlaceAttempt(int column, Texture2D blackTexture)
        {
            for (int i = ConnectXMainGame.BoardHeight - 1; i >= 0; i--)
            {
                if(tiles[column,i].Empty)
                {
                    GridTile currentTile = tiles[column, i];
                    float tileEnd = currentTile.xLocation + 60;
                    float tileBottom = currentTile.yLocation + 60;

                    BoardPiece bPiece = new BoardPiece(currentTile.xLocation + 5,
                                currentTile.yLocation + 5, PieceColor.Black, blackTexture);
                    bPiece.Visible = true;
                    tiles[column, i].SetPiece(bPiece);
                    tiles[column, i].Empty = false;
                    
                    ConnectXMainGame.TurnNumber++;
                    return;
                }
                else if (i == 0)
                {
                    if (column >= ConnectXMainGame.BoardWidth - 1)
                        LogicalPlaceAttempt(0, blackTexture);
                    else
                        LogicalPlaceAttempt(++column, blackTexture);
                }
            }
        }

        public void Draw(SpriteBatch sb, Texture2D tileTexture)
        {
            GridDraw(sb, tileTexture);
        }

        //assigns graphical x and y locations for grid tiles and board pieces
        private void GridDraw(SpriteBatch spriteBatch, Texture2D tileTexture)
        {
            for (int i = 0; i < ConnectXMainGame.BoardWidth; i++)
            {
                for (int j = 0; j < ConnectXMainGame.BoardHeight; j++)
                {
                    Vector2 destVect = new Vector2((60 + i * 60), (60 + j * 60));
                    Vector2 pieceVect = new Vector2((65 + i * 60), (65 + j * 60));
                    
                    GridTile tile = new GridTile(destVect.X, destVect.Y, tileTexture);
                    BoardPiece boardPiece = tiles[i, j].Piece;
                    if (boardPiece != null)
                    {
                        spriteBatch.Draw(tileTexture, destVect, Color.White);
                        if (boardPiece.Visible)
                        {
                            spriteBatch.Draw(boardPiece.Texture, pieceVect, Color.White);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(tileTexture, destVect, Color.White);
                    }
                }
            }
        }
    }
}
