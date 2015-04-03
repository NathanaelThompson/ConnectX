using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ConnectX
{
    //This is the base class for other game objects, mostly used for board pieces
    public class CXGameObject
    {
        Texture2D objTexture;
        Vector2 objPosition;
        Rectangle objRectangle;

        public Texture2D Texture
        {
            get
            {
                return objTexture;
            }
            set
            {
                objTexture = value;
            }
        }
        public CXGameObject()
        {

        }
        public CXGameObject(Texture2D texture, Vector2 position)
        {
            objTexture = texture;
            objPosition = position;
            objRectangle = new Rectangle((int)position.X, (int)position.Y,
                texture.Height, texture.Width);
        }
        public CXGameObject(Texture2D texture, Rectangle rectangle)
        {
            objTexture = texture;
            objRectangle = rectangle;
        }
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
