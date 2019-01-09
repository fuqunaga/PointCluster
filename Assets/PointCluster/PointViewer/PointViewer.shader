Shader "Custom/PointViewer"
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

            struct g2f {
                float4 pos : SV_POSITION;
            };

            float4 vert (float4 v : POSITION) : POSITION
            {
                return v;
            }

            float _Size;

            [maxvertexcount(4)]
            void geom(point float4 p[1] : POSITION, inout TriangleStream<g2f> outStream)
            {
                float3 center = p[0];

                float3 up = float3(0, 1, 0);
                float3 look = _WorldSpaceCameraPos - center;
                look = normalize(look);

                float3 right = cross(up, look);
                up = cross(look, right);
                
                float3 r = right * _Size * 0.5;
                float3 u = up * _Size * 0.5;
                        
                float4 v[4];
                v[0] = float4(center + r - u, 1.0f);
                v[1] = float4(center + r + u, 1.0f);
                v[2] = float4(center - r - u, 1.0f);
                v[3] = float4(center - r + u, 1.0f);

                g2f Out;
                Out.pos = UnityObjectToClipPos(v[0]);
                outStream.Append(Out);

                Out.pos = UnityObjectToClipPos(v[1]);
                outStream.Append(Out);

                Out.pos = UnityObjectToClipPos(v[2]);
                outStream.Append(Out);

                Out.pos = UnityObjectToClipPos(v[3]);
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
