using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace golts
{
    public class Enemy : Mob
    {
        [JsonProperty]
        private bool isGoingRight = false;
        private PhysicalObject lastBorder;

        public Enemy(ContentManager contentManager, double x, double y, double movementX, double movementY, int weight,
            string textureName, string hitboxPath,
            int hP, int maxHP, string action) : base(contentManager, x, y, movementX, movementY, weight,
            true, textureName, hitboxPath,
            hP, maxHP, action)
            { }

        public override void Update(ContentManager contentManager, World world)
        {
            HashSet<PhysicalObject> nearbyBorders = world.objects.GetNearbyObjects(this, 2);
            foreach (PhysicalObject border in nearbyBorders)
            {
                if (Hitbox.CollidesWith(border.Hitbox, X, Y, border.X, border.Y) && lastBorder != border)
                {
                    isGoingRight = !isGoingRight;
                    lastBorder = border; 
                    break;
                }
            }

            if (isGoingRight && CollidedY)
            {
                ChangeMovement(3, 0);
            }
            else if (CollidedY)
            {
                ChangeMovement(-3, 0);
            }

            var nearbyObjcts = world.objects.GetNearbyObjects(this, CollisionLayer);
            Hero hero = world.Hero;
            

            if (nearbyObjcts.Contains(hero) && (collidedWith.Contains(hero) || hero.collidedWith.Contains(this)))
                {
                    GetHit(0, hero, 100);
                    hero.GetHit(0, this, 100);
                }
            base.Update(contentManager, world);
            
            bool a = nearbyObjcts.Contains(hero);
            bool b = collidedWith.Contains(hero);
        } 
    }
}
