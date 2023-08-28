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
        public bool ObtainedWJ {get; protected set;} = false; 
        
        [JsonProperty]
        public int MaxJumpsAmount { get; private set;}  = 1;
        
        private int currentJumpsCount = 0;
        private bool allowedToDoAdditionalJump = false;
        
        public bool clinged { get; protected set; } = false;

        public Hero() { }

        public Hero(ContentManager contentManager, double x, double y, double movementX, double movementY)
            : base(contentManager, x, y, movementX, movementY, 5, true, "hero", @"boxes\hero", 5, 5, "id")
        {
            StandartFallingSpeed = 1;
            ObtainDJ();
            ObtainedWJ = true;
        }

        public override void Update(ContentManager contentManager, World world)
        {
            var ks = Keyboard.GetState();

            timeSinceJump++;

            if (ks.IsKeyDown(Keys.Left) && (!clinged || clinged && Direction == 0))
            {
                ChangeMovement(-8.5, 0);
                clinged = false;
                GravityAffected = true;
            }

            if (ks.IsKeyDown(Keys.Right) && (!clinged || clinged && Direction == 1))
            {
                ChangeMovement(8.5, 0);
                clinged = false;
                GravityAffected = true;
            }

            if (ObtainedWJ && !clinged)
            {
                foreach (var collidedObject in collidedWith)
                {
                    if (collidedObject is Obstacle && collidedObject.IsClingable)
                    {
                        clinged = true;
                        GravityAffected = false;
                        MovementY = 0;
                        CollidedY = true;
                    }
                }
            }

            if (clinged)
                CollidedY = true;

            if (inJump && CollidedY && timeSinceJump > maxJumpingTime)
            {
                inJump = false;
                currentJumpsCount = 0;
                allowedToDoAdditionalJump = false;
            }
            bool a = ks.IsKeyDown(Keys.Z);
            if (a && ((timeSinceJump < maxJumpingTime && !CollidedY && inJump) 
                || (!inJump && CollidedY) || allowedToDoAdditionalJump || clinged) )
            {

                if (!inJump || clinged) //first jump
                {
                    currentJumpsCount++;
                    inJump = true;
                    timeSinceJump = 0;
                    GravityAffected = true;
                    clinged = false;
                }
                else if (allowedToDoAdditionalJump)
                {
                    currentJumpsCount++;
                    timeSinceJump = 0;
                    allowedToDoAdditionalJump = false;
                    MovementY = 0;
                    GravityAffected = true;
                }

                ChangeMovement(0, -35);
            }

            if (ObtainedDJ && !CollidedY && (currentJumpsCount < MaxJumpsAmount) && ks.IsKeyUp(Keys.Z))
            {
                allowedToDoAdditionalJump = true;
            }

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

        public void ObtainPen()
        {
            StandartFallingSpeed = 0.75d;
        }

        public void ObtainAddW()
        {
            StandartFallingSpeed = 1.25d;
        }
    }
}