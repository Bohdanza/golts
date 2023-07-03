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
        public ObjectHitbox Hitbox { get; protected set; }    

        public PhysicalObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight,
            string textureName,  string hitboxPath):
            base(contentManager, x, y, movementx, movementy, weight, textureName)
        {
            Hitbox = new ObjectHitbox(hitboxPath);  
        }

        public PhysicalObject(ContentManager contentManager, 
            double x, double y, double movementx, double movementy, double weight,
            string textureName, List<Tuple<double, double>> hitbox) :
            base(contentManager, x, y, movementx, movementy, weight, textureName)
        {
            Hitbox = new ObjectHitbox(hitbox);
        }
    }
}