Shader "Custom/OutlineGlow"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,1,0,1) // 기본 형광색 초록
        _OutlineThickness ("Outline Thickness", Range (0.0, 0.1)) = 0.02
        _GlowColor ("Glow Color", Color) = (0,1,0,1) // 형광색 초록
        _GlowIntensity ("Glow Intensity", Range (0.0, 2.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Lighting Off
        ZWrite Off
        ZTest Always
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            fixed4 _GlowColor;
            float _GlowIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Main texture color
                fixed4 color = tex2D(_MainTex, i.uv);

                // Outline effect
                float2 offset = float2(_OutlineThickness, 0);
                float2 outlineUV1 = i.uv + offset;
                float2 outlineUV2 = i.uv - offset;
                fixed4 outlineColor1 = tex2D(_MainTex, outlineUV1);
                fixed4 outlineColor2 = tex2D(_MainTex, outlineUV2);
                float outlineAlpha = step(0.01, outlineColor1.a) + step(0.01, outlineColor2.a);
                fixed4 outline = _OutlineColor * outlineAlpha;

                // Glow effect
                fixed4 glow = _GlowColor * _GlowIntensity * (1.0 - color.a);

                // Combine effects
                return color + outline + glow;
            }
            ENDCG
        }
    }
}
