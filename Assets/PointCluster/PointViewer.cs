using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace PointCluster
{
    public class PointViewer : MonoBehaviour
    {
        public Material material;
        public bool gizmoEnable = true;
        public float gizmoCubeSize = 1f;

        protected Mesh mesh;


        public void SetPoint(Vector3[] points)
        {
            mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32,
                vertices = points
            };

            mesh.SetIndices(Enumerable.Range(0, points.Length).ToArray(), MeshTopology.Points, 0);
        }


        private void OnRenderObject()
        {
            material.SetPass(0);
            Graphics.DrawMeshNow(mesh, transform.localToWorldMatrix);
        }

        private void OnDrawGizmosSelected()
        {
            if ((mesh != null) && gizmoEnable)
            {
                var size = Vector3.one * gizmoCubeSize;

                mesh.vertices.ToList().ForEach(p =>
                {
                    Gizmos.DrawWireCube(p, size);
                });
            }
        }
    }
}