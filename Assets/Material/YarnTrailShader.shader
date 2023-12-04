Shader "Custom/YarnTrailShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _TexRecolor ("Texture Re-color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _TexRecolor;

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //float4 col = float4(i.uv, 0.0, 1.0);
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb = col.rgb * _TexRecolor;
                return col;
            }
            ENDCG
        }
    }
}
