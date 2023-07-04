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
    public class World
    {
        //1920*3
        public const int MaxLoadedSize = 5760;

        public string Path { get; private set; }
        public string Name { get; private set; }

        public ObjectList objects { get; private set; }

        //why not
        public WorldObject Camera { get; private set; }

        private bool HitboxesShown = true;

        public World(ContentManager contentManager, string path)
        {
            objects = new ObjectList(MaxLoadedSize);

            objects.AddObject(new TestClass(contentManager, 500, 500));

            objects.AddObject(new TestClass2(contentManager, 800, 500));
        }

        public void Update(ContentManager contentManager)
        {
            for(int i=0; i<objects.objects.Count; i++)
            {
                objects.objects[i].Update(contentManager, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var currentObject in objects.objects)
            {
                currentObject.Draw((int)currentObject.X, (int)currentObject.Y, spriteBatch, 0f, 4f, Color.White);

                if (HitboxesShown && currentObject is PhysicalObject)
                    ((PhysicalObject)currentObject).Hitbox.Draw((int)currentObject.X, (int)currentObject.Y, 
                        spriteBatch, 0f, Color.White);
            }

            for (int i = 0; i <= 1920; i += ObjectList.GridCellSize)
                spriteBatch.Draw(Game1.OnePixel, new Vector2(i, 0), null, Color.Yellow, 0f, new Vector2(0, 0),
                    new Vector2(1, 1080), SpriteEffects.None, 1f);

            for (int i = 0; i <= 1080; i += ObjectList.GridCellSize)
                spriteBatch.Draw(Game1.OnePixel, new Vector2(0, i), null, Color.Yellow, 0f, new Vector2(0, 0),
                    new Vector2(1920, 1), SpriteEffects.None, 1f);
        }
    }
}