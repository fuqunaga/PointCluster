using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PointCluster
{
    /// <summary>
    /// Based on @nobnak's SuperCluster package
    /// </summary>
    [Serializable]
    public class PointClusterCurlFlow : MonoBehaviour
    {
        static class CSPARAM
        {
            public const string KernelFlow = "Flow";

            public const string PointBuffer = "_PointBuffer";
            public const string DT="_DT";
            public const string NoiseScaleInv = "_NoiseScaleInv";
            public const string CurlDelta = "_CurlDelta";
            public const string Speed = "_Speed";
        }

        public ComputeShader cs;
        public float noiseScale = 100f;
        public float curlDelta = 0.01f;
        public float speed = 1f;

        public ComputeBuffer pointBuffer { get; protected set; }
        


        public void Init(List<Point> points)
        {
            if (pointBuffer != null) pointBuffer.Release();
            pointBuffer = new ComputeBuffer(points.Count, Marshal.SizeOf(typeof(Point)));
            pointBuffer.SetData(points);
        }


        public void Update()
        {
            var kernel = cs.FindKernel(CSPARAM.KernelFlow);

            cs.SetFloat(CSPARAM.DT, Time.deltaTime);
            cs.SetFloat(CSPARAM.NoiseScaleInv, 1f / noiseScale);
            cs.SetFloat(CSPARAM.CurlDelta, curlDelta);
            cs.SetFloat(CSPARAM.Speed, speed);

            cs.SetBuffer(kernel, CSPARAM.PointBuffer, pointBuffer);
            cs.DispatchThreadNum(kernel, pointBuffer.count, 1, 1);
        }

        private void OnDestroy()
        {            
            if ( pointBuffer != null)
            {
                pointBuffer.Release();
                pointBuffer = null;
            }
        }
    }


    public static class ComputerShaderExtension
    {
        public static void DispatchThreadNum(this ComputeShader cs, int kernel, int threadNumX, int threadNumY, int threadNumZ)
        {
            uint x, y, z;
            cs.GetKernelThreadGroupSizes(kernel, out x, out y, out z);
            cs.Dispatch(kernel, Mathf.CeilToInt(threadNumX / x), Mathf.CeilToInt(threadNumY / y), Mathf.CeilToInt(threadNumZ / z));
        }
    }
}