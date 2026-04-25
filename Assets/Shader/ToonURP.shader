Shader "Custom/ToonURP_Outline"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ShadeColor ("Shade Color", Color) = (0.6,0.6,0.6,1)
        _Threshold ("Shadow Threshold", Range(0,1)) = 0.5
        _LightIntensity ("Light Intensity", Range(0.5, 2)) = 1.2
        _OutlineColor ("Outline Color", Color) = (0,0,0,0.5)
        _OutlineWidth ("Outline Width", Range(0.0, 0.05)) = 0.02
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        // ===== MAIN PASS =====
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _ShadeColor;
                float _Threshold;
                float _LightIntensity;
                float4 _MainTex_ST;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                Light light = GetMainLight();

                float3 normal = normalize(IN.normalWS);
                float NdotL = saturate(dot(normal, light.direction));

                // نفس منطقك القديم
                float diff = (NdotL > _Threshold) ? 1.0 : 0.6;

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

                float3 finalColor = tex.rgb * (diff * _LightIntensity + 0.2);

                return half4(finalColor, tex.a);
            }

            ENDHLSL
        }

        // ===== OUTLINE PASS =====
        Pass
        {
            Name "Outline"
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float _OutlineWidth;
            float4 _OutlineColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);

                posWS += normalWS * _OutlineWidth;

                OUT.positionHCS = TransformWorldToHClip(posWS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}