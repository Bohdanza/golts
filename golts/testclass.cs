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
    public class TestClass:PhysicalObject
    {
        public TestClass(ContentManager contentManager, double x, double y):base(contentManager, x, y, 0, 0, 4, false,
            "testing1", @"boxes\testing10")
        {}
    }

    public class TestClass2 : PhysicalObject
    {
        public TestClass2(ContentManager contentManager, double x, double y) : base(contentManager, x, y, 0, 0, 4, false,
            "testing2", @"boxes\testing20")
        { }

        public override void Update(ContentManager contentManager, World world)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.W))
                ChangeMovement(0, -5);

            if (ks.IsKeyDown(Keys.S))
                ChangeMovement(0, 5);

            if (ks.IsKeyDown(Keys.A))
                ChangeMovement(-5, 0);

            if (ks.IsKeyDown(Keys.D))
                ChangeMovement(5, 0);

            base.Update(contentManager, world);
        }
    }
}