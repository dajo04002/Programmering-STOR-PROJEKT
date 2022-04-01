using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame
{
    public class Game1 : Game
    { 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D skepp;
        Rectangle rect1;
        Rectangle rect2;

        int rect1startX = 250;
        int rect2startX = 500;
        int rectStart = 445;
        



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }
        protected override void Initialize()
        {
            base.Initialize();

        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            skepp = Content.Load<Texture2D>("skepp");
            rect1 = new Rectangle(rect1startX,rectStart,20,30);
            rect2 = new Rectangle(rect2startX,rectStart,20,30);
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                rect1.Y --;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                rect1.Y ++;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                rect2.Y --;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rect2.Y ++;
            }

            if (rect1.Y < 0)
            {
                rect1.Y = rectStart;
            }
            else if (rect1.Y > rectStart)
            {
                rect1.Y = rectStart;
            }

            if (rect2.Y < 0)
            {
                rect2.Y = rectStart;
            }
            else if (rect2.Y > rectStart)
            {
                rect2.Y = rectStart;
            }

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(skepp, rect1, Color.Cyan);
            _spriteBatch.Draw(skepp, rect2, Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
