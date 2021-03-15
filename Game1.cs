using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        Rectangle playerRect;
        Rectangle computerRect;
        float playerSpeed;
        float computerSpeed;
        Texture2D bumper;
        Texture2D ball;
        Vector2 ballPos;
        Vector2 ballSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Random rng = new Random();
        SpriteFont titleFont;
        Screen currentScreen;
        enum Screen
        {
            Title,
            Game,
            GameOver
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerRect = new Rectangle(10, 0, 10, 100);
            computerRect = new Rectangle(780, 0, 10, 100);
            ballSpeed = new Vector2(-200, 0);
            playerSpeed = 300;
            computerSpeed = 300;
            bumper = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            bumper.SetData(data);
            ball = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            ball.SetData(data);
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            ballPos = new Vector2((_graphics.PreferredBackBufferWidth / 2) - ball.Width / 2,
            (_graphics.PreferredBackBufferHeight / 2) - ball.Height / 2);
            currentScreen = Screen.Title;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("ball");
            titleFont = Content.Load<SpriteFont>("TitleText");
        }

        protected override void Update(GameTime gameTime)
        {            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kState = Keyboard.GetState();
            if (currentScreen == Screen.Title)
            {
                if (kState.IsKeyDown(Keys.Enter))
                    currentScreen = Screen.Game;
            }
            else if (currentScreen == Screen.Game)
            {
                // Player Movement and bounds
                if (kState.IsKeyDown(Keys.Up))
                    playerRect.Y -= (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (kState.IsKeyDown(Keys.Down))
                    playerRect.Y += (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (playerRect.Y < 0)
                    playerRect.Y = 0;
                else if (playerRect.Y > _graphics.PreferredBackBufferHeight - playerRect.Height)
                    playerRect.Y = _graphics.PreferredBackBufferHeight - playerRect.Height;

                // Ball movement and collision
                ballPos.X += (float)(ballSpeed.X * gameTime.ElapsedGameTime.TotalSeconds);
                ballPos.Y += (float)(ballSpeed.Y * gameTime.ElapsedGameTime.TotalSeconds);
                if (ballPos.Y < 0)
                {
                    ballPos.Y = 0;
                    ballSpeed.Y *= -1;
                }
                else if (ballPos.Y >= _graphics.PreferredBackBufferHeight - ball.Height)
                {
                    ballPos.Y = _graphics.PreferredBackBufferHeight - ball.Height;
                    ballSpeed.Y *= -1;
                }

                //Collision w/ player
                if (ballPos.X < 20 & (ballPos.Y > playerRect.Y - ball.Height & ballPos.Y < playerRect.Y + playerRect.Height))
                {
                    ballSpeed.X *= -1;
                    if (kState.IsKeyDown(Keys.Up) & !kState.IsKeyDown(Keys.Down))
                        ballSpeed.Y += (float)rng.NextDouble() * 100;
                    else if (kState.IsKeyDown(Keys.Down) & !kState.IsKeyDown(Keys.Up))
                        ballSpeed.Y -= (float)rng.NextDouble() * 100;
                }

                //Collision w/ cpu
                if (ballPos.X > 780 - ball.Width & (ballPos.Y > computerRect.Y - ball.Height & ballPos.Y < computerRect.Y + computerRect.Height))
                {
                    ballSpeed.X *= -1;
                    if (kState.IsKeyDown(Keys.Up) & !kState.IsKeyDown(Keys.Down))
                        ballSpeed.Y += (float)rng.NextDouble() * 100;
                    else if (kState.IsKeyDown(Keys.Down) & !kState.IsKeyDown(Keys.Up))
                        ballSpeed.Y -= (float)rng.NextDouble() * 100;
                }

                // Win detection
                if (ballPos.X <= -ball.Width | ballPos.X >= 800 + ball.Width)
                {
                    ballPos = new Vector2((_graphics.PreferredBackBufferWidth / 2) - ball.Width / 2, (_graphics.PreferredBackBufferHeight / 2) - ball.Height / 2);
                    ballSpeed = new Vector2(0, 0);
                }
                //Basic cpue AI
                if (ballPos.Y > computerRect.Y)
                    computerRect.Y += (int)(computerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                else if (ballPos.Y < computerRect.Y)
                    computerRect.Y -= (int)(computerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            }
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (currentScreen == Screen.Title)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(titleFont, "PONG\nPress ENTER to play",
                    new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
            else if (currentScreen == Screen.Game)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(bumper, playerRect, Color.White);
                _spriteBatch.Draw(bumper, computerRect, Color.White);
                _spriteBatch.Draw(ball, ballPos, Color.White);
                _spriteBatch.End();
            }
            else if (currentScreen == Screen.GameOver)
            {

            }

            base.Draw(gameTime);
        }
    }
}
