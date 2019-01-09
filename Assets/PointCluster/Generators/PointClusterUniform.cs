using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PointCluster
{
    public class PointClusterUniform : IPointClusterGenerator<Vector3>
    {
        public List<Vector3> Generate(int num, Bounds bounds)
        {
            return Enumerable.Range(0, num).Select(_ => bounds.randomInside()).ToList();
        }
    }
}