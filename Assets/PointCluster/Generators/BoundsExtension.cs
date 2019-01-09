using UnityEngine;


namespace PointCluster
{
    public static class BoundsExtension
    {
        public static Vector3 randomInside(this Bounds bounds)
        {
            var min = bounds.min;
            var max = bounds.max;
#if false
            var r = new System.Random();

            return new Vector3(
                Mathf.Lerp(min.x, max.x, (float)r.NextDouble()),
                Mathf.Lerp(min.y, max.y, (float)r.NextDouble()),
                Mathf.Lerp(min.z, max.z, (float)r.NextDouble())
                );
#else
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
    );

#endif
        }
    }

}