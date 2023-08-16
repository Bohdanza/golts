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
        //Strange things happen when it's less than 0.001
        public const double HitPresicion = 0.1;

        [JsonProperty]
        public ObjectHitbox Hitbox { get; protected set; }

        [JsonProperty]
        public int CollisionLayer { get; protected set; }

        [JsonIgnore]
        protected bool CollidedY = false, CollidedX = false;
        [JsonIgnore]
        public List<PhysicalObject> collidedWith { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        public PhysicalObject() : base() { }

        public PhysicalObject(ContentManager contentManager,
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected,
            string textureName, string hitboxPath, int collisionLayer = 0) :
            base(contentManager, x, y, movementx, movementy, weight, gravityAffected, textureName)
        {
            Hitbox = new ObjectHitbox(hitboxPath);
            CollisionLayer = collisionLayer;
        }

        public PhysicalObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight, bool gravityAffected,
            string textureName, List<Tuple<double, double>> hitbox, int collisionLayer = 0) :
            base(contentManager, x, y, movementx, movementy, weight, gravityAffected, textureName)
        {
            Hitbox = new ObjectHitbox(hitbox);
            CollisionLayer = collisionLayer;
        }

        public override void Update(ContentManager contentManager, World world)
        {
            double px = X;
            double py = Y;

            CollidedY = false;
            CollidedX = false;
            collidedWith = new List<PhysicalObject>();
            
            if (GravityAffected)
            {
                Y += CurrentFallingSpeed;
                CurrentFallingSpeed += StandartFallingSpeed;
            }

            Y += MovementY;
            
            world.objects.UpdateObjectPosition(this, px, py);

            if (Math.Abs(py - Y) > HitPresicion)
            {
                HashSet<PhysicalObject> relatedObjects = world.objects.GetNearbyObjects(this, CollisionLayer);
                
                updateCollidedList(relatedObjects);
                
                if (Obstructed(relatedObjects))
                {
                    CollidedY = true;
                    double l = py, r = Y;

                    while(Math.Abs(l-r)>HitPresicion)
                    {
                        double mid = (l + r) / 2;
                        Y = mid;

                        if (Obstructed(relatedObjects))
                            r = mid;
                        else
                            l = mid;
                    }

                    Y = r;

                    if (Obstructed(relatedObjects))
                        Y = l;

                    MovementY = StandartFallingSpeed;
                    CurrentFallingSpeed = StandartFallingSpeed;
                }
            }

            X += MovementX;

            world.objects.UpdateObjectPosition(this, px, py);

            if (Math.Abs(px - X) > HitPresicion)
            {
                HashSet<PhysicalObject> relatedObjects = world.objects.GetNearbyObjects(this, CollisionLayer);

                updateCollidedList(relatedObjects);

                if (Obstructed(relatedObjects))
                {
                    CollidedX = true;
                    double l = px, r = X;

                    while (Math.Abs(l - r) > HitPresicion)
                    {
                        double mid = (l + r) / 2;
                        X = mid;

                        if (Obstructed(relatedObjects))
                            r = mid;
                        else
                            l = mid;
                    }

                    X = r;

                    if (Obstructed(relatedObjects))
                        X = l;
                    
                    MovementX = 0;
                }
            }

            world.objects.UpdateObjectPosition(this, px, py);

            ChangeMovement(-MovementX, -MovementY);

            Texture.Update(contentManager);
        }

        private bool Obstructed(HashSet<PhysicalObject> relatedObjects)
        {
            foreach (var currentObject in relatedObjects)
                if (currentObject != this && Hitbox.CollidesWith(currentObject.Hitbox, X, Y, currentObject.X, currentObject.Y))
                {
                    return true;
                }

            return false;
        }

        private void updateCollidedList(HashSet<PhysicalObject> relatedObjects)
        {
            foreach (var currentObject in relatedObjects)
                if (currentObject != this && Hitbox.CollidesWith(currentObject.Hitbox, X, Y, currentObject.X, currentObject.Y))
                {
                    collidedWith.Add(currentObject);
                }
        }

        public void GetHit()
        {
            //Throw some particles
        }
    }
}