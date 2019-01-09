using System.Collections.Generic;
using UnityEngine;

namespace PointCluster
{
    interface IPointClusterGenerator<T>
    {
        List<T> Generate(int num, Bounds bounds);
    }
}