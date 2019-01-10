using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PointCluster
{
    /// <summary>
    /// Based on @nobnak's SuperCluster package
    /// </summary>
    [System.Serializable]
    public class PointClusterCurlNoise :  PointClusterSimplexNoise
    {
        [System.Serializable]
        public class CurlParam
        {
            public float noiseScale = 50f;
            public float speedScale = 1f;
            public Vector3 offset0 = new Vector3(1f, 0f, -1f);
            public Vector3 offset1 = new Vector3(-1f, 1f, 0f);
            public Vector3 offset2 = new Vector3(0f, -1f, 1f);

            public float duration = 10f;
            public float dt = 0.5f;
        }

        public const double DELTA = 1e-3;

        public CurlParam curlParam;

        double _invNoiseScale;
        double _invTwoDelta;


        public override List<Point> Generate(int num, Bounds bounds)
        {
            Init();

            var points = base.Generate(num, bounds);
            var duration = curlParam.duration;
            var dt = curlParam.dt;
            for (var i = 0f; i < duration; i += dt)
            {
                for (var idx = 0; idx < points.Count; ++idx)
                {
                    var point = points[idx];
                    var velocity = Velosity(point.pos);

                    point.pos += velocity * dt;
                    point.rot = Quaternion.FromToRotation(Vector3.forward, velocity);

                    points[idx] = point;
                }
            }

            return points;
        }




        public void Init()
        {
            _invNoiseScale = 1.0 / curlParam.noiseScale;
            _invTwoDelta = 1.0 / (2.0 * DELTA);
        }

        public Vector3 Velosity(Vector3 pos)
        {
            var offset0 = curlParam.offset0;
            var offset1 = curlParam.offset1;
            var offset2 = curlParam.offset2;

            var dx = _PartialY(pos.x, pos.y, pos.z, offset2) - _PartialZ(pos.x, pos.y, pos.z, offset1);
            var dy = _PartialZ(pos.x, pos.y, pos.z, offset0) - _PartialX(pos.x, pos.y, pos.z, offset2);
            var dz = _PartialX(pos.x, pos.y, pos.z, offset1) - _PartialY(pos.x, pos.y, pos.z, offset0);
            return new Vector3((float)dx, (float)dy, (float)dz) * curlParam.speedScale;
        }

        private double _PartialX(double x, double y, double z, Vector3 offset)
        {
            return (SimplexNoise.noise(offset.x + x * _invNoiseScale + DELTA, offset.y + y * _invNoiseScale, offset.z + z * _invNoiseScale)
                    - SimplexNoise.noise(offset.x + x * _invNoiseScale - DELTA, offset.y + y * _invNoiseScale, offset.z + z * _invNoiseScale))
                * _invTwoDelta;
        }
        private double _PartialY(double x, double y, double z, Vector3 offset)
        {
            return (SimplexNoise.noise(offset.x + x * _invNoiseScale, offset.y + y * _invNoiseScale + DELTA, offset.z + z * _invNoiseScale)
                    - SimplexNoise.noise(offset.x + x * _invNoiseScale, offset.y + y * _invNoiseScale - DELTA, offset.z + z * _invNoiseScale))
                * _invTwoDelta;
        }
        private double _PartialZ(double x, double y, double z, Vector3 offset)
        {
            return (SimplexNoise.noise(offset.x + x * _invNoiseScale, offset.y + y * _invNoiseScale, offset.z + z * _invNoiseScale + DELTA)
                    - SimplexNoise.noise(offset.x + x * _invNoiseScale, offset.y + y * _invNoiseScale, offset.z + z * _invNoiseScale - DELTA))
                * _invTwoDelta;
        }
    }
}