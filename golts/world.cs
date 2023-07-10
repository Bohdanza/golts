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

        [JsonProperty]
        public int RoomIndex { get; private set; } = 0;

        public ObjectList objects { get; private set; }

        //why not
        public WorldObject Camera { get; private set; }

        private bool HitboxesShown = true;

        public World(ContentManager contentManager, string path)
        {
            if (path[path.Length - 1] != '\\')
                path += "\\";

            Path = path;

            objects = new ObjectList(MaxLoadedSize);

            objects.AddObject(new TestClass(contentManager, 800, 800));

            objects.AddObject(new Hero(contentManager, 800, 300, 0, 0));
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
                currentObject.Draw((int)currentObject.X, (int)currentObject.Y, spriteBatch, 0f, 4f, Color.White, SpriteEffects.None);

                if (HitboxesShown && currentObject is PhysicalObject)
                    ((PhysicalObject)currentObject).Hitbox.Draw((int)currentObject.X, (int)currentObject.Y, 
                        spriteBatch, 0f, Color.White);
            }
        }

        public void Save()
        {
            SaveRoom();
        }

        private void SaveRoom()
        {
            var jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.Objects;

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            using (StreamWriter sw = new StreamWriter(Path + "currentroom"))
                sw.WriteLine(RoomIndex);

            using (StreamWriter sw=new StreamWriter(Path+RoomIndex.ToString()))
            {
                sw.Write(JsonConvert.SerializeObject(objects, jss));
            }
        }
    }
}