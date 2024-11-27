Shader "Custom/Minimap" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_MainTexRatio("Main Texture Ratio", Range(0, 1)) = 1
		_BaseColor ("Base Color", Color) = (0,0,0,1)
		_MapColor ("Map Color", Color) = (1,1,1,1)
		_MapMagnification ("Map Magnification", Range(0.1, 100000)) = 10000
		_Thick("Thick", Range(0.1, 5)) = 1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		ZTest Off
		ZWrite Off
		Lighting Off
		AlphaTest Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _MainTexRatio;
			float4  _MainTex_ST;
			float _MapMagnification;
			float4 _MapColor;
			float4 _BaseColor;
			float _Thick;

			sampler2D _CameraDepthTexture;
			float4  _CameraDepthTexture_TexelSize;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f input) : Color
			{
				float tx = _CameraDepthTexture_TexelSize.x * _Thick;
				float ty = _CameraDepthTexture_TexelSize.y * _Thick;

				float col00 = Linear01Depth(tex2D(_CameraDepthTexture, input.texcoord + half2(-tx, -ty)).r);
				float col10 = Linear01Depth(tex2D(_CameraDepthTexture, input.texcoord + half2(  0, -ty)).r);
				float col01 = Linear01Depth(tex2D(_CameraDepthTexture, input.texcoord + half2(-tx,   0)).r);
				float col11 = Linear01Depth(tex2D(_CameraDepthTexture, input.texcoord + half2(  0,   0)).r);
				float val = (abs(col00 - col11) + abs(col10 - col01))/2;

				float4 main_col = tex2D(_MainTex, input.texcoord);
				float map_ratio = min(val*_MapMagnification,1);
				float4 map_color = _MapColor * map_ratio;				
				map_color.a = 1;
				fixed4 col = (_BaseColor*(1 - _MainTexRatio) + main_col*_MainTexRatio)*(1 - map_ratio) + map_color*map_ratio;
				return col;
			}
			ENDCG
		}
	} 
}
