// Shader created with Shader Forge Beta 0.25 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.25;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:3,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:0,x:32343,y:32843|diff-254-OUT,spec-366-OUT,gloss-146-OUT,normal-349-RGB,transm-339-OUT,amspl-191-OUT;n:type:ShaderForge.SFN_Tex2d,id:138,x:32898,y:32562,ptlb:MainTex,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:146,x:32850,y:32945|A-147-OUT,B-376-OUT;n:type:ShaderForge.SFN_Power,id:147,x:33022,y:32882|VAL-138-RGB,EXP-148-OUT;n:type:ShaderForge.SFN_Vector1,id:148,x:33207,y:32916,v1:3;n:type:ShaderForge.SFN_Multiply,id:191,x:32683,y:33126|A-138-RGB,B-261-OUT;n:type:ShaderForge.SFN_Tex2d,id:249,x:32769,y:32358,ptlb:LightMap,ntxv:0,isnm:False|UVIN-269-UVOUT,MIP-269-U;n:type:ShaderForge.SFN_Multiply,id:250,x:32539,y:32240|A-252-OUT,B-249-RGB;n:type:ShaderForge.SFN_Slider,id:252,x:32890,y:32266,ptlb:LightmapBrightness,min:1,cur:3,max:5;n:type:ShaderForge.SFN_Multiply,id:253,x:32490,y:32495|A-250-OUT,B-138-RGB;n:type:ShaderForge.SFN_Multiply,id:254,x:32325,y:32151|A-255-RGB,B-253-OUT;n:type:ShaderForge.SFN_Color,id:255,x:32574,y:32067,ptlb:Color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:261,x:32906,y:33319,ptlb:SpcAmbientLit,v1:0.2;n:type:ShaderForge.SFN_TexCoord,id:269,x:33013,y:32371,uv:1;n:type:ShaderForge.SFN_Multiply,id:339,x:32558,y:33349|A-138-RGB,B-340-OUT;n:type:ShaderForge.SFN_ValueProperty,id:340,x:32962,y:33450,ptlb:TransmissionColor,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:349,x:32644,y:32945,ptlb:Normal,ntxv:3,isnm:True;n:type:ShaderForge.SFN_ValueProperty,id:366,x:32871,y:32839,ptlb:Specular,v1:0.07;n:type:ShaderForge.SFN_ValueProperty,id:376,x:33040,y:33155,ptlb:Gloss,v1:0.6;proporder:255-340-138-261-366-376-349-249-252;pass:END;sub:END;*/

Shader "Props/Dif_Spc(Cloth)" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _TransmissionColor ("TransmissionColor", Float ) = 0.2
        _MainTex ("MainTex", 2D) = "white" {}
        _SpcAmbientLit ("SpcAmbientLit", Float ) = 0.2
        _Specular ("Specular", Float ) = 0.07
        _Gloss ("Gloss", Float ) = 0.6
        _Normal ("Normal", 2D) = "bump" {}
        _LightMap ("LightMap", 2D) = "white" {}
        _LightmapBrightness ("LightmapBrightness", Range(1, 5)) = 3
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
            #pragma exclude_renderers gles xbox360 ps3 flash 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _LightMap; uniform float4 _LightMap_ST;
            uniform float _LightmapBrightness;
            uniform float4 _Color;
            uniform float _SpcAmbientLit;
            uniform float _TransmissionColor;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Specular;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_390 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_390.rg, _Normal))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
                float NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float4 node_138 = tex2D(_MainTex,TRANSFORM_TEX(node_390.rg, _MainTex));
                float3 backLight = max(0.0, -NdotL ) * (node_138.rgb*_TransmissionColor);
                float3 diffuse = (forwardLight+backLight)*InvPi * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz*2;
///////// Gloss:
                float gloss = exp2((pow(node_138.rgb,3.0)*_Gloss)*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float _Specular_var = _Specular;
                float3 specularColor = float3(_Specular_var,_Specular_var,_Specular_var);
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float HdotL = max(0.0,dot(halfDirection,lightDirection));
                float3 fresnelTerm = specularColor + ( 1.0 - specularColor ) * pow((1.0 - HdotL),5);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float3 fresnelTermAmb = specularColor + ( 1.0 - specularColor ) * pow((1.0 - NdotV),5);
                float alpha = 1.0 / ( sqrt( (Pi/4.0) * gloss + Pi/2.0 ) );
                float visTerm = ( NdotL * ( 1.0 - alpha ) + alpha ) * ( NdotV * ( 1.0 - alpha ) + alpha );
                visTerm = 1.0 / visTerm;
                float normTerm = (gloss + 8.0 ) / (8.0 * Pi);
                float3 specularAmb = (node_138.rgb*_SpcAmbientLit) * fresnelTermAmb;
                float3 specular = (floor(attenuation) * _LightColor0.xyz)*NdotL * pow(max(0,dot(halfDirection,normalDirection)),gloss)*fresnelTerm*visTerm*normTerm + specularAmb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight *= 1-specularMonochrome;
                float2 node_269 = i.uv1;
                finalColor += diffuseLight * (_Color.rgb*((_LightmapBrightness*tex2Dlod(_LightMap,float4(TRANSFORM_TEX(node_269.rg, _LightMap),0.0,node_269.r)).rgb)*node_138.rgb));
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
            #pragma exclude_renderers gles xbox360 ps3 flash 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _LightMap; uniform float4 _LightMap_ST;
            uniform float _LightmapBrightness;
            uniform float4 _Color;
            uniform float _TransmissionColor;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Specular;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_391 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_391.rg, _Normal))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
                float NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float4 node_138 = tex2D(_MainTex,TRANSFORM_TEX(node_391.rg, _MainTex));
                float3 backLight = max(0.0, -NdotL ) * (node_138.rgb*_TransmissionColor);
                float3 diffuse = (forwardLight+backLight)*InvPi * attenColor;
///////// Gloss:
                float gloss = exp2((pow(node_138.rgb,3.0)*_Gloss)*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float _Specular_var = _Specular;
                float3 specularColor = float3(_Specular_var,_Specular_var,_Specular_var);
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float HdotL = max(0.0,dot(halfDirection,lightDirection));
                float3 fresnelTerm = specularColor + ( 1.0 - specularColor ) * pow((1.0 - HdotL),5);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float alpha = 1.0 / ( sqrt( (Pi/4.0) * gloss + Pi/2.0 ) );
                float visTerm = ( NdotL * ( 1.0 - alpha ) + alpha ) * ( NdotV * ( 1.0 - alpha ) + alpha );
                visTerm = 1.0 / visTerm;
                float normTerm = (gloss + 8.0 ) / (8.0 * Pi);
                float3 specular = attenColor*NdotL * pow(max(0,dot(halfDirection,normalDirection)),gloss)*fresnelTerm*visTerm*normTerm;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight *= 1-specularMonochrome;
                float2 node_269 = i.uv1;
                finalColor += diffuseLight * (_Color.rgb*((_LightmapBrightness*tex2Dlod(_LightMap,float4(TRANSFORM_TEX(node_269.rg, _LightMap),0.0,node_269.r)).rgb)*node_138.rgb));
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
