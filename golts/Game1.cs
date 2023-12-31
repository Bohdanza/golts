﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace golts
{
    public class Game1 : Game
    {
        public static Texture2D NoTexture;
        public static Texture2D OnePixel;
        public static float StandardScale = 4f;
        public static SpriteFont debugFont;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World world;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            _graphics.ApplyChanges();

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            _graphics.ApplyChanges();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();

            _graphics.IsFullScreen = false;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            world = new World(Content, "saves\\1", false);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            NoTexture = Content.Load<Texture2D>("missingtexture");
            OnePixel = Content.Load<Texture2D>("onepixel");
            debugFont = Content.Load<SpriteFont>("debugfont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                world.Save();
                Exit();
            }

            // TODO: Add your update logic here

            world.Update(Content);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(128, 128, 128));

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp);
            world.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static double GetDistance(Tuple<double, double> t1, Tuple<double, double> t2)
        {
            return Math.Sqrt((t1.Item1 - t2.Item1) * (t1.Item1 - t2.Item1) +
                (t1.Item2 - t2.Item2) * (t1.Item2 - t2.Item2));
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double GetDirection(double x1, double y1, double x2, double y2)
        {
            return Math.Atan2(y1-y2, x1 - x2);
        }

        public static double GetDirection(Tuple<double, double> t1, Tuple<double, double> t2)
        {
            return (float)Math.Atan2(t1.Item2-t2.Item2, t1.Item1-t2.Item1);
        }

        public static Tuple<double, double> DirectionToTuple(float direction)
        {
            return new Tuple<double, double>(Math.Cos(direction), Math.Sin(direction));
        }
    }
}