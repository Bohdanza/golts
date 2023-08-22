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
        [JsonProperty]
        private int maxJumpingTime = 4; //maximum time after leaving the land during which a jump can be performe

        [JsonProperty]
        public bool ObtainedDJ { get; protected set; } = false; //DJ = double jump
        [JsonProperty]
        public int MaxJumpsAmount { get; private set;}  = 1;
        
        private int currentJumpsCount = 0;
        private bool allowedToDoAdditionalJump = false;
        private bool ZReleased = false;
    public Hero() { }

        public Hero(ContentManager contentManager, double x, double y, double movementX, double movementY)
            : base(contentManager, x, y, movementX, movementY, 5, true, "hero", @"boxes\hero", 5, 5, "id")
        {
            StandartFallingSpeed = 1;
            ObtainDJ();
        }

        public override void Update(ContentManager contentManager, World world)
        {
            var ks = Keyboard.GetState();

            timeSinceJump++;

            if (inJump && CollidedY && timeSinceJump>maxJumpingTime)
            {
                inJump = false;
                currentJumpsCount = 0;
                allowedToDoAdditionalJump = false;
            }

            if (ks.IsKeyDown(Keys.Z) && ((timeSinceJump < maxJumpingTime && !CollidedY && inJump ) || (!inJump && CollidedY) || (allowedToDoAdditionalJump)))
            {
                
                if (!inJump)
                {
                    currentJumpsCount++;
                    inJump = true;
                    ZReleased = false;
                    timeSinceJump = 0;
                }
                
                if(allowedToDoAdditionalJump)
                {
                    currentJumpsCount++;
                    ZReleased = false;
                    timeSinceJump = 0;
                    allowedToDoAdditionalJump = false;
                    MovementY = 0;
                }
                
                ChangeMovement(0, -35);
            }
            
            if (ObtainedDJ && !CollidedY && ks.IsKeyUp(Keys.Z))
            {
                if (currentJumpsCount < MaxJumpsAmount)
                {
                    allowedToDoAdditionalJump = true;
                }
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
        public void ObtainDJ()
        {
            ObtainedDJ = true;
            MaxJumpsAmount = 2;
        }

    }
}