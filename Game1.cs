using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            ballPos = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = new Vector2(-800, 0);
            playerSpeed = 200;
            computerSpeed = 500;
            bumper = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            bumper.SetData(data);
            ball = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            ball.SetData(data);
            ballPos = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Up))
                playerRect.Y -= (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kState.IsKeyDown(Keys.Down))
                playerRect.Y += (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (playerRect.Y < 0)
                playerRect.Y = 0;
            else if (playerRect.Y > _graphics.PreferredBackBufferHeight - playerRect.Height)
                playerRect.Y = _graphics.PreferredBackBufferHeight - playerRect.Height;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(bumper, playerRect, Color.White);
            _spriteBatch.Draw(bumper, computerRect, Color.White);
            _spriteBatch.Draw(ball, ballPos, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
