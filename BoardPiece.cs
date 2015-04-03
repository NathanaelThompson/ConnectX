using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConnectX
{
    public enum PieceColor
    {
        Red,
        Black,
        None
    }
    public class BoardPiece : CXGameObject
    {
        float xPos, yPos;
        PieceColor pColor;
        bool isVisible;
        
        #region Properties
        public bool Visible
        {
            get { return isVisible; }
            set { isVisible = value; }
            
        }
        public float XPosition
        {
            get
            {
                return xPos;
            }
            set
            {
                xPos = value;
            }
        }
        public float YPosition
        {
            get
            {
                return yPos;
            }
            set
            {
                yPos = value;
            }
        }
        public PieceColor Color
        {
            get
            {
                return pColor;
            }
            set
            {
                pColor = value;
            }
        }
        #endregion

        public BoardPiece()
        {

        }
        public BoardPiece(float xPosition, float yPosition, PieceColor pieceColor, Texture2D texture)
            :base(texture, new Vector2(xPosition, yPosition))
        {
            pColor = pieceColor;
            yPos = yPosition;
            xPos = xPosition;
            Visible = false;
        }
    }
}
