using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PointCluster
{
    public class PointClusterUniform : IPointClusterGenerator<Point>
    {
        public List<Point> Generate(int num, Bounds bounds)
        {
            return Enumerable.Range(0, num).Select(_ => 
                new Point()
                {
                    pos = bounds.randomInside(),
                    rot = Random.rotation
                }
                ).ToList();
        }
    }
}