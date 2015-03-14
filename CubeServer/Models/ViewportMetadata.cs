using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CubeServer.Models
{

    public class ViewportMetadata
    {
        public bool[,,] CubeExists { get; set; }
        public Extents Extents { get; set; }
        public Gridsize GridSize { get; set; }
    }

    public class Extents
    {
        public float XMax { get; set; }
        public float XMin { get; set; }
        public float YMax { get; set; }
        public float YMin { get; set; }
        public float ZMax { get; set; }
        public float ZMin { get; set; }
        public float XSize { get; set; }
        public float YSize { get; set; }
        public float ZSize { get; set; }
    }

    public class Gridsize
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

}