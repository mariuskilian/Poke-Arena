Shader "Custom/FireStencilShader"
{
    Properties
    {
        [NoScaleOffset] _BaseMap("Base Map", 2D) = "white" {}
        [NoScaleOffset]_OcclusionMap("Occlusion Map", 2D) = "white" {}
        [NoScaleOffset]Texture2D_2A35ABCD("Texture2D", 2D) = "white" {}
        [HDR]_FireYellow("Yellow", Color) = (1.498039, 0.6698205, 0, 0)
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
            float4 _FireYellow;
            CBUFFER_END
            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
            TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
            TEXTURE2D(Texture2D_2A35ABCD); SAMPLER(samplerTexture2D_2A35ABCD); float4 Texture2D_2A35ABCD_TexelSize;
            SAMPLER(_SampleTexture2D_79582B29_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_73D4FE9_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_857EDD6A_Sampler_3_Linear_Repeat);

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

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
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
                float _Multiply_49DDE66A_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_49DDE66A_Out_2);
                float _Modulo_EEC65ADF_Out_2;
                Unity_Modulo_float(_Multiply_49DDE66A_Out_2, 1, _Modulo_EEC65ADF_Out_2);
                float2 _Vector2_FED3727E_Out_0 = float2(1, _Modulo_EEC65ADF_Out_2);
                float2 _TilingAndOffset_4B238405_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_FED3727E_Out_0, _TilingAndOffset_4B238405_Out_3);
                float4 _SampleTexture2D_79582B29_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_4B238405_Out_3);
                float _SampleTexture2D_79582B29_R_4 = _SampleTexture2D_79582B29_RGBA_0.r;
                float _SampleTexture2D_79582B29_G_5 = _SampleTexture2D_79582B29_RGBA_0.g;
                float _SampleTexture2D_79582B29_B_6 = _SampleTexture2D_79582B29_RGBA_0.b;
                float _SampleTexture2D_79582B29_A_7 = _SampleTexture2D_79582B29_RGBA_0.a;
                float4 _Property_5F6E3A3E_Out_0 = _FireYellow;
                float4 _Multiply_AADA6739_Out_2;
                Unity_Multiply_float(_SampleTexture2D_79582B29_RGBA_0, _Property_5F6E3A3E_Out_0, _Multiply_AADA6739_Out_2);
                float _Multiply_274B396A_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_274B396A_Out_2);
                float _Modulo_5059CB67_Out_2;
                Unity_Modulo_float(_Multiply_274B396A_Out_2, 1, _Modulo_5059CB67_Out_2);
                float2 _Vector2_D32FB7DB_Out_0 = float2(1, _Modulo_5059CB67_Out_2);
                float2 _TilingAndOffset_672A9A77_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_D32FB7DB_Out_0, _TilingAndOffset_672A9A77_Out_3);
                float4 _SampleTexture2D_73D4FE9_RGBA_0 = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, _TilingAndOffset_672A9A77_Out_3);
                float _SampleTexture2D_73D4FE9_R_4 = _SampleTexture2D_73D4FE9_RGBA_0.r;
                float _SampleTexture2D_73D4FE9_G_5 = _SampleTexture2D_73D4FE9_RGBA_0.g;
                float _SampleTexture2D_73D4FE9_B_6 = _SampleTexture2D_73D4FE9_RGBA_0.b;
                float _SampleTexture2D_73D4FE9_A_7 = _SampleTexture2D_73D4FE9_RGBA_0.a;
                float4 _Multiply_61182CCF_Out_2;
                Unity_Multiply_float(_Property_5F6E3A3E_Out_0, _SampleTexture2D_73D4FE9_RGBA_0, _Multiply_61182CCF_Out_2);
                float4 _Add_C278635A_Out_2;
                Unity_Add_float4(_Multiply_AADA6739_Out_2, _Multiply_61182CCF_Out_2, _Add_C278635A_Out_2);
                float4 _UV_AE6BD7DD_Out_0 = IN.uv0;
                float _Split_D4FD2D5D_R_1 = _UV_AE6BD7DD_Out_0[0];
                float _Split_D4FD2D5D_G_2 = _UV_AE6BD7DD_Out_0[1];
                float _Split_D4FD2D5D_B_3 = _UV_AE6BD7DD_Out_0[2];
                float _Split_D4FD2D5D_A_4 = _UV_AE6BD7DD_Out_0[3];
                float _Subtract_99591017_Out_2;
                Unity_Subtract_float(0.5, _Split_D4FD2D5D_G_2, _Subtract_99591017_Out_2);
                float4 _Multiply_E36EBD9E_Out_2;
                Unity_Multiply_float(_Property_5F6E3A3E_Out_0, (_Subtract_99591017_Out_2.xxxx), _Multiply_E36EBD9E_Out_2);
                float4 _Add_A8BE5CF3_Out_2;
                Unity_Add_float4(_Add_C278635A_Out_2, _Multiply_E36EBD9E_Out_2, _Add_A8BE5CF3_Out_2);
                float _Multiply_70F1A33D_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_70F1A33D_Out_2);
                float _Modulo_9E496122_Out_2;
                Unity_Modulo_float(_Multiply_70F1A33D_Out_2, 1, _Modulo_9E496122_Out_2);
                float2 _Vector2_A522EDA_Out_0 = float2(0, _Modulo_9E496122_Out_2);
                float2 _TilingAndOffset_B8FC0787_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_A522EDA_Out_0, _TilingAndOffset_B8FC0787_Out_3);
                float4 _SampleTexture2D_857EDD6A_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_B8FC0787_Out_3);
                float _SampleTexture2D_857EDD6A_R_4 = _SampleTexture2D_857EDD6A_RGBA_0.r;
                float _SampleTexture2D_857EDD6A_G_5 = _SampleTexture2D_857EDD6A_RGBA_0.g;
                float _SampleTexture2D_857EDD6A_B_6 = _SampleTexture2D_857EDD6A_RGBA_0.b;
                float _SampleTexture2D_857EDD6A_A_7 = _SampleTexture2D_857EDD6A_RGBA_0.a;
                float _Add_94D28D4B_Out_2;
                Unity_Add_float(_SampleTexture2D_73D4FE9_A_7, _SampleTexture2D_857EDD6A_A_7, _Add_94D28D4B_Out_2);
                float _Multiply_B9A0286D_Out_2;
                Unity_Multiply_float(_Subtract_99591017_Out_2, _Add_94D28D4B_Out_2, _Multiply_B9A0286D_Out_2);
                surface.Albedo = IsGammaSpace() ? float3(0.3301887, 0.3301887, 0.3301887) : SRGBToLinear(float3(0.3301887, 0.3301887, 0.3301887));
                surface.Normal = IN.TangentSpaceNormal;
                surface.Emission = (_Add_A8BE5CF3_Out_2.xyz);
                surface.Metallic = 0;
                surface.Smoothness = 0.5;
                surface.Occlusion = 1;
                surface.Alpha = _Multiply_B9A0286D_Out_2;
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
                float4 _FireYellow;
                CBUFFER_END
                TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                TEXTURE2D(Texture2D_2A35ABCD); SAMPLER(samplerTexture2D_2A35ABCD); float4 Texture2D_2A35ABCD_TexelSize;
                SAMPLER(_SampleTexture2D_73D4FE9_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_857EDD6A_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }

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

                void Unity_Add_float(float A, float B, out float Out)
                {
                    Out = A + B;
                }

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
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
                    float4 _UV_AE6BD7DD_Out_0 = IN.uv0;
                    float _Split_D4FD2D5D_R_1 = _UV_AE6BD7DD_Out_0[0];
                    float _Split_D4FD2D5D_G_2 = _UV_AE6BD7DD_Out_0[1];
                    float _Split_D4FD2D5D_B_3 = _UV_AE6BD7DD_Out_0[2];
                    float _Split_D4FD2D5D_A_4 = _UV_AE6BD7DD_Out_0[3];
                    float _Subtract_99591017_Out_2;
                    Unity_Subtract_float(0.5, _Split_D4FD2D5D_G_2, _Subtract_99591017_Out_2);
                    float _Multiply_274B396A_Out_2;
                    Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_274B396A_Out_2);
                    float _Modulo_5059CB67_Out_2;
                    Unity_Modulo_float(_Multiply_274B396A_Out_2, 1, _Modulo_5059CB67_Out_2);
                    float2 _Vector2_D32FB7DB_Out_0 = float2(1, _Modulo_5059CB67_Out_2);
                    float2 _TilingAndOffset_672A9A77_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_D32FB7DB_Out_0, _TilingAndOffset_672A9A77_Out_3);
                    float4 _SampleTexture2D_73D4FE9_RGBA_0 = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, _TilingAndOffset_672A9A77_Out_3);
                    float _SampleTexture2D_73D4FE9_R_4 = _SampleTexture2D_73D4FE9_RGBA_0.r;
                    float _SampleTexture2D_73D4FE9_G_5 = _SampleTexture2D_73D4FE9_RGBA_0.g;
                    float _SampleTexture2D_73D4FE9_B_6 = _SampleTexture2D_73D4FE9_RGBA_0.b;
                    float _SampleTexture2D_73D4FE9_A_7 = _SampleTexture2D_73D4FE9_RGBA_0.a;
                    float _Multiply_70F1A33D_Out_2;
                    Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_70F1A33D_Out_2);
                    float _Modulo_9E496122_Out_2;
                    Unity_Modulo_float(_Multiply_70F1A33D_Out_2, 1, _Modulo_9E496122_Out_2);
                    float2 _Vector2_A522EDA_Out_0 = float2(0, _Modulo_9E496122_Out_2);
                    float2 _TilingAndOffset_B8FC0787_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_A522EDA_Out_0, _TilingAndOffset_B8FC0787_Out_3);
                    float4 _SampleTexture2D_857EDD6A_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_B8FC0787_Out_3);
                    float _SampleTexture2D_857EDD6A_R_4 = _SampleTexture2D_857EDD6A_RGBA_0.r;
                    float _SampleTexture2D_857EDD6A_G_5 = _SampleTexture2D_857EDD6A_RGBA_0.g;
                    float _SampleTexture2D_857EDD6A_B_6 = _SampleTexture2D_857EDD6A_RGBA_0.b;
                    float _SampleTexture2D_857EDD6A_A_7 = _SampleTexture2D_857EDD6A_RGBA_0.a;
                    float _Add_94D28D4B_Out_2;
                    Unity_Add_float(_SampleTexture2D_73D4FE9_A_7, _SampleTexture2D_857EDD6A_A_7, _Add_94D28D4B_Out_2);
                    float _Multiply_B9A0286D_Out_2;
                    Unity_Multiply_float(_Subtract_99591017_Out_2, _Add_94D28D4B_Out_2, _Multiply_B9A0286D_Out_2);
                    surface.Alpha = _Multiply_B9A0286D_Out_2;
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
                    float4 interp00 : TEXCOORD0;
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
                    output.interp00.xyzw = input.texCoord0;
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
                    output.texCoord0 = input.interp00.xyzw;
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
                    float4 _FireYellow;
                    CBUFFER_END
                    TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                    TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                    TEXTURE2D(Texture2D_2A35ABCD); SAMPLER(samplerTexture2D_2A35ABCD); float4 Texture2D_2A35ABCD_TexelSize;
                    SAMPLER(_SampleTexture2D_73D4FE9_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_857EDD6A_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_Subtract_float(float A, float B, out float Out)
                    {
                        Out = A - B;
                    }

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

                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal;
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
                        float4 _UV_AE6BD7DD_Out_0 = IN.uv0;
                        float _Split_D4FD2D5D_R_1 = _UV_AE6BD7DD_Out_0[0];
                        float _Split_D4FD2D5D_G_2 = _UV_AE6BD7DD_Out_0[1];
                        float _Split_D4FD2D5D_B_3 = _UV_AE6BD7DD_Out_0[2];
                        float _Split_D4FD2D5D_A_4 = _UV_AE6BD7DD_Out_0[3];
                        float _Subtract_99591017_Out_2;
                        Unity_Subtract_float(0.5, _Split_D4FD2D5D_G_2, _Subtract_99591017_Out_2);
                        float _Multiply_274B396A_Out_2;
                        Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_274B396A_Out_2);
                        float _Modulo_5059CB67_Out_2;
                        Unity_Modulo_float(_Multiply_274B396A_Out_2, 1, _Modulo_5059CB67_Out_2);
                        float2 _Vector2_D32FB7DB_Out_0 = float2(1, _Modulo_5059CB67_Out_2);
                        float2 _TilingAndOffset_672A9A77_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_D32FB7DB_Out_0, _TilingAndOffset_672A9A77_Out_3);
                        float4 _SampleTexture2D_73D4FE9_RGBA_0 = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, _TilingAndOffset_672A9A77_Out_3);
                        float _SampleTexture2D_73D4FE9_R_4 = _SampleTexture2D_73D4FE9_RGBA_0.r;
                        float _SampleTexture2D_73D4FE9_G_5 = _SampleTexture2D_73D4FE9_RGBA_0.g;
                        float _SampleTexture2D_73D4FE9_B_6 = _SampleTexture2D_73D4FE9_RGBA_0.b;
                        float _SampleTexture2D_73D4FE9_A_7 = _SampleTexture2D_73D4FE9_RGBA_0.a;
                        float _Multiply_70F1A33D_Out_2;
                        Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_70F1A33D_Out_2);
                        float _Modulo_9E496122_Out_2;
                        Unity_Modulo_float(_Multiply_70F1A33D_Out_2, 1, _Modulo_9E496122_Out_2);
                        float2 _Vector2_A522EDA_Out_0 = float2(0, _Modulo_9E496122_Out_2);
                        float2 _TilingAndOffset_B8FC0787_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_A522EDA_Out_0, _TilingAndOffset_B8FC0787_Out_3);
                        float4 _SampleTexture2D_857EDD6A_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_B8FC0787_Out_3);
                        float _SampleTexture2D_857EDD6A_R_4 = _SampleTexture2D_857EDD6A_RGBA_0.r;
                        float _SampleTexture2D_857EDD6A_G_5 = _SampleTexture2D_857EDD6A_RGBA_0.g;
                        float _SampleTexture2D_857EDD6A_B_6 = _SampleTexture2D_857EDD6A_RGBA_0.b;
                        float _SampleTexture2D_857EDD6A_A_7 = _SampleTexture2D_857EDD6A_RGBA_0.a;
                        float _Add_94D28D4B_Out_2;
                        Unity_Add_float(_SampleTexture2D_73D4FE9_A_7, _SampleTexture2D_857EDD6A_A_7, _Add_94D28D4B_Out_2);
                        float _Multiply_B9A0286D_Out_2;
                        Unity_Multiply_float(_Subtract_99591017_Out_2, _Add_94D28D4B_Out_2, _Multiply_B9A0286D_Out_2);
                        surface.Alpha = _Multiply_B9A0286D_Out_2;
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
                        float4 interp00 : TEXCOORD0;
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
                        output.interp00.xyzw = input.texCoord0;
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
                        output.texCoord0 = input.interp00.xyzw;
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
                        float4 _FireYellow;
                        CBUFFER_END
                        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                        TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                        TEXTURE2D(Texture2D_2A35ABCD); SAMPLER(samplerTexture2D_2A35ABCD); float4 Texture2D_2A35ABCD_TexelSize;
                        SAMPLER(_SampleTexture2D_79582B29_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_73D4FE9_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_857EDD6A_Sampler_3_Linear_Repeat);

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

                        // Graph Vertex
                        // GraphVertex: <None>

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            float3 TangentSpaceNormal;
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
                            float _Multiply_49DDE66A_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_49DDE66A_Out_2);
                            float _Modulo_EEC65ADF_Out_2;
                            Unity_Modulo_float(_Multiply_49DDE66A_Out_2, 1, _Modulo_EEC65ADF_Out_2);
                            float2 _Vector2_FED3727E_Out_0 = float2(1, _Modulo_EEC65ADF_Out_2);
                            float2 _TilingAndOffset_4B238405_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (-1, 1), _Vector2_FED3727E_Out_0, _TilingAndOffset_4B238405_Out_3);
                            float4 _SampleTexture2D_79582B29_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_4B238405_Out_3);
                            float _SampleTexture2D_79582B29_R_4 = _SampleTexture2D_79582B29_RGBA_0.r;
                            float _SampleTexture2D_79582B29_G_5 = _SampleTexture2D_79582B29_RGBA_0.g;
                            float _SampleTexture2D_79582B29_B_6 = _SampleTexture2D_79582B29_RGBA_0.b;
                            float _SampleTexture2D_79582B29_A_7 = _SampleTexture2D_79582B29_RGBA_0.a;
                            float4 _Property_5F6E3A3E_Out_0 = _FireYellow;
                            float4 _Multiply_AADA6739_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_79582B29_RGBA_0, _Property_5F6E3A3E_Out_0, _Multiply_AADA6739_Out_2);
                            float _Multiply_274B396A_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_274B396A_Out_2);
                            float _Modulo_5059CB67_Out_2;
                            Unity_Modulo_float(_Multiply_274B396A_Out_2, 1, _Modulo_5059CB67_Out_2);
                            float2 _Vector2_D32FB7DB_Out_0 = float2(1, _Modulo_5059CB67_Out_2);
                            float2 _TilingAndOffset_672A9A77_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_D32FB7DB_Out_0, _TilingAndOffset_672A9A77_Out_3);
                            float4 _SampleTexture2D_73D4FE9_RGBA_0 = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, _TilingAndOffset_672A9A77_Out_3);
                            float _SampleTexture2D_73D4FE9_R_4 = _SampleTexture2D_73D4FE9_RGBA_0.r;
                            float _SampleTexture2D_73D4FE9_G_5 = _SampleTexture2D_73D4FE9_RGBA_0.g;
                            float _SampleTexture2D_73D4FE9_B_6 = _SampleTexture2D_73D4FE9_RGBA_0.b;
                            float _SampleTexture2D_73D4FE9_A_7 = _SampleTexture2D_73D4FE9_RGBA_0.a;
                            float4 _Multiply_61182CCF_Out_2;
                            Unity_Multiply_float(_Property_5F6E3A3E_Out_0, _SampleTexture2D_73D4FE9_RGBA_0, _Multiply_61182CCF_Out_2);
                            float4 _Add_C278635A_Out_2;
                            Unity_Add_float4(_Multiply_AADA6739_Out_2, _Multiply_61182CCF_Out_2, _Add_C278635A_Out_2);
                            float4 _UV_AE6BD7DD_Out_0 = IN.uv0;
                            float _Split_D4FD2D5D_R_1 = _UV_AE6BD7DD_Out_0[0];
                            float _Split_D4FD2D5D_G_2 = _UV_AE6BD7DD_Out_0[1];
                            float _Split_D4FD2D5D_B_3 = _UV_AE6BD7DD_Out_0[2];
                            float _Split_D4FD2D5D_A_4 = _UV_AE6BD7DD_Out_0[3];
                            float _Subtract_99591017_Out_2;
                            Unity_Subtract_float(0.5, _Split_D4FD2D5D_G_2, _Subtract_99591017_Out_2);
                            float4 _Multiply_E36EBD9E_Out_2;
                            Unity_Multiply_float(_Property_5F6E3A3E_Out_0, (_Subtract_99591017_Out_2.xxxx), _Multiply_E36EBD9E_Out_2);
                            float4 _Add_A8BE5CF3_Out_2;
                            Unity_Add_float4(_Add_C278635A_Out_2, _Multiply_E36EBD9E_Out_2, _Add_A8BE5CF3_Out_2);
                            float _Multiply_70F1A33D_Out_2;
                            Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_70F1A33D_Out_2);
                            float _Modulo_9E496122_Out_2;
                            Unity_Modulo_float(_Multiply_70F1A33D_Out_2, 1, _Modulo_9E496122_Out_2);
                            float2 _Vector2_A522EDA_Out_0 = float2(0, _Modulo_9E496122_Out_2);
                            float2 _TilingAndOffset_B8FC0787_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_A522EDA_Out_0, _TilingAndOffset_B8FC0787_Out_3);
                            float4 _SampleTexture2D_857EDD6A_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_B8FC0787_Out_3);
                            float _SampleTexture2D_857EDD6A_R_4 = _SampleTexture2D_857EDD6A_RGBA_0.r;
                            float _SampleTexture2D_857EDD6A_G_5 = _SampleTexture2D_857EDD6A_RGBA_0.g;
                            float _SampleTexture2D_857EDD6A_B_6 = _SampleTexture2D_857EDD6A_RGBA_0.b;
                            float _SampleTexture2D_857EDD6A_A_7 = _SampleTexture2D_857EDD6A_RGBA_0.a;
                            float _Add_94D28D4B_Out_2;
                            Unity_Add_float(_SampleTexture2D_73D4FE9_A_7, _SampleTexture2D_857EDD6A_A_7, _Add_94D28D4B_Out_2);
                            float _Multiply_B9A0286D_Out_2;
                            Unity_Multiply_float(_Subtract_99591017_Out_2, _Add_94D28D4B_Out_2, _Multiply_B9A0286D_Out_2);
                            surface.Albedo = IsGammaSpace() ? float3(0.3301887, 0.3301887, 0.3301887) : SRGBToLinear(float3(0.3301887, 0.3301887, 0.3301887));
                            surface.Emission = (_Add_A8BE5CF3_Out_2.xyz);
                            surface.Alpha = _Multiply_B9A0286D_Out_2;
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
                            float4 interp00 : TEXCOORD0;
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
                            output.interp00.xyzw = input.texCoord0;
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
                            output.texCoord0 = input.interp00.xyzw;
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
                            float4 _FireYellow;
                            CBUFFER_END
                            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                            TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap); float4 _OcclusionMap_TexelSize;
                            TEXTURE2D(Texture2D_2A35ABCD); SAMPLER(samplerTexture2D_2A35ABCD); float4 Texture2D_2A35ABCD_TexelSize;
                            SAMPLER(_SampleTexture2D_73D4FE9_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_857EDD6A_Sampler_3_Linear_Repeat);

                            // Graph Functions

                            void Unity_Subtract_float(float A, float B, out float Out)
                            {
                                Out = A - B;
                            }

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

                            void Unity_Add_float(float A, float B, out float Out)
                            {
                                Out = A + B;
                            }

                            // Graph Vertex
                            // GraphVertex: <None>

                            // Graph Pixel
                            struct SurfaceDescriptionInputs
                            {
                                float3 TangentSpaceNormal;
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
                                float4 _UV_AE6BD7DD_Out_0 = IN.uv0;
                                float _Split_D4FD2D5D_R_1 = _UV_AE6BD7DD_Out_0[0];
                                float _Split_D4FD2D5D_G_2 = _UV_AE6BD7DD_Out_0[1];
                                float _Split_D4FD2D5D_B_3 = _UV_AE6BD7DD_Out_0[2];
                                float _Split_D4FD2D5D_A_4 = _UV_AE6BD7DD_Out_0[3];
                                float _Subtract_99591017_Out_2;
                                Unity_Subtract_float(0.5, _Split_D4FD2D5D_G_2, _Subtract_99591017_Out_2);
                                float _Multiply_274B396A_Out_2;
                                Unity_Multiply_float(IN.TimeParameters.x, -1.5, _Multiply_274B396A_Out_2);
                                float _Modulo_5059CB67_Out_2;
                                Unity_Modulo_float(_Multiply_274B396A_Out_2, 1, _Modulo_5059CB67_Out_2);
                                float2 _Vector2_D32FB7DB_Out_0 = float2(1, _Modulo_5059CB67_Out_2);
                                float2 _TilingAndOffset_672A9A77_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_D32FB7DB_Out_0, _TilingAndOffset_672A9A77_Out_3);
                                float4 _SampleTexture2D_73D4FE9_RGBA_0 = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, _TilingAndOffset_672A9A77_Out_3);
                                float _SampleTexture2D_73D4FE9_R_4 = _SampleTexture2D_73D4FE9_RGBA_0.r;
                                float _SampleTexture2D_73D4FE9_G_5 = _SampleTexture2D_73D4FE9_RGBA_0.g;
                                float _SampleTexture2D_73D4FE9_B_6 = _SampleTexture2D_73D4FE9_RGBA_0.b;
                                float _SampleTexture2D_73D4FE9_A_7 = _SampleTexture2D_73D4FE9_RGBA_0.a;
                                float _Multiply_70F1A33D_Out_2;
                                Unity_Multiply_float(IN.TimeParameters.x, -1.2, _Multiply_70F1A33D_Out_2);
                                float _Modulo_9E496122_Out_2;
                                Unity_Modulo_float(_Multiply_70F1A33D_Out_2, 1, _Modulo_9E496122_Out_2);
                                float2 _Vector2_A522EDA_Out_0 = float2(0, _Modulo_9E496122_Out_2);
                                float2 _TilingAndOffset_B8FC0787_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_A522EDA_Out_0, _TilingAndOffset_B8FC0787_Out_3);
                                float4 _SampleTexture2D_857EDD6A_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, _TilingAndOffset_B8FC0787_Out_3);
                                float _SampleTexture2D_857EDD6A_R_4 = _SampleTexture2D_857EDD6A_RGBA_0.r;
                                float _SampleTexture2D_857EDD6A_G_5 = _SampleTexture2D_857EDD6A_RGBA_0.g;
                                float _SampleTexture2D_857EDD6A_B_6 = _SampleTexture2D_857EDD6A_RGBA_0.b;
                                float _SampleTexture2D_857EDD6A_A_7 = _SampleTexture2D_857EDD6A_RGBA_0.a;
                                float _Add_94D28D4B_Out_2;
                                Unity_Add_float(_SampleTexture2D_73D4FE9_A_7, _SampleTexture2D_857EDD6A_A_7, _Add_94D28D4B_Out_2);
                                float _Multiply_B9A0286D_Out_2;
                                Unity_Multiply_float(_Subtract_99591017_Out_2, _Add_94D28D4B_Out_2, _Multiply_B9A0286D_Out_2);
                                surface.Albedo = IsGammaSpace() ? float3(0.3301887, 0.3301887, 0.3301887) : SRGBToLinear(float3(0.3301887, 0.3301887, 0.3301887));
                                surface.Alpha = _Multiply_B9A0286D_Out_2;
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
                                float4 interp00 : TEXCOORD0;
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
                                output.interp00.xyzw = input.texCoord0;
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
                                output.texCoord0 = input.interp00.xyzw;
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
