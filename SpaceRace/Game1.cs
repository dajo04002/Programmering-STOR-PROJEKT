using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceRace
{
    internal class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sprites;
        private SpriteFont font;
        private Texture2D skeppTexture;
        private Texture2D debrisTexture;
        private Vector2 p1;
        private Vector2 p2;
        private Rectangle test;
        private Rectangle p1Box;
        private Rectangle p2Box;
        private Rectangle debrisBox;


        private List<Vector2> debris;
        private List<Rectangle> testList;

        private int debrisTimer = 60;
        private int p1StartX = 250;
        private int p2StartX = 500;
        private int StartY = 445;

        private bool p1Hit;
        private bool p2Hit;

        Random random = new Random();
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            p1 = new Vector2(p1StartX, StartY);
            p2 = new Vector2(p2StartX, StartY);

            debris = new List<Vector2>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprites = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Consolas16");
            skeppTexture = Content.Load<Texture2D>("skepp");
            debrisTexture = Content.Load<Texture2D>("debris");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.W))
            {
                p1.Y--;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.S))
            {
                p1.Y++;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Up))
            {
                p2.Y--;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Down))
            {
                p2.Y++;
            }

            //

            if (p1.Y < 0)
            {
                p1.Y = StartY;
            }
            else if (p1.Y > StartY)
            {
                p1.Y = StartY;
            }

            if (p2.Y < 0)
            {
                p2.Y = StartY;
            }
            else if (p2.Y > StartY)
            {
                p2.Y = StartY;
            }

            //

            debrisTimer--;
            if (debrisTimer <= 0)
            {
                debrisTimer = 20;
                debris.Add(new Vector2(0, random.Next(0, 400)));
            }

            for (int i = 0; i < debris.Count; i++)
            {
                debris[i] = debris[i] + new Vector2(1, 0);
            }

            //

            Rectangle p1Box = new Rectangle((int)p1.X, (int)p1.Y, skeppTexture.Width, skeppTexture.Height);
            Rectangle p2Box = new Rectangle((int)p2.X, (int)p2.Y, skeppTexture.Width, skeppTexture.Height);

            p1Hit = false;
            p2Hit = false;


            foreach (var debris in debris)
            {
                Rectangle debrisBox = new Rectangle((int)debris.X, (int)debris.Y, debrisTexture.Width, debrisTexture.Height);

                var kollision = Intersection(p1Box, debrisBox);

                if (kollision.Width > 0 && kollision.Height > 0)
                {
                    Rectangle r1 = Normalize(p1Box, kollision);
                    Rectangle r2 = Normalize(debrisBox, kollision);
                    p1Hit = TestCollision(skeppTexture, r1, debrisTexture, r2);
                }
            }

            foreach (var debris in debris)
            {
                Rectangle debrisBox = new Rectangle((int)debris.X, (int)debris.Y, debrisTexture.Width, debrisTexture.Height);

                var kollision = Intersection(p2Box, debrisBox);

                if (kollision.Width > 0 && kollision.Height > 0)
                {
                    Rectangle r1 = Normalize(p2Box, kollision);
                    Rectangle r2 = Normalize(debrisBox, kollision);
                    p2Hit = TestCollision(skeppTexture, r1, debrisTexture, r2);
                }
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sprites.Begin();
            

            if (p1Hit)
            {
                sprites.DrawString(font, "P1 DIED", new Vector2(p1StartX, 0), Color.White);
            }

            if (p2Hit)
            {
                sprites.DrawString(font, "P2 DIED", new Vector2(p2StartX, 0), Color.White);
            }

            sprites.Draw(skeppTexture, p1, Color.White);
            sprites.Draw(skeppTexture, p2, Color.White);

            foreach (var debris in debris)
            {
                sprites.Draw(debrisTexture, debris, Color.White);
            }
            sprites.End();
            base.Draw(gameTime);
        }
        public static Rectangle Intersection(Rectangle r1, Rectangle r2)
        {
            int x1 = Math.Max(r1.Left, r2.Left);
            int y1 = Math.Max(r1.Top, r2.Top);
            int x2 = Math.Min(r1.Right, r2.Right);
            int y2 = Math.Min(r1.Bottom, r2.Bottom);

            if ((x2 >= x1) && (y2 >= y1))
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
            return Rectangle.Empty;
        }
        public static Rectangle Normalize(Rectangle reference, Rectangle overlap)
        {
            //Räkna ut en rektangel som kan användas relativt till referensrektangeln
            return new Rectangle(
              overlap.X - reference.X,
              overlap.Y - reference.Y,
              overlap.Width,
              overlap.Height);
        }
        public static bool TestCollision(Texture2D t1, Rectangle r1, Texture2D t2, Rectangle r2)
        {
            //Beräkna hur många pixlar som finns i området som ska undersökas
            int pixelCount = r1.Width * r1.Height;
            uint[] texture1Pixels = new uint[pixelCount];
            uint[] texture2Pixels = new uint[pixelCount];

            //Kopiera ut pixlarna från båda områdena
            t1.GetData(0, r1, texture1Pixels, 0, pixelCount);
            t2.GetData(0, r2, texture2Pixels, 0, pixelCount);

            //Jämför om vi har några pixlar som överlappar varandra i områdena
            for (int i = 0; i < pixelCount; ++i)
            {
                if (((texture1Pixels[i] & 0xff000000) > 0) && ((texture2Pixels[i] & 0xff000000) > 0))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
