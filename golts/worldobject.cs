using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace golts
{
    public abstract class WorldObject
    {
        public double Weight { get; protected set; }

        public double MovementX { get; protected set; }
        public double MovementY { get; protected set; }
        public double X { get; protected set; }
        public double Y { get; protected set; }

        public DynamicTexture Texture { get; protected set; }
        public float DrawingDepth=0.0f;

        public WorldObject(ContentManager contentManager, double x, double y, double movementx, double movementy, double weight, string textureName)
        {
            X = x;
            Y = y;

            MovementX = movementx;
            MovementY = movementy;

            Weight = weight;

            Texture = new DynamicTexture(contentManager, textureName);
        }

        public virtual void Update(ContentManager contentManager, World world)
        {
            X += MovementX;
            Y += MovementY;

            ChangeMovement(-MovementX, -MovementY);

            Texture.Update(contentManager);
        }

        /// <summary>
        /// Draws object sprite centered horizontally and with bottom on y
        /// </summary>
        /// <param name="x">Sprite drawing location center</param>
        /// <param name="y">Sprite drawing location bottom</param>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        public virtual void Draw(int x, int y, SpriteBatch spriteBatch, float depth, float scale, Color color)
        {
            Texture2D spriteToDraw = Texture.GetCurrentFrame();
            spriteBatch.Draw(spriteToDraw, new Vector2(x - spriteToDraw.Width*scale / 2, y - spriteToDraw.Height*scale),
                null, color, 0f, new Vector2(0, 0), scale, SpriteEffects.None, depth + DrawingDepth);
        }

        /// <summary>
        /// Adds x and y to MovementX and MovementY respectively taking weight into account
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void ChangeMovement(double x, double y)
        {
            if (Weight != 0)
            {
                MovementX += x / Weight;
                MovementY += y / Weight;
            }
        }
    }
}