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
using System.Net.NetworkInformation;
using System.Transactions;

namespace golts
{
    public class Camera : WorldObject
    {
        public const double MaxMovementSpeed = 10;

        public Camera(ContentManager contentManager, double x, double y, double movementx, double movementy, double weight)
            : base(contentManager, x, y, movementx, movementy, weight, false, "") { }
    }
}