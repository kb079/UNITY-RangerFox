Shader "Dapasa/FX/ForceShield"
{
    Properties {
        _Color ("Color (RGB)", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelValue ("Intensity", Range(0.0, 5.0)) = 1
        _FresnelWidth ("Width", Range(0.0, 1.0)) = 1
        _Speed ("Speed", Range(0.0, 1.0)) = .5
    }
    SubShader {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }

        ZWrite Off
        Blend One One
        Cull Off

        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 uv2 : TEXCOORD1;
                float2 screenuv : TEXCOORD2;
                float depth : DEPTH;
            };

            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FresnelValue;
            float _FresnelWidth;
            float _Speed;
            sampler2D _CameraDepthNormalsTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Textura
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.y -= _Time.y * _Speed;

                // Interseccion
                o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) / 2;
                o.screenuv.y = 1 - o.screenuv.y;
                o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;

                // Fresnel
                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.uv2 = pow(1 - abs(dot(v.normal, viewDir)), 1/_FresnelWidth) * _FresnelValue;
                
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                fixed4 col = _Color * 20;
                fixed4 tex = tex2D(_MainTex, i.uv);

                float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);
                float diff = screenDepth - i.depth;
                float intersect = 0;

                if (diff > 0) {
                    intersect = 1 - smoothstep(0, _ProjectionParams.w, diff);
                    intersect = pow(intersect, 1 / _FresnelWidth) * _FresnelValue;
                }

                return (tex * max(i.uv2.x, intersect.x)) * col;
            }
            ENDCG
        }
    }
}
