Shader "Custom/StoreUnitFireStencilShader"
{
    Properties
    {
        _DissolveAlphaFade("Dissolve Alpha Fade", Range(0, 1)) = 0.5
        _DissolveTextureFade("Dissolve Texture Fade", Range(0, 1)) = 0.45
        _DissolveSize("Dissolve Size", Float) = 50
        [HDR]_DissolveColor("Dissolve Color", Color) = (11.98431, 6.4, 0, 0)
        [HDR]_FlameOverlayColor("Flame Overlay Color", Color) = (1.498039, 0.07076087, 0, 0)
        [NoScaleOffset]_BaseMap("Base Map", 2D) = "white" {}
        [NoScaleOffset]_OcclusionMap("Occlusion Map", 2D) = "white" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent+3"
            "ForceNoShadowCasting" = "True"
        }

        Stencil
        {
            Ref 0
            Comp NotEqual
            Pass keep
        }

        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

        // Render State
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZTest LEqual
        ZWrite Off
        // ColorMask: <None>


        HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        // Pragmas
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0
        #pragma multi_compile_fog
        #pragma multi_compile_instancing

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS 
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define SHADERPASS_FORWARD

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _DissolveAlphaFade;
        float _DissolveTextureFade;
        float _DissolveSize;
        float4 _DissolveColor;
        float4 _FlameOverlayColor;
        CBUFFER_END
        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
        TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
        SAMPLER(_SampleTexture2D_2D20F5B2_Sampler_3_Linear_Repeat);
        SAMPLER(_SampleTexture2D_97080DB0_Sampler_3_Linear_Repeat);
        SAMPLER(_SampleTexture2D_A8597535_Sampler_3_Linear_Repeat);
        SAMPLER(_SampleTexture2D_D009DFBF_Sampler_3_Linear_Repeat);
        SAMPLER(_SampleTexture2D_F1BA3C89_Sampler_3_Linear_Repeat);

        // Graph Functions

        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Modulo_float(float A, float B, out float Out)
        {
            Out = fmod(A, B);
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda
        {
            half4 uv0;
            float3 TimeParameters;
        };

        void SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(float4 Vector4_CB185251, TEXTURE2D_PARAM(Texture2D_4A46BA58, samplerTexture2D_4A46BA58), float4 Texture2D_4A46BA58_TexelSize, TEXTURE2D_PARAM(Texture2D_AD5955B2, samplerTexture2D_AD5955B2), float4 Texture2D_AD5955B2_TexelSize, Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda IN, out float4 Emission_2, out float Alpha_1)
        {
            float _Multiply_6DC02222_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_6DC02222_Out_2);
            float _Modulo_9B279C29_Out_2;
            Unity_Modulo_float(_Multiply_6DC02222_Out_2, 1, _Modulo_9B279C29_Out_2);
            float2 _Vector2_5214E4AB_Out_0 = float2(1, _Modulo_9B279C29_Out_2);
            float2 _TilingAndOffset_6E5111CD_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_5214E4AB_Out_0, _TilingAndOffset_6E5111CD_Out_3);
            float4 _SampleTexture2D_2D20F5B2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_6E5111CD_Out_3);
            float _SampleTexture2D_2D20F5B2_R_4 = _SampleTexture2D_2D20F5B2_RGBA_0.r;
            float _SampleTexture2D_2D20F5B2_G_5 = _SampleTexture2D_2D20F5B2_RGBA_0.g;
            float _SampleTexture2D_2D20F5B2_B_6 = _SampleTexture2D_2D20F5B2_RGBA_0.b;
            float _SampleTexture2D_2D20F5B2_A_7 = _SampleTexture2D_2D20F5B2_RGBA_0.a;
            float4 _Property_54AD2744_Out_0 = Vector4_CB185251;
            float4 _Multiply_834C87A1_Out_2;
            Unity_Multiply_float(_SampleTexture2D_2D20F5B2_RGBA_0, _Property_54AD2744_Out_0, _Multiply_834C87A1_Out_2);
            float _Multiply_CD63C6AA_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_CD63C6AA_Out_2);
            float _Modulo_7FC60EB4_Out_2;
            Unity_Modulo_float(_Multiply_CD63C6AA_Out_2, 1, _Modulo_7FC60EB4_Out_2);
            float2 _Vector2_76FB44D3_Out_0 = float2(1, _Modulo_7FC60EB4_Out_2);
            float2 _TilingAndOffset_6AB55BDD_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_76FB44D3_Out_0, _TilingAndOffset_6AB55BDD_Out_3);
            float4 _SampleTexture2D_97080DB0_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_AD5955B2, samplerTexture2D_AD5955B2, _TilingAndOffset_6AB55BDD_Out_3);
            float _SampleTexture2D_97080DB0_R_4 = _SampleTexture2D_97080DB0_RGBA_0.r;
            float _SampleTexture2D_97080DB0_G_5 = _SampleTexture2D_97080DB0_RGBA_0.g;
            float _SampleTexture2D_97080DB0_B_6 = _SampleTexture2D_97080DB0_RGBA_0.b;
            float _SampleTexture2D_97080DB0_A_7 = _SampleTexture2D_97080DB0_RGBA_0.a;
            float4 _Multiply_84D26FA_Out_2;
            Unity_Multiply_float(_Property_54AD2744_Out_0, _SampleTexture2D_97080DB0_RGBA_0, _Multiply_84D26FA_Out_2);
            float4 _Add_303DA3B2_Out_2;
            Unity_Add_float4(_Multiply_834C87A1_Out_2, _Multiply_84D26FA_Out_2, _Add_303DA3B2_Out_2);
            float4 _UV_E3C3F753_Out_0 = IN.uv0;
            float _Split_F069D01A_R_1 = _UV_E3C3F753_Out_0[0];
            float _Split_F069D01A_G_2 = _UV_E3C3F753_Out_0[1];
            float _Split_F069D01A_B_3 = _UV_E3C3F753_Out_0[2];
            float _Split_F069D01A_A_4 = _UV_E3C3F753_Out_0[3];
            float _Subtract_14E1ECCC_Out_2;
            Unity_Subtract_float(0.5, _Split_F069D01A_G_2, _Subtract_14E1ECCC_Out_2);
            float4 _Multiply_665F7DB_Out_2;
            Unity_Multiply_float(_Property_54AD2744_Out_0, (_Subtract_14E1ECCC_Out_2.xxxx), _Multiply_665F7DB_Out_2);
            float4 _Add_6C5267ED_Out_2;
            Unity_Add_float4(_Add_303DA3B2_Out_2, _Multiply_665F7DB_Out_2, _Add_6C5267ED_Out_2);
            float _Multiply_5F292A74_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_5F292A74_Out_2);
            float _Modulo_DA8EA6F6_Out_2;
            Unity_Modulo_float(_Multiply_5F292A74_Out_2, 1, _Modulo_DA8EA6F6_Out_2);
            float2 _Vector2_3AD7418B_Out_0 = float2(0, _Modulo_DA8EA6F6_Out_2);
            float2 _TilingAndOffset_EA17C7F8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3AD7418B_Out_0, _TilingAndOffset_EA17C7F8_Out_3);
            float4 _SampleTexture2D_A8597535_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_EA17C7F8_Out_3);
            float _SampleTexture2D_A8597535_R_4 = _SampleTexture2D_A8597535_RGBA_0.r;
            float _SampleTexture2D_A8597535_G_5 = _SampleTexture2D_A8597535_RGBA_0.g;
            float _SampleTexture2D_A8597535_B_6 = _SampleTexture2D_A8597535_RGBA_0.b;
            float _SampleTexture2D_A8597535_A_7 = _SampleTexture2D_A8597535_RGBA_0.a;
            float _Add_4ADE7687_Out_2;
            Unity_Add_float(_SampleTexture2D_97080DB0_A_7, _SampleTexture2D_A8597535_A_7, _Add_4ADE7687_Out_2);
            float _Multiply_45991EFE_Out_2;
            Unity_Multiply_float(_Subtract_14E1ECCC_Out_2, _Add_4ADE7687_Out_2, _Multiply_45991EFE_Out_2);
            Emission_2 = _Add_6C5267ED_Out_2;
            Alpha_1 = _Multiply_45991EFE_Out_2;
        }


        inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
        {
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
        }


        inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
        {
            return (1.0 - t) * a + (t * b);
        }


        inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);

            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0 = Unity_SimpleNoise_RandomValue_float(c0);
            float r1 = Unity_SimpleNoise_RandomValue_float(c1);
            float r2 = Unity_SimpleNoise_RandomValue_float(c2);
            float r3 = Unity_SimpleNoise_RandomValue_float(c3);

            float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
            float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
            float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
            return t;
        }

        void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
        {
            float t = 0.0;

            float freq = pow(2.0, float(0));
            float amp = pow(0.5, float(3 - 0));
            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3 - 1));
            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3 - 2));
            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

            Out = t;
        }

        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }

        struct Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add
        {
            float3 ObjectSpacePosition;
            half4 uv0;
        };

        void SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(float Vector1_309E00F1, float Vector1_7B45F4F5, float4 Vector4_50A7518F, float Vector1_26A8379B, TEXTURE2D_PARAM(Texture2D_91313B09, samplerTexture2D_91313B09), float4 Texture2D_91313B09_TexelSize, TEXTURE2D_PARAM(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2), float4 Texture2D_DCF7C2D2_TexelSize, Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add IN, out float4 Albedo_3, out float4 Emission_1, out float4 Occlusion_2, out float Alpha_4)
        {
            float4 _SampleTexture2D_D009DFBF_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_91313B09, samplerTexture2D_91313B09, IN.uv0.xy);
            float _SampleTexture2D_D009DFBF_R_4 = _SampleTexture2D_D009DFBF_RGBA_0.r;
            float _SampleTexture2D_D009DFBF_G_5 = _SampleTexture2D_D009DFBF_RGBA_0.g;
            float _SampleTexture2D_D009DFBF_B_6 = _SampleTexture2D_D009DFBF_RGBA_0.b;
            float _SampleTexture2D_D009DFBF_A_7 = _SampleTexture2D_D009DFBF_RGBA_0.a;
            float4 _Property_C5D94FA5_Out_0 = Vector4_50A7518F;
            float _Property_C5C7B8DF_Out_0 = Vector1_26A8379B;
            float _SimpleNoise_A8D64151_Out_2;
            Unity_SimpleNoise_float(IN.uv0.xy, _Property_C5C7B8DF_Out_0, _SimpleNoise_A8D64151_Out_2);
            float _Split_E348BCB_R_1 = IN.ObjectSpacePosition[0];
            float _Split_E348BCB_G_2 = IN.ObjectSpacePosition[1];
            float _Split_E348BCB_B_3 = IN.ObjectSpacePosition[2];
            float _Split_E348BCB_A_4 = 0;
            float _Add_42D8175A_Out_2;
            Unity_Add_float(_Split_E348BCB_G_2, 2, _Add_42D8175A_Out_2);
            float _Property_5C03BE49_Out_0 = Vector1_7B45F4F5;
            float _Remap_65038F06_Out_3;
            Unity_Remap_float(_Property_5C03BE49_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_65038F06_Out_3);
            float _Smoothstep_6D5F5C52_Out_3;
            Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_65038F06_Out_3, _Smoothstep_6D5F5C52_Out_3);
            float _Step_E9199F8A_Out_2;
            Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_6D5F5C52_Out_3, _Step_E9199F8A_Out_2);
            float _Subtract_39E16D76_Out_2;
            Unity_Subtract_float(1, _Step_E9199F8A_Out_2, _Subtract_39E16D76_Out_2);
            float4 _Multiply_6F4F600F_Out_2;
            Unity_Multiply_float(_Property_C5D94FA5_Out_0, (_Subtract_39E16D76_Out_2.xxxx), _Multiply_6F4F600F_Out_2);
            float4 _SampleTexture2D_F1BA3C89_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2, IN.uv0.xy);
            float _SampleTexture2D_F1BA3C89_R_4 = _SampleTexture2D_F1BA3C89_RGBA_0.r;
            float _SampleTexture2D_F1BA3C89_G_5 = _SampleTexture2D_F1BA3C89_RGBA_0.g;
            float _SampleTexture2D_F1BA3C89_B_6 = _SampleTexture2D_F1BA3C89_RGBA_0.b;
            float _SampleTexture2D_F1BA3C89_A_7 = _SampleTexture2D_F1BA3C89_RGBA_0.a;
            float _Property_7D032EAF_Out_0 = Vector1_309E00F1;
            float _Remap_9EB17AAF_Out_3;
            Unity_Remap_float(_Property_7D032EAF_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_9EB17AAF_Out_3);
            float _Smoothstep_19B47B70_Out_3;
            Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_9EB17AAF_Out_3, _Smoothstep_19B47B70_Out_3);
            float _Step_FDAF3C54_Out_2;
            Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_19B47B70_Out_3, _Step_FDAF3C54_Out_2);
            Albedo_3 = _SampleTexture2D_D009DFBF_RGBA_0;
            Emission_1 = _Multiply_6F4F600F_Out_2;
            Occlusion_2 = _SampleTexture2D_F1BA3C89_RGBA_0;
            Alpha_4 = _Step_FDAF3C54_Out_2;
        }

        void Unity_Maximum_float4(float4 A, float4 B, out float4 Out)
        {
            Out = max(A, B);
        }

        // Graph Vertex
        // GraphVertex: <None>

        // Graph Pixel
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float3 ObjectSpacePosition;
            float4 uv0;
            float3 TimeParameters;
        };

        struct SurfaceDescription
        {
            float3 Albedo;
            float3 Normal;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_56E21223_Out_0 = _FlameOverlayColor;
            Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda _FireStencilBaseSubshader_61F589E6;
            _FireStencilBaseSubshader_61F589E6.uv0 = IN.uv0;
            _FireStencilBaseSubshader_61F589E6.TimeParameters = IN.TimeParameters;
            float4 _FireStencilBaseSubshader_61F589E6_Emission_2;
            float _FireStencilBaseSubshader_61F589E6_Alpha_1;
            SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(_Property_56E21223_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _FireStencilBaseSubshader_61F589E6, _FireStencilBaseSubshader_61F589E6_Emission_2, _FireStencilBaseSubshader_61F589E6_Alpha_1);
            float _Property_D9CC7967_Out_0 = _DissolveAlphaFade;
            float _Property_FC243360_Out_0 = _DissolveTextureFade;
            float4 _Property_4122AEAD_Out_0 = _DissolveColor;
            float _Property_9FF396ED_Out_0 = _DissolveSize;
            Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add _SpawnSubshader_B9140A45;
            _SpawnSubshader_B9140A45.ObjectSpacePosition = IN.ObjectSpacePosition;
            _SpawnSubshader_B9140A45.uv0 = IN.uv0;
            float4 _SpawnSubshader_B9140A45_Albedo_3;
            float4 _SpawnSubshader_B9140A45_Emission_1;
            float4 _SpawnSubshader_B9140A45_Occlusion_2;
            float _SpawnSubshader_B9140A45_Alpha_4;
            SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(_Property_D9CC7967_Out_0, _Property_FC243360_Out_0, _Property_4122AEAD_Out_0, _Property_9FF396ED_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _SpawnSubshader_B9140A45, _SpawnSubshader_B9140A45_Albedo_3, _SpawnSubshader_B9140A45_Emission_1, _SpawnSubshader_B9140A45_Occlusion_2, _SpawnSubshader_B9140A45_Alpha_4);
            float4 _Maximum_E62506BA_Out_2;
            Unity_Maximum_float4(_FireStencilBaseSubshader_61F589E6_Emission_2, _SpawnSubshader_B9140A45_Emission_1, _Maximum_E62506BA_Out_2);
            float _Multiply_48CAA7FB_Out_2;
            Unity_Multiply_float(_FireStencilBaseSubshader_61F589E6_Alpha_1, _SpawnSubshader_B9140A45_Alpha_4, _Multiply_48CAA7FB_Out_2);
            surface.Albedo = IsGammaSpace() ? float3(0.2735849, 0.2735849, 0.2735849) : SRGBToLinear(float3(0.2735849, 0.2735849, 0.2735849));
            surface.Normal = IN.TangentSpaceNormal;
            surface.Emission = (_Maximum_E62506BA_Out_2.xyz);
            surface.Metallic = 0;
            surface.Smoothness = 0.5;
            surface.Occlusion = 1;
            surface.Alpha = _Multiply_48CAA7FB_Out_2;
            surface.AlphaClipThreshold = 0;
            return surface;
        }

        // --------------------------------------------------
        // Structs and Packing

        // Generated Type: Attributes
        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };

        // Generated Type: Varyings
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        // Generated Type: PackedVaryings
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
            #endif
            #if !defined(LIGHTMAP_ON)
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            float3 interp00 : TEXCOORD0;
            float3 interp01 : TEXCOORD1;
            float4 interp02 : TEXCOORD2;
            float4 interp03 : TEXCOORD3;
            float3 interp04 : TEXCOORD4;
            float2 interp05 : TEXCOORD5;
            float3 interp06 : TEXCOORD6;
            float4 interp07 : TEXCOORD7;
            float4 interp08 : TEXCOORD8;
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        // Packed Type: Varyings
        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output = (PackedVaryings)0;
            output.positionCS = input.positionCS;
            output.interp00.xyz = input.positionWS;
            output.interp01.xyz = input.normalWS;
            output.interp02.xyzw = input.tangentWS;
            output.interp03.xyzw = input.texCoord0;
            output.interp04.xyz = input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp05.xy = input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp06.xyz = input.sh;
            #endif
            output.interp07.xyzw = input.fogFactorAndVertexLight;
            output.interp08.xyzw = input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        // Unpacked Type: Varyings
        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output = (Varyings)0;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp00.xyz;
            output.normalWS = input.interp01.xyz;
            output.tangentWS = input.interp02.xyzw;
            output.texCoord0 = input.interp03.xyzw;
            output.viewDirectionWS = input.interp04.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp05.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp06.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp07.xyzw;
            output.shadowCoord = input.interp08.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        // --------------------------------------------------
        // Build Graph Inputs

        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }


        // --------------------------------------------------
        // Main

        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

        ENDHLSL
    }

    Pass
    {
        Name "ShadowCaster"
        Tags
        {
            "LightMode" = "ShadowCaster"
        }

            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Off
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_TEXCOORD0
            #define SHADERPASS_SHADOWCASTER

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _DissolveAlphaFade;
            float _DissolveTextureFade;
            float _DissolveSize;
            float4 _DissolveColor;
            float4 _FlameOverlayColor;
            CBUFFER_END
            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
            TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
            SAMPLER(_SampleTexture2D_2D20F5B2_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_97080DB0_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_A8597535_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_D009DFBF_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_F1BA3C89_Sampler_3_Linear_Repeat);

            // Graph Functions

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Modulo_float(float A, float B, out float Out)
            {
                Out = fmod(A, B);
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            struct Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda
            {
                half4 uv0;
                float3 TimeParameters;
            };

            void SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(float4 Vector4_CB185251, TEXTURE2D_PARAM(Texture2D_4A46BA58, samplerTexture2D_4A46BA58), float4 Texture2D_4A46BA58_TexelSize, TEXTURE2D_PARAM(Texture2D_AD5955B2, samplerTexture2D_AD5955B2), float4 Texture2D_AD5955B2_TexelSize, Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda IN, out float4 Emission_2, out float Alpha_1)
            {
                float _Multiply_6DC02222_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_6DC02222_Out_2);
                float _Modulo_9B279C29_Out_2;
                Unity_Modulo_float(_Multiply_6DC02222_Out_2, 1, _Modulo_9B279C29_Out_2);
                float2 _Vector2_5214E4AB_Out_0 = float2(1, _Modulo_9B279C29_Out_2);
                float2 _TilingAndOffset_6E5111CD_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_5214E4AB_Out_0, _TilingAndOffset_6E5111CD_Out_3);
                float4 _SampleTexture2D_2D20F5B2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_6E5111CD_Out_3);
                float _SampleTexture2D_2D20F5B2_R_4 = _SampleTexture2D_2D20F5B2_RGBA_0.r;
                float _SampleTexture2D_2D20F5B2_G_5 = _SampleTexture2D_2D20F5B2_RGBA_0.g;
                float _SampleTexture2D_2D20F5B2_B_6 = _SampleTexture2D_2D20F5B2_RGBA_0.b;
                float _SampleTexture2D_2D20F5B2_A_7 = _SampleTexture2D_2D20F5B2_RGBA_0.a;
                float4 _Property_54AD2744_Out_0 = Vector4_CB185251;
                float4 _Multiply_834C87A1_Out_2;
                Unity_Multiply_float(_SampleTexture2D_2D20F5B2_RGBA_0, _Property_54AD2744_Out_0, _Multiply_834C87A1_Out_2);
                float _Multiply_CD63C6AA_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_CD63C6AA_Out_2);
                float _Modulo_7FC60EB4_Out_2;
                Unity_Modulo_float(_Multiply_CD63C6AA_Out_2, 1, _Modulo_7FC60EB4_Out_2);
                float2 _Vector2_76FB44D3_Out_0 = float2(1, _Modulo_7FC60EB4_Out_2);
                float2 _TilingAndOffset_6AB55BDD_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_76FB44D3_Out_0, _TilingAndOffset_6AB55BDD_Out_3);
                float4 _SampleTexture2D_97080DB0_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_AD5955B2, samplerTexture2D_AD5955B2, _TilingAndOffset_6AB55BDD_Out_3);
                float _SampleTexture2D_97080DB0_R_4 = _SampleTexture2D_97080DB0_RGBA_0.r;
                float _SampleTexture2D_97080DB0_G_5 = _SampleTexture2D_97080DB0_RGBA_0.g;
                float _SampleTexture2D_97080DB0_B_6 = _SampleTexture2D_97080DB0_RGBA_0.b;
                float _SampleTexture2D_97080DB0_A_7 = _SampleTexture2D_97080DB0_RGBA_0.a;
                float4 _Multiply_84D26FA_Out_2;
                Unity_Multiply_float(_Property_54AD2744_Out_0, _SampleTexture2D_97080DB0_RGBA_0, _Multiply_84D26FA_Out_2);
                float4 _Add_303DA3B2_Out_2;
                Unity_Add_float4(_Multiply_834C87A1_Out_2, _Multiply_84D26FA_Out_2, _Add_303DA3B2_Out_2);
                float4 _UV_E3C3F753_Out_0 = IN.uv0;
                float _Split_F069D01A_R_1 = _UV_E3C3F753_Out_0[0];
                float _Split_F069D01A_G_2 = _UV_E3C3F753_Out_0[1];
                float _Split_F069D01A_B_3 = _UV_E3C3F753_Out_0[2];
                float _Split_F069D01A_A_4 = _UV_E3C3F753_Out_0[3];
                float _Subtract_14E1ECCC_Out_2;
                Unity_Subtract_float(0.5, _Split_F069D01A_G_2, _Subtract_14E1ECCC_Out_2);
                float4 _Multiply_665F7DB_Out_2;
                Unity_Multiply_float(_Property_54AD2744_Out_0, (_Subtract_14E1ECCC_Out_2.xxxx), _Multiply_665F7DB_Out_2);
                float4 _Add_6C5267ED_Out_2;
                Unity_Add_float4(_Add_303DA3B2_Out_2, _Multiply_665F7DB_Out_2, _Add_6C5267ED_Out_2);
                float _Multiply_5F292A74_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_5F292A74_Out_2);
                float _Modulo_DA8EA6F6_Out_2;
                Unity_Modulo_float(_Multiply_5F292A74_Out_2, 1, _Modulo_DA8EA6F6_Out_2);
                float2 _Vector2_3AD7418B_Out_0 = float2(0, _Modulo_DA8EA6F6_Out_2);
                float2 _TilingAndOffset_EA17C7F8_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3AD7418B_Out_0, _TilingAndOffset_EA17C7F8_Out_3);
                float4 _SampleTexture2D_A8597535_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_EA17C7F8_Out_3);
                float _SampleTexture2D_A8597535_R_4 = _SampleTexture2D_A8597535_RGBA_0.r;
                float _SampleTexture2D_A8597535_G_5 = _SampleTexture2D_A8597535_RGBA_0.g;
                float _SampleTexture2D_A8597535_B_6 = _SampleTexture2D_A8597535_RGBA_0.b;
                float _SampleTexture2D_A8597535_A_7 = _SampleTexture2D_A8597535_RGBA_0.a;
                float _Add_4ADE7687_Out_2;
                Unity_Add_float(_SampleTexture2D_97080DB0_A_7, _SampleTexture2D_A8597535_A_7, _Add_4ADE7687_Out_2);
                float _Multiply_45991EFE_Out_2;
                Unity_Multiply_float(_Subtract_14E1ECCC_Out_2, _Add_4ADE7687_Out_2, _Multiply_45991EFE_Out_2);
                Emission_2 = _Add_6C5267ED_Out_2;
                Alpha_1 = _Multiply_45991EFE_Out_2;
            }


            inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }


            inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
            {
                return (1.0 - t) * a + (t * b);
            }


            inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                Out = t;
            }

            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }

            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }

            struct Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add
            {
                float3 ObjectSpacePosition;
                half4 uv0;
            };

            void SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(float Vector1_309E00F1, float Vector1_7B45F4F5, float4 Vector4_50A7518F, float Vector1_26A8379B, TEXTURE2D_PARAM(Texture2D_91313B09, samplerTexture2D_91313B09), float4 Texture2D_91313B09_TexelSize, TEXTURE2D_PARAM(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2), float4 Texture2D_DCF7C2D2_TexelSize, Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add IN, out float4 Albedo_3, out float4 Emission_1, out float4 Occlusion_2, out float Alpha_4)
            {
                float4 _SampleTexture2D_D009DFBF_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_91313B09, samplerTexture2D_91313B09, IN.uv0.xy);
                float _SampleTexture2D_D009DFBF_R_4 = _SampleTexture2D_D009DFBF_RGBA_0.r;
                float _SampleTexture2D_D009DFBF_G_5 = _SampleTexture2D_D009DFBF_RGBA_0.g;
                float _SampleTexture2D_D009DFBF_B_6 = _SampleTexture2D_D009DFBF_RGBA_0.b;
                float _SampleTexture2D_D009DFBF_A_7 = _SampleTexture2D_D009DFBF_RGBA_0.a;
                float4 _Property_C5D94FA5_Out_0 = Vector4_50A7518F;
                float _Property_C5C7B8DF_Out_0 = Vector1_26A8379B;
                float _SimpleNoise_A8D64151_Out_2;
                Unity_SimpleNoise_float(IN.uv0.xy, _Property_C5C7B8DF_Out_0, _SimpleNoise_A8D64151_Out_2);
                float _Split_E348BCB_R_1 = IN.ObjectSpacePosition[0];
                float _Split_E348BCB_G_2 = IN.ObjectSpacePosition[1];
                float _Split_E348BCB_B_3 = IN.ObjectSpacePosition[2];
                float _Split_E348BCB_A_4 = 0;
                float _Add_42D8175A_Out_2;
                Unity_Add_float(_Split_E348BCB_G_2, 2, _Add_42D8175A_Out_2);
                float _Property_5C03BE49_Out_0 = Vector1_7B45F4F5;
                float _Remap_65038F06_Out_3;
                Unity_Remap_float(_Property_5C03BE49_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_65038F06_Out_3);
                float _Smoothstep_6D5F5C52_Out_3;
                Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_65038F06_Out_3, _Smoothstep_6D5F5C52_Out_3);
                float _Step_E9199F8A_Out_2;
                Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_6D5F5C52_Out_3, _Step_E9199F8A_Out_2);
                float _Subtract_39E16D76_Out_2;
                Unity_Subtract_float(1, _Step_E9199F8A_Out_2, _Subtract_39E16D76_Out_2);
                float4 _Multiply_6F4F600F_Out_2;
                Unity_Multiply_float(_Property_C5D94FA5_Out_0, (_Subtract_39E16D76_Out_2.xxxx), _Multiply_6F4F600F_Out_2);
                float4 _SampleTexture2D_F1BA3C89_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2, IN.uv0.xy);
                float _SampleTexture2D_F1BA3C89_R_4 = _SampleTexture2D_F1BA3C89_RGBA_0.r;
                float _SampleTexture2D_F1BA3C89_G_5 = _SampleTexture2D_F1BA3C89_RGBA_0.g;
                float _SampleTexture2D_F1BA3C89_B_6 = _SampleTexture2D_F1BA3C89_RGBA_0.b;
                float _SampleTexture2D_F1BA3C89_A_7 = _SampleTexture2D_F1BA3C89_RGBA_0.a;
                float _Property_7D032EAF_Out_0 = Vector1_309E00F1;
                float _Remap_9EB17AAF_Out_3;
                Unity_Remap_float(_Property_7D032EAF_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_9EB17AAF_Out_3);
                float _Smoothstep_19B47B70_Out_3;
                Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_9EB17AAF_Out_3, _Smoothstep_19B47B70_Out_3);
                float _Step_FDAF3C54_Out_2;
                Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_19B47B70_Out_3, _Step_FDAF3C54_Out_2);
                Albedo_3 = _SampleTexture2D_D009DFBF_RGBA_0;
                Emission_1 = _Multiply_6F4F600F_Out_2;
                Occlusion_2 = _SampleTexture2D_F1BA3C89_RGBA_0;
                Alpha_4 = _Step_FDAF3C54_Out_2;
            }

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 ObjectSpacePosition;
                float4 uv0;
                float3 TimeParameters;
            };

            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_56E21223_Out_0 = _FlameOverlayColor;
                Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda _FireStencilBaseSubshader_61F589E6;
                _FireStencilBaseSubshader_61F589E6.uv0 = IN.uv0;
                _FireStencilBaseSubshader_61F589E6.TimeParameters = IN.TimeParameters;
                float4 _FireStencilBaseSubshader_61F589E6_Emission_2;
                float _FireStencilBaseSubshader_61F589E6_Alpha_1;
                SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(_Property_56E21223_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _FireStencilBaseSubshader_61F589E6, _FireStencilBaseSubshader_61F589E6_Emission_2, _FireStencilBaseSubshader_61F589E6_Alpha_1);
                float _Property_D9CC7967_Out_0 = _DissolveAlphaFade;
                float _Property_FC243360_Out_0 = _DissolveTextureFade;
                float4 _Property_4122AEAD_Out_0 = _DissolveColor;
                float _Property_9FF396ED_Out_0 = _DissolveSize;
                Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add _SpawnSubshader_B9140A45;
                _SpawnSubshader_B9140A45.ObjectSpacePosition = IN.ObjectSpacePosition;
                _SpawnSubshader_B9140A45.uv0 = IN.uv0;
                float4 _SpawnSubshader_B9140A45_Albedo_3;
                float4 _SpawnSubshader_B9140A45_Emission_1;
                float4 _SpawnSubshader_B9140A45_Occlusion_2;
                float _SpawnSubshader_B9140A45_Alpha_4;
                SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(_Property_D9CC7967_Out_0, _Property_FC243360_Out_0, _Property_4122AEAD_Out_0, _Property_9FF396ED_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _SpawnSubshader_B9140A45, _SpawnSubshader_B9140A45_Albedo_3, _SpawnSubshader_B9140A45_Emission_1, _SpawnSubshader_B9140A45_Occlusion_2, _SpawnSubshader_B9140A45_Alpha_4);
                float _Multiply_48CAA7FB_Out_2;
                Unity_Multiply_float(_FireStencilBaseSubshader_61F589E6_Alpha_1, _SpawnSubshader_B9140A45_Alpha_4, _Multiply_48CAA7FB_Out_2);
                surface.Alpha = _Multiply_48CAA7FB_Out_2;
                surface.AlphaClipThreshold = 0;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.texCoord0 = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
                output.uv0 = input.texCoord0;
                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

                // Render State
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                Cull Off
                ZTest LEqual
                ZWrite On
                ColorMask 0


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_POSITION_WS 
                #define VARYINGS_NEED_TEXCOORD0
                #define SHADERPASS_DEPTHONLY

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float _DissolveAlphaFade;
                float _DissolveTextureFade;
                float _DissolveSize;
                float4 _DissolveColor;
                float4 _FlameOverlayColor;
                CBUFFER_END
                TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                SAMPLER(_SampleTexture2D_2D20F5B2_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_97080DB0_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_A8597535_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_D009DFBF_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_F1BA3C89_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }

                void Unity_Modulo_float(float A, float B, out float Out)
                {
                    Out = fmod(A, B);
                }

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }

                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }

                void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A + B;
                }

                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }

                void Unity_Add_float(float A, float B, out float Out)
                {
                    Out = A + B;
                }

                struct Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda
                {
                    half4 uv0;
                    float3 TimeParameters;
                };

                void SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(float4 Vector4_CB185251, TEXTURE2D_PARAM(Texture2D_4A46BA58, samplerTexture2D_4A46BA58), float4 Texture2D_4A46BA58_TexelSize, TEXTURE2D_PARAM(Texture2D_AD5955B2, samplerTexture2D_AD5955B2), float4 Texture2D_AD5955B2_TexelSize, Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda IN, out float4 Emission_2, out float Alpha_1)
                {
                    float _Multiply_6DC02222_Out_2;
                    Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_6DC02222_Out_2);
                    float _Modulo_9B279C29_Out_2;
                    Unity_Modulo_float(_Multiply_6DC02222_Out_2, 1, _Modulo_9B279C29_Out_2);
                    float2 _Vector2_5214E4AB_Out_0 = float2(1, _Modulo_9B279C29_Out_2);
                    float2 _TilingAndOffset_6E5111CD_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_5214E4AB_Out_0, _TilingAndOffset_6E5111CD_Out_3);
                    float4 _SampleTexture2D_2D20F5B2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_6E5111CD_Out_3);
                    float _SampleTexture2D_2D20F5B2_R_4 = _SampleTexture2D_2D20F5B2_RGBA_0.r;
                    float _SampleTexture2D_2D20F5B2_G_5 = _SampleTexture2D_2D20F5B2_RGBA_0.g;
                    float _SampleTexture2D_2D20F5B2_B_6 = _SampleTexture2D_2D20F5B2_RGBA_0.b;
                    float _SampleTexture2D_2D20F5B2_A_7 = _SampleTexture2D_2D20F5B2_RGBA_0.a;
                    float4 _Property_54AD2744_Out_0 = Vector4_CB185251;
                    float4 _Multiply_834C87A1_Out_2;
                    Unity_Multiply_float(_SampleTexture2D_2D20F5B2_RGBA_0, _Property_54AD2744_Out_0, _Multiply_834C87A1_Out_2);
                    float _Multiply_CD63C6AA_Out_2;
                    Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_CD63C6AA_Out_2);
                    float _Modulo_7FC60EB4_Out_2;
                    Unity_Modulo_float(_Multiply_CD63C6AA_Out_2, 1, _Modulo_7FC60EB4_Out_2);
                    float2 _Vector2_76FB44D3_Out_0 = float2(1, _Modulo_7FC60EB4_Out_2);
                    float2 _TilingAndOffset_6AB55BDD_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_76FB44D3_Out_0, _TilingAndOffset_6AB55BDD_Out_3);
                    float4 _SampleTexture2D_97080DB0_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_AD5955B2, samplerTexture2D_AD5955B2, _TilingAndOffset_6AB55BDD_Out_3);
                    float _SampleTexture2D_97080DB0_R_4 = _SampleTexture2D_97080DB0_RGBA_0.r;
                    float _SampleTexture2D_97080DB0_G_5 = _SampleTexture2D_97080DB0_RGBA_0.g;
                    float _SampleTexture2D_97080DB0_B_6 = _SampleTexture2D_97080DB0_RGBA_0.b;
                    float _SampleTexture2D_97080DB0_A_7 = _SampleTexture2D_97080DB0_RGBA_0.a;
                    float4 _Multiply_84D26FA_Out_2;
                    Unity_Multiply_float(_Property_54AD2744_Out_0, _SampleTexture2D_97080DB0_RGBA_0, _Multiply_84D26FA_Out_2);
                    float4 _Add_303DA3B2_Out_2;
                    Unity_Add_float4(_Multiply_834C87A1_Out_2, _Multiply_84D26FA_Out_2, _Add_303DA3B2_Out_2);
                    float4 _UV_E3C3F753_Out_0 = IN.uv0;
                    float _Split_F069D01A_R_1 = _UV_E3C3F753_Out_0[0];
                    float _Split_F069D01A_G_2 = _UV_E3C3F753_Out_0[1];
                    float _Split_F069D01A_B_3 = _UV_E3C3F753_Out_0[2];
                    float _Split_F069D01A_A_4 = _UV_E3C3F753_Out_0[3];
                    float _Subtract_14E1ECCC_Out_2;
                    Unity_Subtract_float(0.5, _Split_F069D01A_G_2, _Subtract_14E1ECCC_Out_2);
                    float4 _Multiply_665F7DB_Out_2;
                    Unity_Multiply_float(_Property_54AD2744_Out_0, (_Subtract_14E1ECCC_Out_2.xxxx), _Multiply_665F7DB_Out_2);
                    float4 _Add_6C5267ED_Out_2;
                    Unity_Add_float4(_Add_303DA3B2_Out_2, _Multiply_665F7DB_Out_2, _Add_6C5267ED_Out_2);
                    float _Multiply_5F292A74_Out_2;
                    Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_5F292A74_Out_2);
                    float _Modulo_DA8EA6F6_Out_2;
                    Unity_Modulo_float(_Multiply_5F292A74_Out_2, 1, _Modulo_DA8EA6F6_Out_2);
                    float2 _Vector2_3AD7418B_Out_0 = float2(0, _Modulo_DA8EA6F6_Out_2);
                    float2 _TilingAndOffset_EA17C7F8_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3AD7418B_Out_0, _TilingAndOffset_EA17C7F8_Out_3);
                    float4 _SampleTexture2D_A8597535_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_EA17C7F8_Out_3);
                    float _SampleTexture2D_A8597535_R_4 = _SampleTexture2D_A8597535_RGBA_0.r;
                    float _SampleTexture2D_A8597535_G_5 = _SampleTexture2D_A8597535_RGBA_0.g;
                    float _SampleTexture2D_A8597535_B_6 = _SampleTexture2D_A8597535_RGBA_0.b;
                    float _SampleTexture2D_A8597535_A_7 = _SampleTexture2D_A8597535_RGBA_0.a;
                    float _Add_4ADE7687_Out_2;
                    Unity_Add_float(_SampleTexture2D_97080DB0_A_7, _SampleTexture2D_A8597535_A_7, _Add_4ADE7687_Out_2);
                    float _Multiply_45991EFE_Out_2;
                    Unity_Multiply_float(_Subtract_14E1ECCC_Out_2, _Add_4ADE7687_Out_2, _Multiply_45991EFE_Out_2);
                    Emission_2 = _Add_6C5267ED_Out_2;
                    Alpha_1 = _Multiply_45991EFE_Out_2;
                }


                inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
                {
                    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
                }


                inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
                {
                    return (1.0 - t) * a + (t * b);
                }


                inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
                {
                    float2 i = floor(uv);
                    float2 f = frac(uv);
                    f = f * f * (3.0 - 2.0 * f);

                    uv = abs(frac(uv) - 0.5);
                    float2 c0 = i + float2(0.0, 0.0);
                    float2 c1 = i + float2(1.0, 0.0);
                    float2 c2 = i + float2(0.0, 1.0);
                    float2 c3 = i + float2(1.0, 1.0);
                    float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                    float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                    float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                    float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                    float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                    float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                    float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                    return t;
                }

                void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
                {
                    float t = 0.0;

                    float freq = pow(2.0, float(0));
                    float amp = pow(0.5, float(3 - 0));
                    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                    freq = pow(2.0, float(1));
                    amp = pow(0.5, float(3 - 1));
                    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                    freq = pow(2.0, float(2));
                    amp = pow(0.5, float(3 - 2));
                    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                    Out = t;
                }

                void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                {
                    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                }

                void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                {
                    Out = smoothstep(Edge1, Edge2, In);
                }

                void Unity_Step_float(float Edge, float In, out float Out)
                {
                    Out = step(Edge, In);
                }

                struct Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add
                {
                    float3 ObjectSpacePosition;
                    half4 uv0;
                };

                void SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(float Vector1_309E00F1, float Vector1_7B45F4F5, float4 Vector4_50A7518F, float Vector1_26A8379B, TEXTURE2D_PARAM(Texture2D_91313B09, samplerTexture2D_91313B09), float4 Texture2D_91313B09_TexelSize, TEXTURE2D_PARAM(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2), float4 Texture2D_DCF7C2D2_TexelSize, Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add IN, out float4 Albedo_3, out float4 Emission_1, out float4 Occlusion_2, out float Alpha_4)
                {
                    float4 _SampleTexture2D_D009DFBF_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_91313B09, samplerTexture2D_91313B09, IN.uv0.xy);
                    float _SampleTexture2D_D009DFBF_R_4 = _SampleTexture2D_D009DFBF_RGBA_0.r;
                    float _SampleTexture2D_D009DFBF_G_5 = _SampleTexture2D_D009DFBF_RGBA_0.g;
                    float _SampleTexture2D_D009DFBF_B_6 = _SampleTexture2D_D009DFBF_RGBA_0.b;
                    float _SampleTexture2D_D009DFBF_A_7 = _SampleTexture2D_D009DFBF_RGBA_0.a;
                    float4 _Property_C5D94FA5_Out_0 = Vector4_50A7518F;
                    float _Property_C5C7B8DF_Out_0 = Vector1_26A8379B;
                    float _SimpleNoise_A8D64151_Out_2;
                    Unity_SimpleNoise_float(IN.uv0.xy, _Property_C5C7B8DF_Out_0, _SimpleNoise_A8D64151_Out_2);
                    float _Split_E348BCB_R_1 = IN.ObjectSpacePosition[0];
                    float _Split_E348BCB_G_2 = IN.ObjectSpacePosition[1];
                    float _Split_E348BCB_B_3 = IN.ObjectSpacePosition[2];
                    float _Split_E348BCB_A_4 = 0;
                    float _Add_42D8175A_Out_2;
                    Unity_Add_float(_Split_E348BCB_G_2, 2, _Add_42D8175A_Out_2);
                    float _Property_5C03BE49_Out_0 = Vector1_7B45F4F5;
                    float _Remap_65038F06_Out_3;
                    Unity_Remap_float(_Property_5C03BE49_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_65038F06_Out_3);
                    float _Smoothstep_6D5F5C52_Out_3;
                    Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_65038F06_Out_3, _Smoothstep_6D5F5C52_Out_3);
                    float _Step_E9199F8A_Out_2;
                    Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_6D5F5C52_Out_3, _Step_E9199F8A_Out_2);
                    float _Subtract_39E16D76_Out_2;
                    Unity_Subtract_float(1, _Step_E9199F8A_Out_2, _Subtract_39E16D76_Out_2);
                    float4 _Multiply_6F4F600F_Out_2;
                    Unity_Multiply_float(_Property_C5D94FA5_Out_0, (_Subtract_39E16D76_Out_2.xxxx), _Multiply_6F4F600F_Out_2);
                    float4 _SampleTexture2D_F1BA3C89_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2, IN.uv0.xy);
                    float _SampleTexture2D_F1BA3C89_R_4 = _SampleTexture2D_F1BA3C89_RGBA_0.r;
                    float _SampleTexture2D_F1BA3C89_G_5 = _SampleTexture2D_F1BA3C89_RGBA_0.g;
                    float _SampleTexture2D_F1BA3C89_B_6 = _SampleTexture2D_F1BA3C89_RGBA_0.b;
                    float _SampleTexture2D_F1BA3C89_A_7 = _SampleTexture2D_F1BA3C89_RGBA_0.a;
                    float _Property_7D032EAF_Out_0 = Vector1_309E00F1;
                    float _Remap_9EB17AAF_Out_3;
                    Unity_Remap_float(_Property_7D032EAF_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_9EB17AAF_Out_3);
                    float _Smoothstep_19B47B70_Out_3;
                    Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_9EB17AAF_Out_3, _Smoothstep_19B47B70_Out_3);
                    float _Step_FDAF3C54_Out_2;
                    Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_19B47B70_Out_3, _Step_FDAF3C54_Out_2);
                    Albedo_3 = _SampleTexture2D_D009DFBF_RGBA_0;
                    Emission_1 = _Multiply_6F4F600F_Out_2;
                    Occlusion_2 = _SampleTexture2D_F1BA3C89_RGBA_0;
                    Alpha_4 = _Step_FDAF3C54_Out_2;
                }

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 ObjectSpacePosition;
                    float4 uv0;
                    float3 TimeParameters;
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _Property_56E21223_Out_0 = _FlameOverlayColor;
                    Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda _FireStencilBaseSubshader_61F589E6;
                    _FireStencilBaseSubshader_61F589E6.uv0 = IN.uv0;
                    _FireStencilBaseSubshader_61F589E6.TimeParameters = IN.TimeParameters;
                    float4 _FireStencilBaseSubshader_61F589E6_Emission_2;
                    float _FireStencilBaseSubshader_61F589E6_Alpha_1;
                    SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(_Property_56E21223_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _FireStencilBaseSubshader_61F589E6, _FireStencilBaseSubshader_61F589E6_Emission_2, _FireStencilBaseSubshader_61F589E6_Alpha_1);
                    float _Property_D9CC7967_Out_0 = _DissolveAlphaFade;
                    float _Property_FC243360_Out_0 = _DissolveTextureFade;
                    float4 _Property_4122AEAD_Out_0 = _DissolveColor;
                    float _Property_9FF396ED_Out_0 = _DissolveSize;
                    Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add _SpawnSubshader_B9140A45;
                    _SpawnSubshader_B9140A45.ObjectSpacePosition = IN.ObjectSpacePosition;
                    _SpawnSubshader_B9140A45.uv0 = IN.uv0;
                    float4 _SpawnSubshader_B9140A45_Albedo_3;
                    float4 _SpawnSubshader_B9140A45_Emission_1;
                    float4 _SpawnSubshader_B9140A45_Occlusion_2;
                    float _SpawnSubshader_B9140A45_Alpha_4;
                    SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(_Property_D9CC7967_Out_0, _Property_FC243360_Out_0, _Property_4122AEAD_Out_0, _Property_9FF396ED_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _SpawnSubshader_B9140A45, _SpawnSubshader_B9140A45_Albedo_3, _SpawnSubshader_B9140A45_Emission_1, _SpawnSubshader_B9140A45_Occlusion_2, _SpawnSubshader_B9140A45_Alpha_4);
                    float _Multiply_48CAA7FB_Out_2;
                    Unity_Multiply_float(_FireStencilBaseSubshader_61F589E6_Alpha_1, _SpawnSubshader_B9140A45_Alpha_4, _Multiply_48CAA7FB_Out_2);
                    surface.Alpha = _Multiply_48CAA7FB_Out_2;
                    surface.AlphaClipThreshold = 0;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    float3 interp00 : TEXCOORD0;
                    float4 interp01 : TEXCOORD1;
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    output.interp00.xyz = input.positionWS;
                    output.interp01.xyzw = input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp00.xyz;
                    output.texCoord0 = input.interp01.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                    output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
                    output.uv0 = input.texCoord0;
                    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "Meta"
                Tags
                {
                    "LightMode" = "Meta"
                }

                    // Render State
                    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                    Cull Off
                    ZTest LEqual
                    ZWrite On
                    // ColorMask: <None>


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0

                    // Keywords
                    #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                    // GraphKeywords: <None>

                    // Defines
                    #define _SURFACE_TYPE_TRANSPARENT 1
                    #define _NORMAL_DROPOFF_TS 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define ATTRIBUTES_NEED_TEXCOORD1
                    #define ATTRIBUTES_NEED_TEXCOORD2
                    #define VARYINGS_NEED_POSITION_WS 
                    #define VARYINGS_NEED_TEXCOORD0
                    #define SHADERPASS_META

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
                    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float _DissolveAlphaFade;
                    float _DissolveTextureFade;
                    float _DissolveSize;
                    float4 _DissolveColor;
                    float4 _FlameOverlayColor;
                    CBUFFER_END
                    TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                    TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                    SAMPLER(_SampleTexture2D_2D20F5B2_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_97080DB0_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_A8597535_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_D009DFBF_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_F1BA3C89_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Modulo_float(float A, float B, out float Out)
                    {
                        Out = fmod(A, B);
                    }

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                    {
                        Out = A + B;
                    }

                    void Unity_Subtract_float(float A, float B, out float Out)
                    {
                        Out = A - B;
                    }

                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }

                    struct Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda
                    {
                        half4 uv0;
                        float3 TimeParameters;
                    };

                    void SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(float4 Vector4_CB185251, TEXTURE2D_PARAM(Texture2D_4A46BA58, samplerTexture2D_4A46BA58), float4 Texture2D_4A46BA58_TexelSize, TEXTURE2D_PARAM(Texture2D_AD5955B2, samplerTexture2D_AD5955B2), float4 Texture2D_AD5955B2_TexelSize, Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda IN, out float4 Emission_2, out float Alpha_1)
                    {
                        float _Multiply_6DC02222_Out_2;
                        Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_6DC02222_Out_2);
                        float _Modulo_9B279C29_Out_2;
                        Unity_Modulo_float(_Multiply_6DC02222_Out_2, 1, _Modulo_9B279C29_Out_2);
                        float2 _Vector2_5214E4AB_Out_0 = float2(1, _Modulo_9B279C29_Out_2);
                        float2 _TilingAndOffset_6E5111CD_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_5214E4AB_Out_0, _TilingAndOffset_6E5111CD_Out_3);
                        float4 _SampleTexture2D_2D20F5B2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_6E5111CD_Out_3);
                        float _SampleTexture2D_2D20F5B2_R_4 = _SampleTexture2D_2D20F5B2_RGBA_0.r;
                        float _SampleTexture2D_2D20F5B2_G_5 = _SampleTexture2D_2D20F5B2_RGBA_0.g;
                        float _SampleTexture2D_2D20F5B2_B_6 = _SampleTexture2D_2D20F5B2_RGBA_0.b;
                        float _SampleTexture2D_2D20F5B2_A_7 = _SampleTexture2D_2D20F5B2_RGBA_0.a;
                        float4 _Property_54AD2744_Out_0 = Vector4_CB185251;
                        float4 _Multiply_834C87A1_Out_2;
                        Unity_Multiply_float(_SampleTexture2D_2D20F5B2_RGBA_0, _Property_54AD2744_Out_0, _Multiply_834C87A1_Out_2);
                        float _Multiply_CD63C6AA_Out_2;
                        Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_CD63C6AA_Out_2);
                        float _Modulo_7FC60EB4_Out_2;
                        Unity_Modulo_float(_Multiply_CD63C6AA_Out_2, 1, _Modulo_7FC60EB4_Out_2);
                        float2 _Vector2_76FB44D3_Out_0 = float2(1, _Modulo_7FC60EB4_Out_2);
                        float2 _TilingAndOffset_6AB55BDD_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_76FB44D3_Out_0, _TilingAndOffset_6AB55BDD_Out_3);
                        float4 _SampleTexture2D_97080DB0_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_AD5955B2, samplerTexture2D_AD5955B2, _TilingAndOffset_6AB55BDD_Out_3);
                        float _SampleTexture2D_97080DB0_R_4 = _SampleTexture2D_97080DB0_RGBA_0.r;
                        float _SampleTexture2D_97080DB0_G_5 = _SampleTexture2D_97080DB0_RGBA_0.g;
                        float _SampleTexture2D_97080DB0_B_6 = _SampleTexture2D_97080DB0_RGBA_0.b;
                        float _SampleTexture2D_97080DB0_A_7 = _SampleTexture2D_97080DB0_RGBA_0.a;
                        float4 _Multiply_84D26FA_Out_2;
                        Unity_Multiply_float(_Property_54AD2744_Out_0, _SampleTexture2D_97080DB0_RGBA_0, _Multiply_84D26FA_Out_2);
                        float4 _Add_303DA3B2_Out_2;
                        Unity_Add_float4(_Multiply_834C87A1_Out_2, _Multiply_84D26FA_Out_2, _Add_303DA3B2_Out_2);
                        float4 _UV_E3C3F753_Out_0 = IN.uv0;
                        float _Split_F069D01A_R_1 = _UV_E3C3F753_Out_0[0];
                        float _Split_F069D01A_G_2 = _UV_E3C3F753_Out_0[1];
                        float _Split_F069D01A_B_3 = _UV_E3C3F753_Out_0[2];
                        float _Split_F069D01A_A_4 = _UV_E3C3F753_Out_0[3];
                        float _Subtract_14E1ECCC_Out_2;
                        Unity_Subtract_float(0.5, _Split_F069D01A_G_2, _Subtract_14E1ECCC_Out_2);
                        float4 _Multiply_665F7DB_Out_2;
                        Unity_Multiply_float(_Property_54AD2744_Out_0, (_Subtract_14E1ECCC_Out_2.xxxx), _Multiply_665F7DB_Out_2);
                        float4 _Add_6C5267ED_Out_2;
                        Unity_Add_float4(_Add_303DA3B2_Out_2, _Multiply_665F7DB_Out_2, _Add_6C5267ED_Out_2);
                        float _Multiply_5F292A74_Out_2;
                        Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_5F292A74_Out_2);
                        float _Modulo_DA8EA6F6_Out_2;
                        Unity_Modulo_float(_Multiply_5F292A74_Out_2, 1, _Modulo_DA8EA6F6_Out_2);
                        float2 _Vector2_3AD7418B_Out_0 = float2(0, _Modulo_DA8EA6F6_Out_2);
                        float2 _TilingAndOffset_EA17C7F8_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3AD7418B_Out_0, _TilingAndOffset_EA17C7F8_Out_3);
                        float4 _SampleTexture2D_A8597535_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_EA17C7F8_Out_3);
                        float _SampleTexture2D_A8597535_R_4 = _SampleTexture2D_A8597535_RGBA_0.r;
                        float _SampleTexture2D_A8597535_G_5 = _SampleTexture2D_A8597535_RGBA_0.g;
                        float _SampleTexture2D_A8597535_B_6 = _SampleTexture2D_A8597535_RGBA_0.b;
                        float _SampleTexture2D_A8597535_A_7 = _SampleTexture2D_A8597535_RGBA_0.a;
                        float _Add_4ADE7687_Out_2;
                        Unity_Add_float(_SampleTexture2D_97080DB0_A_7, _SampleTexture2D_A8597535_A_7, _Add_4ADE7687_Out_2);
                        float _Multiply_45991EFE_Out_2;
                        Unity_Multiply_float(_Subtract_14E1ECCC_Out_2, _Add_4ADE7687_Out_2, _Multiply_45991EFE_Out_2);
                        Emission_2 = _Add_6C5267ED_Out_2;
                        Alpha_1 = _Multiply_45991EFE_Out_2;
                    }


                    inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
                    {
                        return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
                    }


                    inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
                    {
                        return (1.0 - t) * a + (t * b);
                    }


                    inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
                    {
                        float2 i = floor(uv);
                        float2 f = frac(uv);
                        f = f * f * (3.0 - 2.0 * f);

                        uv = abs(frac(uv) - 0.5);
                        float2 c0 = i + float2(0.0, 0.0);
                        float2 c1 = i + float2(1.0, 0.0);
                        float2 c2 = i + float2(0.0, 1.0);
                        float2 c3 = i + float2(1.0, 1.0);
                        float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                        float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                        float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                        float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                        float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                        float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                        float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                        return t;
                    }

                    void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
                    {
                        float t = 0.0;

                        float freq = pow(2.0, float(0));
                        float amp = pow(0.5, float(3 - 0));
                        t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                        freq = pow(2.0, float(1));
                        amp = pow(0.5, float(3 - 1));
                        t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                        freq = pow(2.0, float(2));
                        amp = pow(0.5, float(3 - 2));
                        t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                        Out = t;
                    }

                    void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                    {
                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                    }

                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                    {
                        Out = smoothstep(Edge1, Edge2, In);
                    }

                    void Unity_Step_float(float Edge, float In, out float Out)
                    {
                        Out = step(Edge, In);
                    }

                    struct Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add
                    {
                        float3 ObjectSpacePosition;
                        half4 uv0;
                    };

                    void SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(float Vector1_309E00F1, float Vector1_7B45F4F5, float4 Vector4_50A7518F, float Vector1_26A8379B, TEXTURE2D_PARAM(Texture2D_91313B09, samplerTexture2D_91313B09), float4 Texture2D_91313B09_TexelSize, TEXTURE2D_PARAM(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2), float4 Texture2D_DCF7C2D2_TexelSize, Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add IN, out float4 Albedo_3, out float4 Emission_1, out float4 Occlusion_2, out float Alpha_4)
                    {
                        float4 _SampleTexture2D_D009DFBF_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_91313B09, samplerTexture2D_91313B09, IN.uv0.xy);
                        float _SampleTexture2D_D009DFBF_R_4 = _SampleTexture2D_D009DFBF_RGBA_0.r;
                        float _SampleTexture2D_D009DFBF_G_5 = _SampleTexture2D_D009DFBF_RGBA_0.g;
                        float _SampleTexture2D_D009DFBF_B_6 = _SampleTexture2D_D009DFBF_RGBA_0.b;
                        float _SampleTexture2D_D009DFBF_A_7 = _SampleTexture2D_D009DFBF_RGBA_0.a;
                        float4 _Property_C5D94FA5_Out_0 = Vector4_50A7518F;
                        float _Property_C5C7B8DF_Out_0 = Vector1_26A8379B;
                        float _SimpleNoise_A8D64151_Out_2;
                        Unity_SimpleNoise_float(IN.uv0.xy, _Property_C5C7B8DF_Out_0, _SimpleNoise_A8D64151_Out_2);
                        float _Split_E348BCB_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_E348BCB_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_E348BCB_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_E348BCB_A_4 = 0;
                        float _Add_42D8175A_Out_2;
                        Unity_Add_float(_Split_E348BCB_G_2, 2, _Add_42D8175A_Out_2);
                        float _Property_5C03BE49_Out_0 = Vector1_7B45F4F5;
                        float _Remap_65038F06_Out_3;
                        Unity_Remap_float(_Property_5C03BE49_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_65038F06_Out_3);
                        float _Smoothstep_6D5F5C52_Out_3;
                        Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_65038F06_Out_3, _Smoothstep_6D5F5C52_Out_3);
                        float _Step_E9199F8A_Out_2;
                        Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_6D5F5C52_Out_3, _Step_E9199F8A_Out_2);
                        float _Subtract_39E16D76_Out_2;
                        Unity_Subtract_float(1, _Step_E9199F8A_Out_2, _Subtract_39E16D76_Out_2);
                        float4 _Multiply_6F4F600F_Out_2;
                        Unity_Multiply_float(_Property_C5D94FA5_Out_0, (_Subtract_39E16D76_Out_2.xxxx), _Multiply_6F4F600F_Out_2);
                        float4 _SampleTexture2D_F1BA3C89_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2, IN.uv0.xy);
                        float _SampleTexture2D_F1BA3C89_R_4 = _SampleTexture2D_F1BA3C89_RGBA_0.r;
                        float _SampleTexture2D_F1BA3C89_G_5 = _SampleTexture2D_F1BA3C89_RGBA_0.g;
                        float _SampleTexture2D_F1BA3C89_B_6 = _SampleTexture2D_F1BA3C89_RGBA_0.b;
                        float _SampleTexture2D_F1BA3C89_A_7 = _SampleTexture2D_F1BA3C89_RGBA_0.a;
                        float _Property_7D032EAF_Out_0 = Vector1_309E00F1;
                        float _Remap_9EB17AAF_Out_3;
                        Unity_Remap_float(_Property_7D032EAF_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_9EB17AAF_Out_3);
                        float _Smoothstep_19B47B70_Out_3;
                        Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_9EB17AAF_Out_3, _Smoothstep_19B47B70_Out_3);
                        float _Step_FDAF3C54_Out_2;
                        Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_19B47B70_Out_3, _Step_FDAF3C54_Out_2);
                        Albedo_3 = _SampleTexture2D_D009DFBF_RGBA_0;
                        Emission_1 = _Multiply_6F4F600F_Out_2;
                        Occlusion_2 = _SampleTexture2D_F1BA3C89_RGBA_0;
                        Alpha_4 = _Step_FDAF3C54_Out_2;
                    }

                    void Unity_Maximum_float4(float4 A, float4 B, out float4 Out)
                    {
                        Out = max(A, B);
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal;
                        float3 ObjectSpacePosition;
                        float4 uv0;
                        float3 TimeParameters;
                    };

                    struct SurfaceDescription
                    {
                        float3 Albedo;
                        float3 Emission;
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _Property_56E21223_Out_0 = _FlameOverlayColor;
                        Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda _FireStencilBaseSubshader_61F589E6;
                        _FireStencilBaseSubshader_61F589E6.uv0 = IN.uv0;
                        _FireStencilBaseSubshader_61F589E6.TimeParameters = IN.TimeParameters;
                        float4 _FireStencilBaseSubshader_61F589E6_Emission_2;
                        float _FireStencilBaseSubshader_61F589E6_Alpha_1;
                        SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(_Property_56E21223_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _FireStencilBaseSubshader_61F589E6, _FireStencilBaseSubshader_61F589E6_Emission_2, _FireStencilBaseSubshader_61F589E6_Alpha_1);
                        float _Property_D9CC7967_Out_0 = _DissolveAlphaFade;
                        float _Property_FC243360_Out_0 = _DissolveTextureFade;
                        float4 _Property_4122AEAD_Out_0 = _DissolveColor;
                        float _Property_9FF396ED_Out_0 = _DissolveSize;
                        Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add _SpawnSubshader_B9140A45;
                        _SpawnSubshader_B9140A45.ObjectSpacePosition = IN.ObjectSpacePosition;
                        _SpawnSubshader_B9140A45.uv0 = IN.uv0;
                        float4 _SpawnSubshader_B9140A45_Albedo_3;
                        float4 _SpawnSubshader_B9140A45_Emission_1;
                        float4 _SpawnSubshader_B9140A45_Occlusion_2;
                        float _SpawnSubshader_B9140A45_Alpha_4;
                        SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(_Property_D9CC7967_Out_0, _Property_FC243360_Out_0, _Property_4122AEAD_Out_0, _Property_9FF396ED_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _SpawnSubshader_B9140A45, _SpawnSubshader_B9140A45_Albedo_3, _SpawnSubshader_B9140A45_Emission_1, _SpawnSubshader_B9140A45_Occlusion_2, _SpawnSubshader_B9140A45_Alpha_4);
                        float4 _Maximum_E62506BA_Out_2;
                        Unity_Maximum_float4(_FireStencilBaseSubshader_61F589E6_Emission_2, _SpawnSubshader_B9140A45_Emission_1, _Maximum_E62506BA_Out_2);
                        float _Multiply_48CAA7FB_Out_2;
                        Unity_Multiply_float(_FireStencilBaseSubshader_61F589E6_Alpha_1, _SpawnSubshader_B9140A45_Alpha_4, _Multiply_48CAA7FB_Out_2);
                        surface.Albedo = IsGammaSpace() ? float3(0.2735849, 0.2735849, 0.2735849) : SRGBToLinear(float3(0.2735849, 0.2735849, 0.2735849));
                        surface.Emission = (_Maximum_E62506BA_Out_2.xyz);
                        surface.Alpha = _Multiply_48CAA7FB_Out_2;
                        surface.AlphaClipThreshold = 0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        float4 uv1 : TEXCOORD1;
                        float4 uv2 : TEXCOORD2;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        float3 positionWS;
                        float4 texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        float3 interp00 : TEXCOORD0;
                        float4 interp01 : TEXCOORD1;
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        output.interp00.xyz = input.positionWS;
                        output.interp01.xyzw = input.texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        output.positionWS = input.interp00.xyz;
                        output.texCoord0 = input.interp01.xyzw;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                        output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
                        output.uv0 = input.texCoord0;
                        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                    ENDHLSL
                }

                Pass
                {
                        // Name: <None>
                        Tags
                        {
                            "LightMode" = "Universal2D"
                        }

                        // Render State
                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                        Cull Off
                        ZTest LEqual
                        ZWrite Off
                        // ColorMask: <None>


                        HLSLPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        // Pragmas
                        #pragma prefer_hlslcc gles
                        #pragma exclude_renderers d3d11_9x
                        #pragma target 2.0
                        #pragma multi_compile_instancing

                        // Keywords
                        // PassKeywords: <None>
                        // GraphKeywords: <None>

                        // Defines
                        #define _SURFACE_TYPE_TRANSPARENT 1
                        #define _NORMAL_DROPOFF_TS 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define ATTRIBUTES_NEED_TEXCOORD0
                        #define VARYINGS_NEED_POSITION_WS 
                        #define VARYINGS_NEED_TEXCOORD0
                        #define SHADERPASS_2D

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float _DissolveAlphaFade;
                        float _DissolveTextureFade;
                        float _DissolveSize;
                        float4 _DissolveColor;
                        float4 _FlameOverlayColor;
                        CBUFFER_END
                        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                        TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                        SAMPLER(_SampleTexture2D_2D20F5B2_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_97080DB0_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_A8597535_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_D009DFBF_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_F1BA3C89_Sampler_3_Linear_Repeat);

                        // Graph Functions

                        void Unity_Multiply_float(float A, float B, out float Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Modulo_float(float A, float B, out float Out)
                        {
                            Out = fmod(A, B);
                        }

                        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                        {
                            Out = UV * Tiling + Offset;
                        }

                        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                        {
                            Out = A + B;
                        }

                        void Unity_Subtract_float(float A, float B, out float Out)
                        {
                            Out = A - B;
                        }

                        void Unity_Add_float(float A, float B, out float Out)
                        {
                            Out = A + B;
                        }

                        struct Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda
                        {
                            half4 uv0;
                            float3 TimeParameters;
                        };

                        void SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(float4 Vector4_CB185251, TEXTURE2D_PARAM(Texture2D_4A46BA58, samplerTexture2D_4A46BA58), float4 Texture2D_4A46BA58_TexelSize, TEXTURE2D_PARAM(Texture2D_AD5955B2, samplerTexture2D_AD5955B2), float4 Texture2D_AD5955B2_TexelSize, Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda IN, out float4 Emission_2, out float Alpha_1)
                        {
                            float _Multiply_6DC02222_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_6DC02222_Out_2);
                            float _Modulo_9B279C29_Out_2;
                            Unity_Modulo_float(_Multiply_6DC02222_Out_2, 1, _Modulo_9B279C29_Out_2);
                            float2 _Vector2_5214E4AB_Out_0 = float2(1, _Modulo_9B279C29_Out_2);
                            float2 _TilingAndOffset_6E5111CD_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_5214E4AB_Out_0, _TilingAndOffset_6E5111CD_Out_3);
                            float4 _SampleTexture2D_2D20F5B2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_6E5111CD_Out_3);
                            float _SampleTexture2D_2D20F5B2_R_4 = _SampleTexture2D_2D20F5B2_RGBA_0.r;
                            float _SampleTexture2D_2D20F5B2_G_5 = _SampleTexture2D_2D20F5B2_RGBA_0.g;
                            float _SampleTexture2D_2D20F5B2_B_6 = _SampleTexture2D_2D20F5B2_RGBA_0.b;
                            float _SampleTexture2D_2D20F5B2_A_7 = _SampleTexture2D_2D20F5B2_RGBA_0.a;
                            float4 _Property_54AD2744_Out_0 = Vector4_CB185251;
                            float4 _Multiply_834C87A1_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_2D20F5B2_RGBA_0, _Property_54AD2744_Out_0, _Multiply_834C87A1_Out_2);
                            float _Multiply_CD63C6AA_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_CD63C6AA_Out_2);
                            float _Modulo_7FC60EB4_Out_2;
                            Unity_Modulo_float(_Multiply_CD63C6AA_Out_2, 1, _Modulo_7FC60EB4_Out_2);
                            float2 _Vector2_76FB44D3_Out_0 = float2(1, _Modulo_7FC60EB4_Out_2);
                            float2 _TilingAndOffset_6AB55BDD_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_76FB44D3_Out_0, _TilingAndOffset_6AB55BDD_Out_3);
                            float4 _SampleTexture2D_97080DB0_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_AD5955B2, samplerTexture2D_AD5955B2, _TilingAndOffset_6AB55BDD_Out_3);
                            float _SampleTexture2D_97080DB0_R_4 = _SampleTexture2D_97080DB0_RGBA_0.r;
                            float _SampleTexture2D_97080DB0_G_5 = _SampleTexture2D_97080DB0_RGBA_0.g;
                            float _SampleTexture2D_97080DB0_B_6 = _SampleTexture2D_97080DB0_RGBA_0.b;
                            float _SampleTexture2D_97080DB0_A_7 = _SampleTexture2D_97080DB0_RGBA_0.a;
                            float4 _Multiply_84D26FA_Out_2;
                            Unity_Multiply_float(_Property_54AD2744_Out_0, _SampleTexture2D_97080DB0_RGBA_0, _Multiply_84D26FA_Out_2);
                            float4 _Add_303DA3B2_Out_2;
                            Unity_Add_float4(_Multiply_834C87A1_Out_2, _Multiply_84D26FA_Out_2, _Add_303DA3B2_Out_2);
                            float4 _UV_E3C3F753_Out_0 = IN.uv0;
                            float _Split_F069D01A_R_1 = _UV_E3C3F753_Out_0[0];
                            float _Split_F069D01A_G_2 = _UV_E3C3F753_Out_0[1];
                            float _Split_F069D01A_B_3 = _UV_E3C3F753_Out_0[2];
                            float _Split_F069D01A_A_4 = _UV_E3C3F753_Out_0[3];
                            float _Subtract_14E1ECCC_Out_2;
                            Unity_Subtract_float(0.5, _Split_F069D01A_G_2, _Subtract_14E1ECCC_Out_2);
                            float4 _Multiply_665F7DB_Out_2;
                            Unity_Multiply_float(_Property_54AD2744_Out_0, (_Subtract_14E1ECCC_Out_2.xxxx), _Multiply_665F7DB_Out_2);
                            float4 _Add_6C5267ED_Out_2;
                            Unity_Add_float4(_Add_303DA3B2_Out_2, _Multiply_665F7DB_Out_2, _Add_6C5267ED_Out_2);
                            float _Multiply_5F292A74_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_5F292A74_Out_2);
                            float _Modulo_DA8EA6F6_Out_2;
                            Unity_Modulo_float(_Multiply_5F292A74_Out_2, 1, _Modulo_DA8EA6F6_Out_2);
                            float2 _Vector2_3AD7418B_Out_0 = float2(0, _Modulo_DA8EA6F6_Out_2);
                            float2 _TilingAndOffset_EA17C7F8_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_3AD7418B_Out_0, _TilingAndOffset_EA17C7F8_Out_3);
                            float4 _SampleTexture2D_A8597535_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_4A46BA58, samplerTexture2D_4A46BA58, _TilingAndOffset_EA17C7F8_Out_3);
                            float _SampleTexture2D_A8597535_R_4 = _SampleTexture2D_A8597535_RGBA_0.r;
                            float _SampleTexture2D_A8597535_G_5 = _SampleTexture2D_A8597535_RGBA_0.g;
                            float _SampleTexture2D_A8597535_B_6 = _SampleTexture2D_A8597535_RGBA_0.b;
                            float _SampleTexture2D_A8597535_A_7 = _SampleTexture2D_A8597535_RGBA_0.a;
                            float _Add_4ADE7687_Out_2;
                            Unity_Add_float(_SampleTexture2D_97080DB0_A_7, _SampleTexture2D_A8597535_A_7, _Add_4ADE7687_Out_2);
                            float _Multiply_45991EFE_Out_2;
                            Unity_Multiply_float(_Subtract_14E1ECCC_Out_2, _Add_4ADE7687_Out_2, _Multiply_45991EFE_Out_2);
                            Emission_2 = _Add_6C5267ED_Out_2;
                            Alpha_1 = _Multiply_45991EFE_Out_2;
                        }


                        inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
                        {
                            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
                        }


                        inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
                        {
                            return (1.0 - t) * a + (t * b);
                        }


                        inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
                        {
                            float2 i = floor(uv);
                            float2 f = frac(uv);
                            f = f * f * (3.0 - 2.0 * f);

                            uv = abs(frac(uv) - 0.5);
                            float2 c0 = i + float2(0.0, 0.0);
                            float2 c1 = i + float2(1.0, 0.0);
                            float2 c2 = i + float2(0.0, 1.0);
                            float2 c3 = i + float2(1.0, 1.0);
                            float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                            float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                            float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                            float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                            float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                            float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                            float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                            return t;
                        }

                        void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
                        {
                            float t = 0.0;

                            float freq = pow(2.0, float(0));
                            float amp = pow(0.5, float(3 - 0));
                            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                            freq = pow(2.0, float(1));
                            amp = pow(0.5, float(3 - 1));
                            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                            freq = pow(2.0, float(2));
                            amp = pow(0.5, float(3 - 2));
                            t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                            Out = t;
                        }

                        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                        {
                            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                        }

                        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                        {
                            Out = smoothstep(Edge1, Edge2, In);
                        }

                        void Unity_Step_float(float Edge, float In, out float Out)
                        {
                            Out = step(Edge, In);
                        }

                        struct Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add
                        {
                            float3 ObjectSpacePosition;
                            half4 uv0;
                        };

                        void SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(float Vector1_309E00F1, float Vector1_7B45F4F5, float4 Vector4_50A7518F, float Vector1_26A8379B, TEXTURE2D_PARAM(Texture2D_91313B09, samplerTexture2D_91313B09), float4 Texture2D_91313B09_TexelSize, TEXTURE2D_PARAM(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2), float4 Texture2D_DCF7C2D2_TexelSize, Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add IN, out float4 Albedo_3, out float4 Emission_1, out float4 Occlusion_2, out float Alpha_4)
                        {
                            float4 _SampleTexture2D_D009DFBF_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_91313B09, samplerTexture2D_91313B09, IN.uv0.xy);
                            float _SampleTexture2D_D009DFBF_R_4 = _SampleTexture2D_D009DFBF_RGBA_0.r;
                            float _SampleTexture2D_D009DFBF_G_5 = _SampleTexture2D_D009DFBF_RGBA_0.g;
                            float _SampleTexture2D_D009DFBF_B_6 = _SampleTexture2D_D009DFBF_RGBA_0.b;
                            float _SampleTexture2D_D009DFBF_A_7 = _SampleTexture2D_D009DFBF_RGBA_0.a;
                            float4 _Property_C5D94FA5_Out_0 = Vector4_50A7518F;
                            float _Property_C5C7B8DF_Out_0 = Vector1_26A8379B;
                            float _SimpleNoise_A8D64151_Out_2;
                            Unity_SimpleNoise_float(IN.uv0.xy, _Property_C5C7B8DF_Out_0, _SimpleNoise_A8D64151_Out_2);
                            float _Split_E348BCB_R_1 = IN.ObjectSpacePosition[0];
                            float _Split_E348BCB_G_2 = IN.ObjectSpacePosition[1];
                            float _Split_E348BCB_B_3 = IN.ObjectSpacePosition[2];
                            float _Split_E348BCB_A_4 = 0;
                            float _Add_42D8175A_Out_2;
                            Unity_Add_float(_Split_E348BCB_G_2, 2, _Add_42D8175A_Out_2);
                            float _Property_5C03BE49_Out_0 = Vector1_7B45F4F5;
                            float _Remap_65038F06_Out_3;
                            Unity_Remap_float(_Property_5C03BE49_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_65038F06_Out_3);
                            float _Smoothstep_6D5F5C52_Out_3;
                            Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_65038F06_Out_3, _Smoothstep_6D5F5C52_Out_3);
                            float _Step_E9199F8A_Out_2;
                            Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_6D5F5C52_Out_3, _Step_E9199F8A_Out_2);
                            float _Subtract_39E16D76_Out_2;
                            Unity_Subtract_float(1, _Step_E9199F8A_Out_2, _Subtract_39E16D76_Out_2);
                            float4 _Multiply_6F4F600F_Out_2;
                            Unity_Multiply_float(_Property_C5D94FA5_Out_0, (_Subtract_39E16D76_Out_2.xxxx), _Multiply_6F4F600F_Out_2);
                            float4 _SampleTexture2D_F1BA3C89_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_DCF7C2D2, samplerTexture2D_DCF7C2D2, IN.uv0.xy);
                            float _SampleTexture2D_F1BA3C89_R_4 = _SampleTexture2D_F1BA3C89_RGBA_0.r;
                            float _SampleTexture2D_F1BA3C89_G_5 = _SampleTexture2D_F1BA3C89_RGBA_0.g;
                            float _SampleTexture2D_F1BA3C89_B_6 = _SampleTexture2D_F1BA3C89_RGBA_0.b;
                            float _SampleTexture2D_F1BA3C89_A_7 = _SampleTexture2D_F1BA3C89_RGBA_0.a;
                            float _Property_7D032EAF_Out_0 = Vector1_309E00F1;
                            float _Remap_9EB17AAF_Out_3;
                            Unity_Remap_float(_Property_7D032EAF_Out_0, float2 (0, 1), float2 (-0.1, 2), _Remap_9EB17AAF_Out_3);
                            float _Smoothstep_19B47B70_Out_3;
                            Unity_Smoothstep_float(_Split_E348BCB_G_2, _Add_42D8175A_Out_2, _Remap_9EB17AAF_Out_3, _Smoothstep_19B47B70_Out_3);
                            float _Step_FDAF3C54_Out_2;
                            Unity_Step_float(_SimpleNoise_A8D64151_Out_2, _Smoothstep_19B47B70_Out_3, _Step_FDAF3C54_Out_2);
                            Albedo_3 = _SampleTexture2D_D009DFBF_RGBA_0;
                            Emission_1 = _Multiply_6F4F600F_Out_2;
                            Occlusion_2 = _SampleTexture2D_F1BA3C89_RGBA_0;
                            Alpha_4 = _Step_FDAF3C54_Out_2;
                        }

                        // Graph Vertex
                        // GraphVertex: <None>

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            float3 TangentSpaceNormal;
                            float3 ObjectSpacePosition;
                            float4 uv0;
                            float3 TimeParameters;
                        };

                        struct SurfaceDescription
                        {
                            float3 Albedo;
                            float Alpha;
                            float AlphaClipThreshold;
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            float4 _Property_56E21223_Out_0 = _FlameOverlayColor;
                            Bindings_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda _FireStencilBaseSubshader_61F589E6;
                            _FireStencilBaseSubshader_61F589E6.uv0 = IN.uv0;
                            _FireStencilBaseSubshader_61F589E6.TimeParameters = IN.TimeParameters;
                            float4 _FireStencilBaseSubshader_61F589E6_Emission_2;
                            float _FireStencilBaseSubshader_61F589E6_Alpha_1;
                            SG_FireStencilBaseSubshader_cf9495d321d93ae43809262b27ab6fda(_Property_56E21223_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _FireStencilBaseSubshader_61F589E6, _FireStencilBaseSubshader_61F589E6_Emission_2, _FireStencilBaseSubshader_61F589E6_Alpha_1);
                            float _Property_D9CC7967_Out_0 = _DissolveAlphaFade;
                            float _Property_FC243360_Out_0 = _DissolveTextureFade;
                            float4 _Property_4122AEAD_Out_0 = _DissolveColor;
                            float _Property_9FF396ED_Out_0 = _DissolveSize;
                            Bindings_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add _SpawnSubshader_B9140A45;
                            _SpawnSubshader_B9140A45.ObjectSpacePosition = IN.ObjectSpacePosition;
                            _SpawnSubshader_B9140A45.uv0 = IN.uv0;
                            float4 _SpawnSubshader_B9140A45_Albedo_3;
                            float4 _SpawnSubshader_B9140A45_Emission_1;
                            float4 _SpawnSubshader_B9140A45_Occlusion_2;
                            float _SpawnSubshader_B9140A45_Alpha_4;
                            SG_SpawnSubshader_4ee9533f64e2d6048bf852ace6192add(_Property_D9CC7967_Out_0, _Property_FC243360_Out_0, _Property_4122AEAD_Out_0, _Property_9FF396ED_Out_0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), _BaseMap_TexelSize, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap), _OcclusionMap_TexelSize, _SpawnSubshader_B9140A45, _SpawnSubshader_B9140A45_Albedo_3, _SpawnSubshader_B9140A45_Emission_1, _SpawnSubshader_B9140A45_Occlusion_2, _SpawnSubshader_B9140A45_Alpha_4);
                            float _Multiply_48CAA7FB_Out_2;
                            Unity_Multiply_float(_FireStencilBaseSubshader_61F589E6_Alpha_1, _SpawnSubshader_B9140A45_Alpha_4, _Multiply_48CAA7FB_Out_2);
                            surface.Albedo = IsGammaSpace() ? float3(0.2735849, 0.2735849, 0.2735849) : SRGBToLinear(float3(0.2735849, 0.2735849, 0.2735849));
                            surface.Alpha = _Multiply_48CAA7FB_Out_2;
                            surface.AlphaClipThreshold = 0;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Structs and Packing

                        // Generated Type: Attributes
                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            float4 uv0 : TEXCOORD0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };

                        // Generated Type: Varyings
                        struct Varyings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 positionWS;
                            float4 texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Generated Type: PackedVaryings
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            float3 interp00 : TEXCOORD0;
                            float4 interp01 : TEXCOORD1;
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Packed Type: Varyings
                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output = (PackedVaryings)0;
                            output.positionCS = input.positionCS;
                            output.interp00.xyz = input.positionWS;
                            output.interp01.xyzw = input.texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // Unpacked Type: Varyings
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output = (Varyings)0;
                            output.positionCS = input.positionCS;
                            output.positionWS = input.interp00.xyz;
                            output.texCoord0 = input.interp01.xyzw;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Build Graph Inputs

                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



                            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


                            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
                            output.uv0 = input.texCoord0;
                            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                        #else
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                        #endif
                        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                        }


                        // --------------------------------------------------
                        // Main

                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                        ENDHLSL
                    }

    }
        CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
                            FallBack "Hidden/Shader Graph/FallbackError"
}
