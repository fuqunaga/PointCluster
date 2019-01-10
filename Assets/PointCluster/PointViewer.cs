using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace PointCluster
{
    public class PointViewer : MonoBehaviour
    {
        public bool enable = false;
        public Material material;
        public bool gizmoEnable = true;
        public float gizmoCubeSize = 1f;

        protected Mesh mesh;


        public void SetPoint(List<Point> points)
        {
            if (mesh != null) Destroy(mesh);

            mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32,
                vertices = points.Select(p => p.pos).ToArray(),
                normals = points.Select(p => p.rot * Vector3.forward).ToArray()
            };

            mesh.SetIndices(Enumerable.Range(0, points.Count).ToArray(), MeshTopology.Points, 0);
            mesh.RecalculateBounds();
        }


        private void OnRenderObject()
        {
            if (enable && ((Camera.current.cullingMask & (1<<gameObject.layer)) != 0))
            {
                material.SetPass(0);
                Graphics.DrawMeshNow(mesh, transform.localToWorldMatrix);
            }
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