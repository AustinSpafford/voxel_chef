Shader "Unlit/PixelatedVoxelShader"
{
	Properties
	{
		_MainTex ("Main Texture (3D)", 3D) = "white" {}
		_MainTex_ActualTexelSize ("Texel Size (script calculated)", Vector) =  (1, 1, 1, 1)
		_MainTexInvScale ("Main Texture Tiling", Vector) =  (1, 1, 1, 1)
		_MainTexOffset ("Main Texture Offset", Vector) =  (0, 0, 0, 0)

		_Color ("Main Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		// Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		// Cull Off
		// ZWrite Off
		// Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texCoord : TEXCOORD0;
			};

			sampler3D _MainTex;
			uniform float4 _MainTex_ActualTexelSize;
			float4 _MainTexInvScale;
			float4 _MainTexOffset;
			float4 _Color;
			
			v2f vertexShader(appdata input)
			{
				v2f output;
				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);

				// We're intentionally mapping the texture coordinates to world-space.
				// This makes it easy to drag objects through the 3D texture to
				// examine the resulting intersections.
				float4 baseTexCoord = mul(_Object2World, input.vertex);

				output.texCoord = ((baseTexCoord * _MainTexInvScale) + _MainTexOffset);

				return output;
			}
			
			fixed4 fragmentShader(v2f input) : SV_Target
			{
				float4 snappedTexCoord = input.texCoord;
				snappedTexCoord /= _MainTex_ActualTexelSize;
				snappedTexCoord = floor(snappedTexCoord);
				snappedTexCoord *= _MainTex_ActualTexelSize;
				
				// Offset into the sampling the texel's true color.
				snappedTexCoord += (_MainTex_ActualTexelSize / 2);

				fixed4 output = (tex3D(_MainTex, snappedTexCoord.xyz) * _Color);

				return output;
			}

			ENDCG
		}
	}
}
