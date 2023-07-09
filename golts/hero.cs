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
        public Hero(ContentManager contentManager, double x, double y, double movementX, double movementY)
            : base(contentManager, x, y, movementX, movementY, 5, true, "hero", @"boxes\hero", 5, 5, "id")
        { }

        public override void Update(ContentManager contentManager, World world)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Z) && CollidedY)
                ChangeMovement(0, -320);

            if (ks.IsKeyDown(Keys.Left))
                ChangeMovement(-3, 0);

            if (ks.IsKeyDown(Keys.Right))
                ChangeMovement(3, 0);

            if (Math.Abs(MovementX) < HitPresicion)
                Action = "id";
            else
                Action = "wa";

            base.Update(contentManager, world);
        }
    }
}