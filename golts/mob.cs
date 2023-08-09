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
    public abstract class Mob:PhysicalObject
    {
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }
        public bool isKilled = false;

        public string Action { get; protected set; }
        protected string previousAction = "";

        public int Direction { get; protected set; }
        protected int previousDirection = -1;
        
        [JsonProperty]
        protected string standTextureName { get; init; }

        [JsonConstructor]
        public Mob() { }

        public Mob(ContentManager contentManager, double x, double y, double movementX, double movementY, int weight,
            bool gravityAffected, string textureName, string hitboxPath, 
            int hP, int maxHP, string action):
            base(contentManager, x, y, movementX, movementY, weight, gravityAffected, textureName, hitboxPath)
        {
            HP = hP;
            MaxHP = maxHP;

            Action = action;
            standTextureName = textureName;
        }

        public Mob(ContentManager contentManager, double x, double y, double movementX, double movementY, int weight,
            bool gravityAffected, string textureName, List<Tuple<double, double>> hitbox, 
            int hP, int maxHP, string action) :
            base(contentManager, x, y, movementX, movementY, weight, gravityAffected, textureName, hitbox)
        {
            HP = hP;
            MaxHP = maxHP;

            Action = action;
            standTextureName = textureName;
        }

        public override void Update(ContentManager contentManager, World world)
        {
            if (isKilled)
            {
                world.objects.DeleteObject(this);
            }
            if (MovementX > 0)
                Direction = 0;
            else if (MovementX < 0)
                Direction = 1;

            if (previousAction != Action)
            {
                Texture = new DynamicTexture(contentManager, standTextureName + "_" + Action + "_".ToString());
            }

            base.Update(contentManager, world);

            previousAction = Action;
            previousDirection = Direction;
        }

        public override void Draw(int xAbsolute, int yAbsolute, int xCamera, int yCamera, 
            SpriteBatch spriteBatch, float depth, float scale, Color color, SpriteEffects spriteEffects)
        {
            if (Direction == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            base.Draw(xAbsolute, yAbsolute, xCamera, yCamera, spriteBatch, depth, scale, color, spriteEffects);
        }

        public void getHit(int damage)
        {
            if (HP <= 0)
            {
                isKilled = true;
                return;
            }

            base.getHit();
        }
        
        public void getHit(int damage, PhysicalObject source, int power)
        {
            HP -= damage;
            
            if(HP <= 0)
            {
                isKilled = true;
                return;
            }
            else
            {
                Tuple<double, double> sourceCenter = source.Hitbox.geomCenter();
                Tuple<double, double> ownCenter = Hitbox.geomCenter();

                sourceCenter = new Tuple<double, double>(sourceCenter.Item1 + source.X, sourceCenter.Item2 + source.Y);
                ownCenter = new Tuple<double, double>(ownCenter.Item1 + X, ownCenter.Item2 + Y);

                Tuple<double, double> a = Game1.DirectionToTuple((float)Game1.GetDirection(sourceCenter, ownCenter));
            
                ChangeMovement(-power*a.Item1, -power*a.Item2*2);
                base.getHit();
            }
        }
    }
}