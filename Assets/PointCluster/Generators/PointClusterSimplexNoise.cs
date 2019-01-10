using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PointCluster
{
    /// <summary>
    /// Based on @nobnak's SuperCluster package
    /// </summary>    
    public class PointClusterSimplexNoise : IPointClusterGenerator<Point>
    {
        protected struct DVec3
        {
            public double x;
            public double y;
            public double z;

            public static implicit operator DVec3(Vector3 v)
            {
                return new DVec3()
                {
                    x = v.x,
                    y = v.y,
                    z = v.z
                };
            }

            public static implicit operator Vector3(DVec3 dv)
            {
                return new Vector3((float)dv.x, (float)dv.y, (float)dv.z);
            }
        };

        [System.Serializable]
        public class NoiseParam
        {
            public float sd = 1f;
            public float noiseSize = 10f;
            public int walkMax = 20;
        }

        public int numPerThread = 100000;
        public double blackLevel = 1e-6;


        public NoiseParam param;


        Vector3 _min;
        //Vector3 _max;
        Vector3 _size;
        double _invNoiseSize;

        public virtual List<Point> Generate(int num, Bounds bounds)
        {
            _size = bounds.size;
            _min = bounds.min;
            //_max = bounds.max;
            _invNoiseSize = 1.0 / param.noiseSize;

            return MCMC(num, bounds).ToList();

        }

        protected IEnumerable<Point> MCMC(int num, Bounds bounds)
        {
            (double current, DVec3 pos) = Init(bounds);

            for (var i = 0; i < num; ++i)
            {
                Walk(ref current, ref pos);
                yield return new Point() { pos = pos, rot = Quaternion.identity };
            }
        }

        protected (double, DVec3) Init(Bounds bounds)
        {
            DVec3 pos = bounds.randomInside();
            var current = Height(pos);

            //Walk(ref current, ref pos);

            return (current, pos);
        }

        protected void Walk(ref double current, ref DVec3 pos)
        {
            for (var i = 0; i < param.walkMax; ++i)
            {
                if (_Walk(ref current, ref pos)) break;
            }
        }

        protected bool _Walk(ref double current, ref DVec3 pos)
        {
            var sd = param.sd;
            var rand = BoxMuller.Gaussian() * sd;

            DVec3 posNext;
            posNext.x = pos.x + rand.x;
            posNext.y = pos.y + rand.y;
            rand = BoxMuller.Gaussian() * sd;
            posNext.z = pos.z + rand.x;

            posNext.x -= _size.x * Math.Floor((posNext.x - _min.x) / _size.x);
            posNext.y -= _size.y * Math.Floor((posNext.y - _min.y) / _size.y);
            posNext.z -= _size.z * Math.Floor((posNext.z - _min.z) / _size.z);

            var next = Height(posNext);
            var change = (float)(new System.Random()).NextDouble() < (next / current);
            if (change)
            {
                pos = posNext;
                current = next;
            }

            return change;
        }

        protected double Height(DVec3 pos)
        {
            var h = SimplexNoise.noise(pos.x * _invNoiseSize, pos.y * _invNoiseSize, pos.z * _invNoiseSize);
            return (h < blackLevel) ? blackLevel : h;
        }
    }


    public static class BoxMuller
    {
        public const float TWO_PI = 2f * Mathf.PI;

        public static Vector2 Gaussian()
        {
            var u1 = UnityEngine.Random.value;
            var u2 = UnityEngine.Random.value;
            var logU1 = -2f * Mathf.Log(u1);
            var sqrt = (logU1 <= 0f) ? 0f : Mathf.Sqrt(logU1);
            var theta = TWO_PI * u2;
            var z0 = sqrt * Mathf.Cos(theta);
            var z1 = sqrt * Mathf.Sin(theta);
            return new Vector2(z0, z1);
        }
    }
}