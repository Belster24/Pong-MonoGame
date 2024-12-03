using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PongTestMono
{
    public class Game1 : Game
    {
        Texture2D pixel;

        private Texture2D ballTexture;
        private Texture2D playerTexture;
        private Rectangle enemyTexture;

        private Vector2 ballPosition;
        private Vector2 playerPosition;
        private Vector2 player2Position;

        private Rectangle playerCollision;
        private Rectangle player2Collision;
        private Rectangle ballCollision;

        public bool BallHitWallUP = false, BallHitWallDOWN=true;

        float playerSpeed, ballSpeed;

        int right=1, top=1;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //Collision collision = new Collision();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 480,
                PreferredBackBufferWidth = 640
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                       _graphics.PreferredBackBufferHeight / 2);
            playerPosition = Vector2.Zero;
            player2Position =new Vector2(_graphics.PreferredBackBufferWidth-_graphics.PreferredBackBufferWidth/10, 128);

            

            playerSpeed = 400;
            ballSpeed = 200;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            playerTexture = Content.Load<Texture2D>("Tile");

            //enemyTexture = new Rectangle((int)player2Position.X, (int)player2Position.Y, 40, 100);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

        }

        protected override void Update(GameTime gameTime)
        {
         
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float updatePlayerSpeed = playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            int deltaBallSpeed = (int)(ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            playerCollision = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerTexture.Width, playerTexture.Height);
            player2Collision = new Rectangle((int)player2Position.X, (int)player2Position.Y, playerTexture.Width, playerTexture.Height);
            ballCollision = new Rectangle((int) ballPosition.X+3, (int) ballPosition.Y+3, ballTexture.Width-6, ballTexture.Height-6);

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
            {
                playerPosition.Y -= updatePlayerSpeed;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                playerPosition.Y += updatePlayerSpeed;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                player2Position.Y -= updatePlayerSpeed;
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                player2Position.Y += updatePlayerSpeed;
            }

            /*
            if (kstate.IsKeyDown(Keys.W))
            {
                ballPosition.Y -= updatePlayerSpeed;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                ballPosition.Y += updatePlayerSpeed;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                ballPosition.X += updatePlayerSpeed;
            }
            if (kstate.IsKeyDown(Keys.A))
            {
                ballPosition.X -= updatePlayerSpeed;
            }
            */

            /*ballPosition.X += right * deltaBallSpeed;
            ballPosition.Y -= top * deltaBallSpeed;
            */
            BaseCollision();

            BallMovement(deltaBallSpeed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            _spriteBatch.Draw(playerTexture, player2Position, Color.White);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            _spriteBatch.Draw(pixel, playerCollision, Color.Red*0.5f);
            _spriteBatch.Draw(pixel, player2Collision, Color.Red * 0.5f);
            _spriteBatch.Draw(pixel, ballCollision, Color.Red * 0.5f);
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        public void BaseCollision()
        {
            if (playerCollision.Y > _graphics.PreferredBackBufferHeight - playerTexture.Height)
            {
                playerCollision.Y = _graphics.PreferredBackBufferHeight - playerTexture.Height;
                playerPosition.Y = playerCollision.Y;
            }
            else if (playerCollision.Y < playerTexture.Height / 8)
            {
                playerCollision.Y = playerTexture.Height / 8;
                playerPosition.Y = playerCollision.Y;
            }
            if (player2Collision.Y > _graphics.PreferredBackBufferHeight - playerTexture.Height)
            {
                player2Collision.Y = _graphics.PreferredBackBufferHeight - playerTexture.Height;
                player2Position.Y = player2Collision.Y;
            }
            else if (player2Collision.Y < playerTexture.Height / 8)
            {
                player2Collision.Y = playerTexture.Height / 8;
                player2Position.Y = player2Collision.Y;
            }



            if (playerCollision.Intersects(ballCollision))
            {
                
                Debug.WriteLine("Hit");
            }
        }

        public void BallMovement(int deltaBallSpeed)
        {
            //ballCollision
            ballPosition.X += right * deltaBallSpeed;
            ballPosition.Y -= top * deltaBallSpeed;
            //Vector2.Dot()

            if (playerCollision.Right > ballCollision.Left && ballCollision.Top >playerCollision.Top && ballCollision.Bottom < playerCollision.Bottom)
            {
                right = 1;
            }

            if (player2Collision.Left < ballCollision.Right && ballCollision.Top > player2Collision.Top && ballCollision.Bottom < player2Collision.Bottom)
            {
                right = -1;
            }

            if(ballPosition.Y < 0)
            {
                top *= -1;
            }
            if(ballPosition.Y > _graphics.PreferredBackBufferHeight - ballCollision.Height)
            {
                top *= -1;
            } 

            if(ballPosition.X < 0)
            {
                resetGame();
            }
            if(ballPosition.X > _graphics.PreferredBackBufferWidth - ballCollision.Width)
            {
                resetGame();
            }

        }

        public void resetGame()
        {
            ballPosition.X = _graphics.PreferredBackBufferWidth/2;
            ballPosition.Y = _graphics.PreferredBackBufferHeight/2;
        }

    }
}
