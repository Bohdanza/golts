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
    public class Hero : Mob
    {
        [JsonProperty]
        private bool inJump = false;
        [JsonProperty]
        private long timeSinceJump = 0;//actual time in GAME TICKS since last jump started
        private const int acceptabelDifference = 40;//acceptable difference  between jumps
        [JsonProperty]
        private int maxJumpingTime = 10; //maximum time after leaving the land during which a jump can be performed
        
        private const double actualStandartFallingSpeed= 5;

        [JsonConstructor]
        public Hero() { }

        public Hero(ContentManager contentManager, double x, double y, double movementX, double movementY)
            : base(contentManager, x, y, movementX, movementY, 5, true, "hero", @"boxes\hero", 5, 5, "id")
        {
            StandartFallingSpeed = actualStandartFallingSpeed;
        }

        public override void Update(ContentManager contentManager, World world)
        {
            var ks = Keyboard.GetState();

            timeSinceJump++;

            if (ks.IsKeyDown(Keys.Z) && ((timeSinceJump < maxJumpingTime && !CollidedY && inJump) || (!inJump && CollidedY)))
            {
                if (!inJump)
                {
                    inJump = true;
                    timeSinceJump = 0;
                }

                StandartFallingSpeed = 0.1;
                
                ChangeMovement(0, -40);
            }
            else
            {
                StandartFallingSpeed = actualStandartFallingSpeed;
            }

            if (inJump && CollidedY && StandartFallingSpeed==actualStandartFallingSpeed)
            {
                inJump = false;
            }

            if (ks.IsKeyDown(Keys.Left))
                ChangeMovement(-8.5, 0);

            if (ks.IsKeyDown(Keys.Right))
                ChangeMovement(8.5, 0);

            if (ks.IsKeyDown(Keys.C))
                world.objects.AddObject(new Obstacle(contentManager, X, Y, "hero_id_", new List<Tuple<double, double>>()));

            if (Math.Abs(MovementX) < HitPresicion)
                Action = "id";
            else
                Action = "wa";

            base.Update(contentManager, world);
        }

        public override void Draw(int xAbsolute, int yAbsolute, int xCamera, int yCamera, SpriteBatch spriteBatch, float depth, float scale, Color color, SpriteEffects spriteEffects)
        {
            base.Draw(xAbsolute, yAbsolute, xCamera, yCamera, spriteBatch, depth, scale, color, spriteEffects);

            spriteBatch.DrawString(Game1.debugFont, MovementY.ToString(), new Vector2(10, 10), Color.White);
        }
    }
}