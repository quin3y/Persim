// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.25 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.25;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-12-OUT,spec-23-OUT,gloss-24-OUT,transm-37-RGB,amspl-77-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33827,y:32532,ptlb:MainTex,tex:3d403fe3184a448fa8bc190c7f07f28c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:8,x:33075,y:32435,ptlb:Color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:10,x:32887,y:32526|A-8-RGB,B-58-OUT;n:type:ShaderForge.SFN_Tex2d,id:11,x:33063,y:32231,ptlb:LightMap,ntxv:0,isnm:False|UVIN-16-UVOUT,MIP-16-U;n:type:ShaderForge.SFN_Multiply,id:12,x:32835,y:32329|A-13-OUT,B-10-OUT;n:type:ShaderForge.SFN_Multiply,id:13,x:32939,y:32068|A-11-RGB,B-15-OUT;n:type:ShaderForge.SFN_Slider,id:15,x:33175,y:32091,ptlb:LightBrightness,min:1,cur:3,max:5;n:type:ShaderForge.SFN_TexCoord,id:16,x:33293,y:32161,uv:1;n:type:ShaderForge.SFN_ValueProperty,id:23,x:33026,y:32814,ptlb:Specular,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:24,x:33007,y:32918,ptlb:Gloss,v1:0.3;n:type:ShaderForge.SFN_Color,id:37,x:33021,y:33180,ptlb:TransmissionColor,c1:0.625,c2:0.5489351,c3:0.3492647,c4:1;n:type:ShaderForge.SFN_Fresnel,id:50,x:33814,y:32369;n:type:ShaderForge.SFN_Multiply,id:51,x:33517,y:32521|A-50-OUT,B-2-RGB;n:type:ShaderForge.SFN_Lerp,id:58,x:33275,y:32390|A-2-RGB,B-51-OUT,T-50-OUT;n:type:ShaderForge.SFN_Cubemap,id:64,x:33349,y:32939,ptlb:Cube,cube:a596436b21c6d484bb9b3b6385e3e666,pvfc:0;n:type:ShaderForge.SFN_ValueProperty,id:76,x:33288,y:33155,ptlb:CubeIntensity,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:77,x:33110,y:32990|A-64-RGB,B-76-OUT;proporder:8-37-2-23-24-64-76-11-15;pass:END;sub:END;*/

Shader "Props/Dif_Spc_Ref" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _TransmissionColor ("TransmissionColor", Color) = (0.625,0.5489351,0.3492647,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Specular ("Specular", Float ) = 0.5
        _Gloss ("Gloss", Float ) = 0.3
        _Cube ("Cube", Cube) = "_Skybox" {}
        _CubeIntensity ("CubeIntensity", Float ) = 0.2
        _LightMap ("LightMap", 2D) = "white" {}
        _LightBrightness ("LightBrightness", Range(1, 5)) = 3
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _LightMap; uniform float4 _LightMap_ST;
            uniform float _LightBrightness;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float4 _TransmissionColor;
            uniform samplerCUBE _Cube;
            uniform float _CubeIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float3 backLight = max(0.0, -NdotL ) * _TransmissionColor.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
///////// Gloss:
                float gloss = exp2(_Gloss*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float _Specular_var = _Specular;
                float3 specularColor = float3(_Specular_var,_Specular_var,_Specular_var);
                float3 specularAmb = (texCUBE(_Cube,viewReflectDirection).rgb*_CubeIntensity) * specularColor;
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor + specularAmb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_16 = i.uv1;
                float2 node_91 = i.uv0;
                float4 node_2 = tex2D(_MainTex,TRANSFORM_TEX(node_91.rg, _MainTex));
                float node_50 = (1.0-max(0,dot(normalDirection, viewDirection)));
                finalColor += diffuseLight * ((tex2Dlod(_LightMap,float4(TRANSFORM_TEX(node_16.rg, _LightMap),0.0,node_16.r)).rgb*_LightBrightness)*(_Color.rgb*lerp(node_2.rgb,(node_50*node_2.rgb),node_50)));
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _LightMap; uniform float4 _LightMap_ST;
            uniform float _LightBrightness;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float4 _TransmissionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float3 backLight = max(0.0, -NdotL ) * _TransmissionColor.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor;
///////// Gloss:
                float gloss = exp2(_Gloss*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float _Specular_var = _Specular;
                float3 specularColor = float3(_Specular_var,_Specular_var,_Specular_var);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_16 = i.uv1;
                float2 node_92 = i.uv0;
                float4 node_2 = tex2D(_MainTex,TRANSFORM_TEX(node_92.rg, _MainTex));
                float node_50 = (1.0-max(0,dot(normalDirection, viewDirection)));
                finalColor += diffuseLight * ((tex2Dlod(_LightMap,float4(TRANSFORM_TEX(node_16.rg, _LightMap),0.0,node_16.r)).rgb*_LightBrightness)*(_Color.rgb*lerp(node_2.rgb,(node_50*node_2.rgb),node_50)));
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
