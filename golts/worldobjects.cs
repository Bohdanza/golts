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
                double xBegin = Math.Max(0, po.X + po.Hitbox.MinX);
                double xEnd = Math.Min(GridSize * GridCellSize, po.X + po.Hitbox.MaxX);
                double yBegin = Math.Max(0, po.Y + po.Hitbox.MinY);
                double yEnd = Math.Min(GridSize * GridCellSize, po.Y + po.Hitbox.MaxY);

                for (double i = xBegin; i < xEnd; i += GridCellSize)
                    for (double j = yBegin; j < yEnd; j += GridCellSize)
                        ObjectGrid[(int)(i / GridCellSize), (int)(j / GridCellSize)].Add(po);
            }
        }
    }
}