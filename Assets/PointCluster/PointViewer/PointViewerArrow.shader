Shader "PointCluster/PointViewerArrow"
{
    Properties
    {
        _Size ("Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct vsin
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            struct g2f {
                float4 pos : SV_POSITION;
            };

            vsin vert (vsin v)
            {
                return v;
            }

            float _Size;

            [maxvertexcount(5)]
            void geom(point vsin p[1], inout LineStream<g2f> outStream)
            {
                float3 pos = p[0].pos;
                float3 dir = p[0].normal;

                float3 up = float3(0,1,0);
                float3 right = normalize(cross(up, dir));
                        
                float4 tail   = UnityObjectToClipPos(float4(pos,1));
                float4 head   = UnityObjectToClipPos(float4(pos + dir * _Size,1));
                float4 wing_r = UnityObjectToClipPos(float4(pos + dir * _Size * 0.7 + right * _Size * 0.2,1));
                float4 wing_l = UnityObjectToClipPos(float4(pos + dir * _Size * 0.7 - right * _Size * 0.2,1));

                g2f Out;
                Out.pos = wing_r;
                outStream.Append(Out);

                Out.pos = head;
                outStream.Append(Out);

                Out.pos = tail;
                outStream.Append(Out);

                Out.pos = head;
                outStream.Append(Out);

                Out.pos = wing_l;
                outStream.Append(Out);
            }           

            fixed4 frag () : SV_Target
            {
                return fixed4(1,0,0,1);
            }
            ENDCG
        }
    }
}
