using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PointCluster.Example
{
    public class PointClusterExample : MonoBehaviour
    {
        public enum Cluster
        {
            Uniform,
            Noise,
            CurlNoise
        }

        public bool generateAtStart;
        public int num = 100000;
        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 200f);

        public Cluster cluster = Cluster.CurlNoise;
        public PointClusterSimplexNoise.NoiseParam noiseParam;
        public PointClusterCurlNoise.CurlParam curlParam;

        PointClusterUniform uniform = new PointClusterUniform();
        PointClusterSimplexNoise noise = new PointClusterSimplexNoise();
        PointClusterCurlNoise curlNoise = new PointClusterCurlNoise();

        PointClusterCurlFlow curlFlow;

        public PointViewer viewer;

        Dictionary<Cluster, IPointClusterGenerator<Point>> clusters;

        private void Awake()
        {
            clusters = new Dictionary<Cluster, IPointClusterGenerator<Point>>()
            {
                [Cluster.Uniform] = uniform,
                [Cluster.Noise] = noise,
                [Cluster.CurlNoise] = curlNoise
            };
        }

        private void Start()
        {
            if (curlFlow == null) curlFlow = GetComponent<PointClusterCurlFlow>();
            if (generateAtStart)
            {
                Generate();
            }
        }


        [ContextMenu("Generate")]
        public List<Point> Generate()
        {
            noise.param = noiseParam;
            curlNoise.param = noiseParam;
            curlNoise.curlParam = curlParam;

            var points = clusters[cluster].Generate(num, bounds);

            if (viewer == null) viewer = GetComponent<PointViewer>();
            if (curlFlow != null)
            {
                curlFlow.Init(points);
                viewer?.SetBuffer(curlFlow.pointBuffer);
            }
            else
            {
                viewer?.SetPoint(points);
            }

            return points;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}