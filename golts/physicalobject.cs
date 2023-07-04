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
    //It's like WorldObject but it has hitbox and stuff
    public abstract class PhysicalObject:WorldObject
    {
        protected int PrevFallingSpeed = StandartFallingSpeed;

        public ObjectHitbox Hitbox { get; protected set; }

        protected bool CollidedY = false, CollidedX = false;

        public PhysicalObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected,
            string textureName,  string hitboxPath):
            base(contentManager, x, y, movementx, movementy, weight, gravityAffected, textureName)
        {
            Hitbox = new ObjectHitbox(hitboxPath);  
        }

        public PhysicalObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected,
            string textureName, List<Tuple<double, double>> hitbox) :
            base(contentManager, x, y, movementx, movementy, weight, gravityAffected, textureName)
        {
            Hitbox = new ObjectHitbox(hitbox);
        }

        public override void Update(ContentManager contentManager, World world)
        {
            double px = X;
            double py = Y;

            if (GravityAffected)
            {
                if (!CollidedY)
                {
                    MovementY += StandartFallingSpeed;
                    PrevFallingSpeed = StandartFallingSpeed;
                }
                else
                {
                    MovementY += PrevFallingSpeed;
                    PrevFallingSpeed /= 2;
                }
            }

            CollidedY = false;
            CollidedX = false;

            Y += MovementY;

            world.objects.UpdateObjectPosition(this, px, py);

            if (Math.Abs(py - Y) > 0.0000001)
            {
                HashSet<PhysicalObject> relatedObjects = world.objects.GetNearbyObjects(this);

                foreach (var currentObject in relatedObjects)
                    if (currentObject!=this&&Hitbox.CollidesWith(currentObject.Hitbox, X, Y, currentObject.X, currentObject.Y))
                    {
                        Y = py;
                        MovementY /=2;
                        CollidedY = true;

                        //I'm sorry.
                        break;
                    }
            }

            X += MovementX;

            world.objects.UpdateObjectPosition(this, px, py);

            if (Math.Abs(px - X) > 0.0000001)
            {
                HashSet<PhysicalObject> relatedObjects = world.objects.GetNearbyObjects(this);

                foreach (var currentObject in relatedObjects)
                    if (currentObject != this && Hitbox.CollidesWith(currentObject.Hitbox, X, Y, currentObject.X, currentObject.Y))
                    {
                        X = px;
                        MovementX /=2;
                        CollidedX = true;

                        //I'm sorry. I copypasted this (I solemnly swear it was tested)
                        break;
                    }
            }

            world.objects.UpdateObjectPosition(this, px, py);

            ChangeMovement(-MovementX, -MovementY);

            Texture.Update(contentManager);
        }
    }
}