using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace golts
{
    public class Border:PhysicalObject
    {
        public Border(ContentManager contentManager, double x, double y) :
            base(contentManager, x, y, 0, 0, 0, false, "transparent", @"boxes\border", 2)
        { }
        public Border(ContentManager contentManager, double x, double y, List<Tuple<double, double>> hitbox) :
            base(contentManager, x, y, 0, 0, 0, false, "transparent", hitbox, 2)
        { }
        public override void Update(ContentManager contentManager, World world)
        { }
        
    }
}