using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    abstract class GraphicsData
    {
        public int ID { get; }

        static readonly List<GraphicsData> graphicsDataInstances = new List<GraphicsData>();

        protected GraphicsData(int id)
        {
            ID = id;
            graphicsDataInstances.Add(this);
        }

        public abstract void Bind();

        protected abstract void Unbind();

        public virtual void Delete()
        {
            graphicsDataInstances.Remove(this);
        }

        public static void DeleteAllGraphicsDataInstances()
        {
            graphicsDataInstances.ForEach(graphicsDataInstance => {
                graphicsDataInstance.Delete();
            });
        }
    }
}
