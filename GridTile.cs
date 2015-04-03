using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConnectX
{
    public class GridTile : CXGameObject
    {
        //Instance variables
        float tileXStartPos, tileYStartPos;
        bool isEmpty;
        BoardPiece boardPiece;
        int logicalGridX, logicalGridY;
        
        #region Properties
        public int LogicalX
        {
            get { return logicalGridX; }
            set { logicalGridX = value; }
        }
        public int LogicalY
        {
            get { return logicalGridY; }
            set { logicalGridY = value; }
        }
        
       
        public BoardPiece Piece
        {
            get
            {
                return boardPiece;
            }
            set
            {
                boardPiece = value;
            }
        }
        public bool Empty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
            }
        }
        public float xLocation
        {
            get
            {
                return tileXStartPos;
            }
            set
            {
                tileXStartPos = value;
            }
        }
        public float yLocation
        {
            get
            {
                return tileYStartPos;
            }
            set
            {
                tileYStartPos = value;
            }
        }
        #endregion

        public GridTile()
            : base()
        {

        }
        public GridTile(float xPos, float yPos, Texture2D texture)
            :base(texture, new Vector2(xPos, yPos))
        {
            tileXStartPos = xPos;
            tileYStartPos = yPos;
            isEmpty = true;
        }
        public void SetPiece(BoardPiece bPiece)
        {
            boardPiece = bPiece;
        }
        
    }
}
