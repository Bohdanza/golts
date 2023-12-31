﻿using Microsoft.VisualBasic;
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
        [JsonProperty]
        public double StandartFallingSpeed { get; protected set; } = 3;
        [JsonProperty]
        public double CurrentFallingSpeed { get; protected set; }

        [JsonProperty]
        public bool GravityAffected { get; protected set; }
        [JsonProperty]
        public double Weight { get; protected set; }

        [JsonProperty]
        public double MovementX { get; protected set; }
        [JsonProperty]
        public double MovementY { get; protected set; }
        [JsonProperty]
        public double X { get; protected set; }
        [JsonProperty]
        public double Y { get; protected set; }

        [JsonProperty]
        public DynamicTexture Texture { get; protected set; }
        [JsonProperty]
        public float DrawingDepth=0.0f;
        [JsonProperty]
        public float ParalaxCoefficient { get; protected set; } = 1.0f;

        [Newtonsoft.Json.JsonConstructor]
        public WorldObject() { }

        public WorldObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected, 
            string textureName, float paralaxCoefficient=1.0f)
        {
            ParalaxCoefficient = paralaxCoefficient;

            X = x;
            Y = y;

            MovementX = movementx;
            MovementY = movementy;

            Weight = weight;
            GravityAffected = gravityAffected;

            CurrentFallingSpeed = StandartFallingSpeed;

            Texture = new DynamicTexture(contentManager, textureName);
        }

        public virtual void Update(ContentManager contentManager, World world)
        {
            if (GravityAffected)
                MovementY += StandartFallingSpeed;

            X += MovementX;
            Y += MovementY;

            if (!GravityAffected)
                ChangeMovement(-MovementX, -MovementY);
            else
                ChangeMovement(-MovementX, 0);

            Texture.Update(contentManager);
        }

        /// <summary>
        /// Draws object sprite centered horizontally and with bottom on y
        /// </summary>
        /// <param name="x">Sprite drawing location center</param>
        /// <param name="y">Sprite drawing location bottom</param>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        public virtual void Draw(int xAbsolute, int yAbsolute, int xCamera, int yCamera,
            SpriteBatch spriteBatch, float depth, float scale, Color color, SpriteEffects spriteEffects)
        {
            int x1 = (int)(xCamera * ParalaxCoefficient) + xAbsolute;
            int y1 = (int)(yCamera * ParalaxCoefficient) + yAbsolute;

            Texture2D spriteToDraw = Texture.GetCurrentFrame();
            spriteBatch.Draw(spriteToDraw, new Vector2(x1 - spriteToDraw.Width * scale / 2, y1 - spriteToDraw.Height * scale),
                null, color, 0f, new Vector2(0, 0), scale, spriteEffects, depth + DrawingDepth);
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

        public virtual void ChangePosition(double x, double y)
        {
            X = Math.Max(0, x);
            Y = Math.Max(0, y);
        }
    }
}