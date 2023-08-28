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
using System.Timers;
using System.Transactions;

namespace golts
{
    public class World
    {
        //1920*3
        public const int MaxLoadedSize = 5760;

        public string Path { get; private set; }
        public string Name { get; private set; }

        public int RoomIndex { get; private set; } = 0;

        public ObjectList objects { get; private set; }

        //why not
        public Camera WorldCamera { get; private set; }
        public Hero Hero { get; private set; }

        private bool hitboxesShown = true, hitbordersShown = true;
        private int roomForChange = -1, exitIndex = -1;

        //Later these init methods shall be made one for the good code style rejoice.
        //It should automatically check for saves and load or create new depending on found ones

        /// <summary>
        /// Use this to init new world TEMPORARILY
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="path"></param>
        public World(ContentManager contentManager, string path, bool Rewrite)
        {
            if (path[path.Length - 1] != '\\')
                path += "\\";

            Path = path;

            if (Rewrite)
            {
                objects = new ObjectList(MaxLoadedSize);

                //TODO: add copying world from standard world temp
                objects.AddObject(new TestClass(contentManager, 800, 800));
                objects.AddObject(new Obstacle(contentManager, 1100, 500, "testing1", @"boxes\wall1", true));
                objects.AddObject(new Obstacle(contentManager, 800, 500, "testing1", @"boxes\wall1", true));



                Hero = new Hero(contentManager, 1000, 300, 0, 0);
                objects.AddObject(Hero);
            }
            else
            {
                Load();
                foreach(var currentObject in objects.objects)
                    if(currentObject is Hero)
                        Hero = (Hero)currentObject;
            }

            WorldCamera = new Camera(contentManager, Hero.X, Hero.Y, 0, 0, 1);
        }

        public void Update(ContentManager contentManager)
        {
            roomForChange = -1;

            for(int i=0; i<objects.objects.Count; i++)
            {
                objects.objects[i].Update(contentManager, this);
            }

            WorldCamera.ChangeMovement(
                Math.Min(Math.Max(Hero.X - 960 - WorldCamera.X, -Camera.MaxMovementSpeed), Camera.MaxMovementSpeed),
                Math.Min(Math.Max(Hero.Y - 540 - WorldCamera.Y, -Camera.MaxMovementSpeed), Camera.MaxMovementSpeed));

            WorldCamera.Update(contentManager, this);

            if (roomForChange != -1)
                ActuallyChangeRoom();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var currentObject in objects.objects)
            {
                currentObject.Draw((int)currentObject.X, (int)currentObject.Y, -(int)WorldCamera.X, -(int)WorldCamera.Y,
                    spriteBatch, 0.5f, Game1.StandardScale, Color.White, SpriteEffects.None);

                if (hitboxesShown && currentObject is PhysicalObject)
                    ((PhysicalObject)currentObject).Hitbox.Draw(
                        (int)(currentObject.X - WorldCamera.X), (int)(currentObject.Y - WorldCamera.Y),
                        spriteBatch, 0.9f, Color.White);
            }

            if(hitbordersShown)
            {
                int bgx = (int)((-WorldCamera.X % ObjectList.GridCellSize) + ObjectList.GridCellSize) % ObjectList.GridCellSize;
                int bgy = (int)((-WorldCamera.Y % ObjectList.GridCellSize) + ObjectList.GridCellSize) % ObjectList.GridCellSize;

                for (int i = bgx; i <= 1920; i+= ObjectList.GridCellSize)
                    spriteBatch.Draw(Game1.OnePixel, new Vector2(i, 0), null, Color.Yellow, 0f,
                        new Vector2(0, 0), new Vector2(1, 1080), SpriteEffects.None, 1f);

                for (int i = bgy; i <= 1080; i += ObjectList.GridCellSize)
                    spriteBatch.Draw(Game1.OnePixel, new Vector2(0, i), null, Color.Yellow, 0f,
                        new Vector2(0, 0), new Vector2(1920, 1), SpriteEffects.None, 1f);
            }
        }

        public void ChangeRoom(int roomIndex, int exitObjectIndex)
        {
            roomForChange=roomIndex; exitIndex=exitObjectIndex;
        }
        
        private void ActuallyChangeRoom()
        {
            objects.DeleteObject(Hero);
            SaveRoom();

            RoomIndex = roomForChange;

            LoadRoom(RoomIndex);

            bool found = false;

            for (int i = 0; i < objects.objects.Count && !found; i++)
                if (objects.objects[i] is ExitPoint && ((ExitPoint)objects.objects[i]).Index==exitIndex)
                {
                    found = true;
                    Hero.ChangePosition(objects.objects[i].X, objects.objects[i].Y);
                }
            

            objects.AddObject(Hero);
        }

        public void Save()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            using (StreamWriter sw = new StreamWriter(Path + "currentroom"))
                sw.WriteLine(RoomIndex);

            SaveRoom();
        }

        private void SaveRoom()
        {
            var jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.Objects;

            using (StreamWriter sw=new StreamWriter(Path+RoomIndex.ToString()))
            {
                sw.Write(JsonConvert.SerializeObject(objects, jss));
            }
        }

        private void Load()
        {
            using (StreamReader sr = new StreamReader(Path + "currentroom"))
                RoomIndex = int.Parse(sr.ReadLine());

            LoadRoom(RoomIndex);
        }

        private void LoadRoom(int index)
        {
            var jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.Objects;

            using (StreamReader sr = new StreamReader(Path + index.ToString()))
            {
                objects = (ObjectList)JsonConvert.DeserializeObject(sr.ReadToEnd(), jss);
            }
        }
    }
}