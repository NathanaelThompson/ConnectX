using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ConnectX
{
    public class GUIHandler : Microsoft.Xna.Framework.GameComponent
    {
        #region Instance Variables
        static MouseState mouseState = new MouseState();
        static MouseState lastMouseState = new MouseState();
        public List<Rectangle> guiRects = new List<Rectangle>();
        public List<Texture2D> guiTextures = new List<Texture2D>();
        Vector2 restartButtonVect;
        Vector2 exitButtonVect;
        Vector2 widthUpVect;
        Vector2 widthDownVect;
        Vector2 heightUpVect;
        Vector2 heightDownVect;
        Vector2 pvpVect;
        Vector2 pvaiVect;
        Vector2 connectUpVect, connectDownVect;
        #endregion

        //Properties
        public static MouseState CurrentMouseState
        {
            get
            {
                return mouseState;
            }
        }
        public static MouseState LastMouseState
        {
            get
            {
                return lastMouseState;
            }
        }
        
        public GUIHandler(Game game)
            :base(game)
        {
        }

        //This ugly thing takes in a list of textures and translates them, with instance Vector2s, onto rectangles
        //The textures in question are related to the interface, thus the name GUIInitializer
        public void GUIInitializer(List<Texture2D> guiTextures)
        {
            this.guiTextures = guiTextures;
            Texture2D restartTex = guiTextures[0];
            Texture2D exitTex = guiTextures[1];
            Texture2D arrowUpTex = guiTextures[2];
            Texture2D arrowDownTex = guiTextures[3];
            Texture2D pvpTex = guiTextures[4];
            Texture2D pvaiTex = guiTextures[5];
            restartButtonVect = new Vector2(ConnectXMainGame.WindowWidth-225, ConnectXMainGame.WindowHeight-60);
            exitButtonVect = new Vector2(ConnectXMainGame.WindowWidth - 100, ConnectXMainGame.WindowHeight - 60);
            widthUpVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 300);
            widthDownVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 225);
            heightUpVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 475);
            heightDownVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 400);
            pvpVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 150);
            pvaiVect = new Vector2(ConnectXMainGame.WindowWidth - 150, ConnectXMainGame.WindowHeight - 150);
            connectUpVect = new Vector2(ConnectXMainGame.WindowWidth - 225, ConnectXMainGame.WindowHeight - 625);
            connectDownVect = new Vector2(ConnectXMainGame.WindowWidth - 225,ConnectXMainGame.WindowHeight -550);
            Rectangle restartRectangle = new Rectangle((int)restartButtonVect.X, (int)restartButtonVect.Y, restartTex.Width, restartTex.Height);
            Rectangle exitRectangle = new Rectangle((int)exitButtonVect.X, (int)exitButtonVect.Y, exitTex.Width, exitTex.Height);
            Rectangle widthUpRectangle = new Rectangle((int)widthUpVect.X, (int)widthUpVect.Y, arrowUpTex.Width, arrowUpTex.Height);
            Rectangle widthDownRectangle = new Rectangle((int)widthDownVect.X, (int)widthDownVect.Y, arrowDownTex.Width, arrowDownTex.Height);
            Rectangle heightUpRectangle = new Rectangle((int)heightUpVect.X, (int)heightUpVect.Y, arrowUpTex.Width, arrowUpTex.Height);
            Rectangle heightDownRectangle = new Rectangle((int)heightDownVect.X, (int)heightDownVect.Y, arrowDownTex.Width, arrowDownTex.Height);
            Rectangle pvpRect = new Rectangle((int)pvpVect.X, (int)pvpVect.Y, pvpTex.Width, pvpTex.Height);
            Rectangle pvaiRect = new Rectangle((int)pvaiVect.X, (int)pvaiVect.Y, pvaiTex.Width, pvaiTex.Height);
            Rectangle connectUpRect = new Rectangle((int)connectUpVect.X, (int)connectUpVect.Y, arrowUpTex.Width, arrowDownTex.Width);
            Rectangle connectDownRect = new Rectangle((int)connectDownVect.X, (int)connectDownVect.Y, arrowUpTex.Width, arrowDownTex.Width);
            guiRects.Add(restartRectangle);
            guiRects.Add(exitRectangle);
            guiRects.Add(widthUpRectangle);
            guiRects.Add(widthDownRectangle);
            guiRects.Add(heightUpRectangle);
            guiRects.Add(heightDownRectangle);
            guiRects.Add(pvpRect);
            guiRects.Add(pvaiRect);
            guiRects.Add(connectUpRect);
            guiRects.Add(connectDownRect);
        }

        
        public Tuple<int, bool> Update(GameTime gameTime)
        {
            //get mouse states
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            //If the mouse was clicked
            if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                //Validate click location is a legal choice
                int buttonIndex = ValidateGuiInput((float)lastMouseState.Position.X, (float)lastMouseState.Position.Y, guiRects);
                if (buttonIndex == -1)//if not valid
                {   
                    //return false, indicating the mouse did not click on any of the GUI 
                    return new Tuple<int,bool>(-1,false);
                }
                else
                {
                    //otherwise, return the number to be drawn
                    return new Tuple<int, bool>(buttonIndex, true);
                }
            }
            return new Tuple<int,bool>(-1, false);
        }
        
        //returns index of interface item clicked
        public int ValidateGuiInput(float mouseX, float mouseY, List<Rectangle> guiRects)
        {
            foreach (Rectangle rect in guiRects)
            {
                //if the mouse click was inside a given rectangle
                if (mouseX > rect.X && mouseY > rect.Y &&
                    mouseX <= (rect.X + rect.Width) && mouseY <= (rect.Y + rect.Height))
                {
                    //return the rectangle's index
                    return guiRects.IndexOf(rect);
                }
            }
            return -1;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //drawing the interface elements
            spriteBatch.Draw(guiTextures[0], restartButtonVect, Color.White);
            spriteBatch.Draw(guiTextures[1], exitButtonVect, Color.White);
            spriteBatch.Draw(guiTextures[2], widthUpVect, Color.White);
            spriteBatch.Draw(guiTextures[3], widthDownVect, Color.White);
            spriteBatch.Draw(guiTextures[2], heightUpVect, Color.White);
            spriteBatch.Draw(guiTextures[3], heightDownVect, Color.White);
            spriteBatch.Draw(guiTextures[4], pvpVect, Color.White);
            spriteBatch.Draw(guiTextures[5], pvaiVect, Color.White);
            spriteBatch.Draw(guiTextures[2], connectUpVect, Color.White);
            spriteBatch.Draw(guiTextures[3], connectDownVect, Color.White);
        }
    }
}
