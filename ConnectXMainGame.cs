//Nathanael Thompson
//Artificial Intelligence, Prof. Franklin
//Project 2 - Connect 4 Agent
//SPSU Spring 2015

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectX
{
    public enum GameMode
    {
        PvP,
        PvAI,
        Complete
    }
    public class ConnectXMainGame : Game
    {
        #region Instance Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D tile, restartButton, exitButton, redPiece, blackPiece,
            arrowUp, arrowDown, pvpButton, pvaiButton, clearPiece;
        private SpriteFont font, winnerFont;
        private GUIHandler guiHandler;
        private GridTile[,] allTiles;

        //note that these variables must be lowered if the user wishes to have values lower than the default
        private static int boardWidth=7, boardHeight=6;
        private static int numToConnect = 4;
        List<Texture2D> guiTextures;
        Board board;
        GameMode currentMode;
        private static int windowWidth, windowHeight;
        #endregion

        #region Properties
        public static int NumToConnect
        {
            get { return numToConnect; }
            set 
            { 
                numToConnect = value;
                if (numToConnect < 4)
                    numToConnect = 4;
                else if (numToConnect > BoardHeight || numToConnect > BoardWidth)
                    numToConnect = 4;
            }
        }

        public static int WindowWidth
        {
            get { return windowWidth; }
            set 
            { 
                windowWidth = value;
            }
        }
        public static int WindowHeight
        {
            get { return windowHeight; }
            set { windowHeight = value; }
        }
        public static int BoardWidth
        {
            get
            {return boardWidth;}
            set
            {
                boardWidth = value;
                if (boardWidth < 7)
                    boardWidth = 7;
            }
        }
        public static int BoardHeight
        {
            get
            {
                return boardHeight;
            }
            set
            {
                boardHeight = value;
                if (boardHeight < 6)
                    boardHeight = 6;
            }
        }
        public static int TurnNumber
        {
            get;
            set;
        }
        #endregion
        public ConnectXMainGame()
            : base()
        {
            //set window size
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
            windowHeight = graphics.PreferredBackBufferHeight;
            windowWidth = graphics.PreferredBackBufferWidth;
            Content.RootDirectory = "Content";

            //and create a new GUIHandler
            guiHandler = new GUIHandler(this);
        }

        //I think this is self-explanatory
        public void Restart()
        {
            BoardWidth = 7;
            BoardHeight = 6;
            NumToConnect = 4;
            TurnNumber = 0;
            board = new Board(tile, clearPiece);
            currentMode = GameMode.PvP;
            
        }
        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
            allTiles = new GridTile[BoardWidth, BoardHeight];
            currentMode = GameMode.PvP;
            
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Fonts
            font = Content.Load<SpriteFont>("GeneralUseFont");
            winnerFont = Content.Load<SpriteFont>("WinnerFont");

            //GUI Textures
            restartButton = Content.Load<Texture2D>("restartButton");
            exitButton = Content.Load<Texture2D>("exitButton");
            arrowUp = Content.Load<Texture2D>("arrowUp");
            arrowDown = Content.Load<Texture2D>("arrowDown");
            pvpButton = Content.Load<Texture2D>("pvpButton");
            pvaiButton = Content.Load<Texture2D>("pvaiButton");

            guiTextures = new List<Texture2D>();
            guiTextures.Add(restartButton);
            guiTextures.Add(exitButton);
            guiTextures.Add(arrowUp);
            guiTextures.Add(arrowDown);
            guiTextures.Add(pvpButton);
            guiTextures.Add(pvaiButton);

            //Game object Textures
            tile = Content.Load<Texture2D>("tile");
            redPiece = Content.Load<Texture2D>("redPiece");
            blackPiece = Content.Load<Texture2D>("blackPiece");
            clearPiece = Content.Load<Texture2D>("clearPiece");
            board = new Board(tile, clearPiece);
        }
        protected override void UnloadContent()
        {
            
        }
        public void GuiCheck(int guiIndex, bool guiHitFlag)
        {
            if (guiHitFlag)
            {
                Board.WinnerExists = false;
                switch (guiIndex)
                {
                    case 0://Restart
                        Restart();
                        break;
                    case 1://Exit
                        this.Exit();
                        break;
                    case 2://Width up
                        TurnNumber = 0;
                        BoardWidth++;
                        board = new Board(tile, clearPiece);
                        break;
                    case 3://Width down
                        TurnNumber = 0;
                        BoardWidth--;
                        board = new Board(tile, clearPiece);
                        break;
                    case 4://Height up
                        TurnNumber = 0;
                        BoardHeight++;
                        board = new Board(tile, clearPiece);
                        break;
                    case 5://Height down
                        TurnNumber = 0;
                        BoardHeight--;
                        board = new Board(tile, clearPiece);
                        break;
                    case 6://PvP
                        currentMode = GameMode.PvP;
                        TurnNumber = 0;
                        board = new Board(tile, clearPiece);
                        break;
                    case 7://PvAI
                        currentMode = GameMode.PvAI;
                        TurnNumber = 0;
                        board = new Board(tile, clearPiece);
                        break;
                    case 8://Connect number up
                        TurnNumber = 0;
                        NumToConnect++;
                        board = new Board(tile, clearPiece);
                        break;
                    case 9://Connect number down
                        TurnNumber = 0;
                        NumToConnect--;
                        board = new Board(tile, clearPiece);
                        break;
                    default:
                        break;
                }
            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //If it is the first turn, we want to initialize everything related to the GUI
            if (TurnNumber == 0)
            {
                guiHandler.GUIInitializer(guiTextures);
            }

            //If there is one, get the GUI input, then apply
            Tuple<int, bool> guiResponse;
            guiResponse = guiHandler.Update(gameTime);
            GuiCheck(guiResponse.Item1, guiResponse.Item2);

            //Update the board, if winner is found, user cannot give the board more input
            board.Update(redPiece, blackPiece, currentMode);
            if (Board.WinnerExists)
            {
                currentMode = GameMode.Complete;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            board.Draw(spriteBatch, tile);
            guiHandler.Draw(spriteBatch);
            spriteBatch.DrawString(winnerFont, "Width", new Vector2(WindowWidth - 155, WindowHeight - 266), Color.Black);
            spriteBatch.DrawString(winnerFont, "Height", new Vector2(WindowWidth - 155, WindowHeight - 445), Color.Black);
            spriteBatch.DrawString(font, "Number to Connect: " + NumToConnect.ToString(), new Vector2(WindowWidth - 165, WindowHeight - 572), Color.Black);
            if(TurnNumber % 2 == 0)
            {
                spriteBatch.DrawString(font, "It is player 1's turn.", new Vector2(WindowWidth - 200, WindowHeight - 700), Color.Red);
            }
            else
            {
                spriteBatch.DrawString(font, "It is player 2's turn.", new Vector2(WindowWidth - 200, WindowHeight - 700), Color.Black);
            }

            if (Board.WinnerExists)
            {
                spriteBatch.DrawString(winnerFont, "Winner!", new Vector2(200, 5), Color.Blue);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}
