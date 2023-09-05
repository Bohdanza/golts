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
    public class BlackenedObstacle : PhysicalObject
    {
        [JsonProperty]
        public int TextureWidth { get; protected set; }
        [JsonProperty]
        public int TextureHeight { get; protected set; }
        [JsonProperty]
        public Color TextureColor { get; protected set; }

        [JsonConstructor]
        public BlackenedObstacle() : base()
        { }

        public BlackenedObstacle(ContentManager contentManager,
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected,
            string textureName, string hitboxPath, int textureHeight, int textureWidth, int collisionLayer = 0)
            : base(contentManager, x, y, movementx, movementy, weight, gravityAffected, textureName, hitboxPath, collisionLayer)
        {
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
        }

        public override void Draw(int xAbsolute, int yAbsolute, int xCamera, int yCamera,
            SpriteBatch spriteBatch, float depth, float scale, Color color, SpriteEffects spriteEffects)
        {
            int x1 = (int)(xCamera * ParalaxCoefficient) + xAbsolute;
            int y1 = (int)(yCamera * ParalaxCoefficient) + yAbsolute;

            Texture2D spriteToDraw = Game1.OnePixel;
            spriteBatch.Draw(spriteToDraw, new Vector2(x1 - spriteToDraw.Width * scale / 2, y1 - spriteToDraw.Height * scale),
                null, TextureColor, 0f, new Vector2(0, 0), new Vector2(TextureWidth * scale, TextureHeight * scale),
                spriteEffects, depth + DrawingDepth);
        }
    }
}