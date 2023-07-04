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
    public class ObjectList
    {
        public const int GridCellSize = 120;

        public List<WorldObject> objects { get; private set; }
        public List<PhysicalObject>[,] ObjectGrid { get; private set; }

        public int GridSize { get; private set; }

        /// <summary>
        /// Init normally
        /// </summary>
        /// <param name="worldSize">Size of the LOADED world. ]
        /// </param>
        public ObjectList(int worldSize)
        {
            GridSize = worldSize / GridCellSize;

            objects = new List<WorldObject>();
            ObjectGrid=new List<PhysicalObject>[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
                for (int j = 0; j < GridSize; j++)
                    ObjectGrid[i, j] = new List<PhysicalObject>();
        }

        public void AddObject(WorldObject worldObject)
        {
            objects.Add(worldObject);
            
            if(worldObject is PhysicalObject)
            {
                PhysicalObject po = (PhysicalObject)worldObject;
                double xBegin = Math.Max(0, po.X + po.Hitbox.MinX - GridCellSize);
                double xEnd = Math.Min(GridSize * GridCellSize, po.X + po.Hitbox.MaxX+GridCellSize);
                double yBegin = Math.Max(0, po.Y + po.Hitbox.MinY- GridCellSize);
                double yEnd = Math.Min(GridSize * GridCellSize, po.Y + po.Hitbox.MaxY + GridCellSize);

                for (double i = xBegin; i < xEnd; i += GridCellSize)
                    for (double j = yBegin; j < yEnd; j += GridCellSize)
                        ObjectGrid[(int)(i / GridCellSize), (int)(j / GridCellSize)].Add(po);
            }
        }

        public void UpdateObjectPosition(PhysicalObject physicalObject, double previousX, double previousY)
        {
            if ((int)(physicalObject.X / GridCellSize) != (int)(previousX / GridCellSize)|| 
                (int)(physicalObject.Y / GridCellSize) != (int)(previousY / GridCellSize))
            {
                double xBegin = Math.Max(0, previousX + physicalObject.Hitbox.MinX-GridCellSize);
                double xEnd = Math.Min(GridSize * GridCellSize, previousX + physicalObject.Hitbox.MaxX+GridCellSize);
                double yBegin = Math.Max(0, previousY + physicalObject.Hitbox.MinY - GridCellSize);
                double yEnd = Math.Min(GridSize * GridCellSize, previousY + physicalObject.Hitbox.MaxY+GridCellSize);

                for (double i = xBegin; i < xEnd; i += GridCellSize)
                    for (double j = yBegin; j < yEnd; j += GridCellSize)
                        ObjectGrid[(int)(i / GridCellSize), (int)(j / GridCellSize)].Remove(physicalObject);

                xBegin = Math.Max(0, physicalObject.X + physicalObject.Hitbox.MinX - GridCellSize);
                xEnd = Math.Min(GridSize * GridCellSize, physicalObject.X + physicalObject.Hitbox.MaxX+ GridCellSize);
                yBegin = Math.Max(0, physicalObject.Y + physicalObject.Hitbox.MinY - GridCellSize);
                yEnd = Math.Min(GridSize * GridCellSize, physicalObject.Y + physicalObject.Hitbox.MaxY + GridCellSize);

                for (double i = xBegin; i < xEnd; i += GridCellSize)
                    for (double j = yBegin; j < yEnd; j += GridCellSize)
                        ObjectGrid[(int)(i / GridCellSize), (int)(j / GridCellSize)].Add(physicalObject);
            }
        }

        /// <summary>
        /// Returns objects that physicalObject may collide with (including this object)
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public HashSet<PhysicalObject> GetNearbyObjects(PhysicalObject physicalObject)
        {
            HashSet<PhysicalObject> objects = new HashSet<PhysicalObject>();

            double xBegin = Math.Max(0, physicalObject.X + physicalObject.Hitbox.MinX - GridCellSize);
            double xEnd = Math.Min(GridSize * GridCellSize, physicalObject.X + physicalObject.Hitbox.MaxX + GridCellSize);
            double yBegin = Math.Max(0, physicalObject.Y + physicalObject.Hitbox.MinY - GridCellSize);
            double yEnd = Math.Min(GridSize * GridCellSize, physicalObject.Y + physicalObject.Hitbox.MaxY + GridCellSize);

            for (double i = xBegin; i < xEnd; i += GridCellSize)
                for (double j = yBegin; j < yEnd; j += GridCellSize)
                    objects.UnionWith(ObjectGrid[(int)(i / GridCellSize), (int)(j / GridCellSize)]);

            return objects;
        }
    }
}