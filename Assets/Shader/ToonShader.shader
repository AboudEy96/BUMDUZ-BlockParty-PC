Shader "Custom/ToonShaderBrightOutlineSoft"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ShadeColor ("Shade Color", Color) = (0.6,0.6,0.6,1)
        _Threshold ("Shadow Threshold", Range(0,1)) = 0.5
        _LightIntensity ("Light Intensity", Range(0.5, 2)) = 1.2
        _OutlineColor ("Outline Color", Color) = (0,0,0,0.5) // صار نصف شفاف
        _OutlineWidth ("Outline Width", Range(0.0, 0.05)) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ToonRamp fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _ShadeColor;
        float _Threshold;
        float _LightIntensity;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);

            half diff = NdotL > _Threshold ? 1.0 : 0.6;

            half4 c;
            c.rgb = s.Albedo * (diff * atten * _LightIntensity);
            c.a = s.Alpha;
            return c;
        }
        ENDCG

        // Outline Pass
        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha // دعم الشفافية
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                float3 offset = norm * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex + float4(offset,0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
