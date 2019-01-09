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

        public int num = 100000;
        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 100f);

        public Cluster cluster;
        public PointClusterSimplexNoise.NoiseParam noiseParam;
        public PointClusterCurlNoise.CurlParam curlParam;

        PointClusterUniform uniform = new PointClusterUniform();
        PointClusterSimplexNoise noise = new PointClusterSimplexNoise();
        PointClusterCurlNoise curlNoise = new PointClusterCurlNoise();

        public PointViewer viewer;

        Dictionary<Cluster, IPointClusterGenerator<Vector3>> clusters;

        private void Awake()
        {
            clusters = new Dictionary<Cluster, IPointClusterGenerator<Vector3>>()
            {
                [Cluster.Uniform] = uniform,
                [Cluster.Noise] = noise,
                [Cluster.CurlNoise] = curlNoise
            };
        }

        private void Start()
        {
            if (viewer == null) viewer = GetComponent<PointViewer>();
            Generate();
        }

        [ContextMenu("Generate")]
        public void Generate()
        {
            noise.param = noiseParam;
            curlNoise.param = noiseParam;
            curlNoise.curlParam = curlParam;

            var points = clusters[cluster].Generate(num, bounds);
            viewer.SetPoint(points.ToArray());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}