Shader "Custom/Rainbow"
{
    Properties
    {
        _Speed("Flow Speed", Float) = 1
        [KeywordEnum(Horizontal, Vertical)] _FlowDirection("Flow Direction", Float) = 0
        _EmissionIntensity("Emission Intensity", Range(0, 20)) = 2
        _EmissionBoost("Emission Boost", Range(1, 10)) = 1
        _GradientScale("Gradient Scale", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend One One
        ZWrite Off
        Cull Back

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float _Speed;
                float _EmissionIntensity;
                float _EmissionBoost;
                float _GradientScale;
            CBUFFER_END

            float3 HSVToRGB(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float flowValue = IN.uv.x;
                #if defined(_FLOWDIRECTION_VERTICAL)
                    flowValue = IN.uv.y;
                #endif

                float wave = flowValue * _GradientScale + _Time.y * _Speed;
                float hue = frac(wave);
                float brightness = 0.85 + 0.15 * sin((IN.uv.y + _Time.y * _Speed) * 6.28318);

                float3 rgb = HSVToRGB(float3(hue, 1.0, brightness));
                rgb *= _EmissionIntensity * _EmissionBoost;

                return half4(rgb, 1.0);
            }
            ENDHLSL
        }
    }
}
