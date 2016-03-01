 Shader "Props/Dif_Nor_Rim" {
    Properties {
      _Color ("Main Color", Color) = (0,0,0,0)
      _MainTex ("Base(RGB)", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
      _LightMap ("Lightmap (RGB)", 2D) = "white" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      fixed4 _Color;
        sampler2D _MainTex;
      sampler2D _BumpMap;
      sampler2D _LightMap;
      float4 _RimColor;
      float _RimPower;
      
           struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
           float2 uv2_LightMap;
          float3 viewDir;
      };
    
      
      
      void surf (Input IN, inout SurfaceOutput o) {
          half4 lm = tex2D (_LightMap, IN.uv2_LightMap)* 1.2;
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * lm.rgb * _Color;
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }
