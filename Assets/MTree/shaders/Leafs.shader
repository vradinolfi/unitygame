
Shader "Mtree/Leafs" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpStrength("Normal Strength", float) = 1
		_Glossiness("Smoothness", Range(0,1)) = 0
		_Ao("AO strength", Range(0,1)) = 0.5
		_Translucency("Translucency", Range(0,2)) = 0.7
		_TranslucencyColor("Translucency Color", Color) = (1,0.7,0,1)
		_Cutoff("Alpha Clip", Range(0,1)) = 0.4
		_ColorVariation("leafs color variation", Color) = (1,1,0,1)
		[PerRendererData]_WindStrength("WindStrength", float) = 0.1
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull off
		CGPROGRAM

		#pragma target 3.0
		#pragma surface surf Standard vertex:vert addshadow dithercrossfade // Important note: the "automatically setting target of LOD_FADE_CROSSFADE to 3.0" warning is to be ignored, there is no way around it.

		#pragma instancing_options procedural:setup
		#pragma multi_compile GPU_FRUSTUM_ON __
		#include "VS_indirect.cginc"


		struct Input {
			float2 uv_MainTex;
			float4 vertexColor;
			float3 worldPos;
			float3 viewDir;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
		};
		half _WindStrength;

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method

			float3 v_pos = v.vertex.xyz;
			float1 turbulence = sin(_Time * 40 - mul(unity_ObjectToWorld, v.vertex).z / 15) * .5f;
			float3 dir = float3(1, 0, 0);
			float1 angle = _WindStrength * (1 + sin(_Time * 2 + turbulence - v_pos.z / 50 - v.color.x / 20)) * sqrt(v.color.x) * .02f;
			float1 y = v_pos.y;
			float1 z = v_pos.z;
			float1 cos_a = cos(angle);
			float1 sin_a = sin(angle);
			//float1 leaf_turbulence = sin(sin(_Time/5 * (1 + angle * 3) + v.color.y*3.14)*.6 - v_pos.z * 2 + _Time *10 + v.color.y*3.14) *.4 * v.color.z * (min(.5, _WindStrength) + angle);
			float1 leaf_turbulence = sin(_Time * 200 * (.2+v.color.g) + v.color.g * 10 + turbulence + v_pos.z/2) * v.color.z * (angle + _WindStrength /200);
			v_pos.y = y * cos_a - z * sin_a;
			v_pos.y += leaf_turbulence;
			v_pos.z = y * sin_a + z * cos_a;

			v.vertex.xyz = v_pos;
		}

		sampler2D _MainTex;
		sampler2D _BumpMap;
		half _BumpStrength;
		half _Glossiness;
		fixed4 _Color;
		fixed4 _ColorVariation;
		fixed4 _TranslucencyColor;
		half _Cutoff;
		half _Ao;
		half _Translucency;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			fixed3 lightDir = normalize(_WorldSpaceLightPos0);
			fixed3 viewDir = normalize(UNITY_MATRIX_IT_MV[2].xyz);
			fixed3 trans = pow(max(0, dot(viewDir, -lightDir)), 8.0) * _Translucency * _TranslucencyColor * (IN.vertexColor.a * .5 + .5);
			
			o.Albedo = (lerp(_ColorVariation, float3(1,1,1), IN.vertexColor.g)) * c.rgb * (1 + trans);
			//o.Albedo = dot(viewDir, -lightDir);
			o.Occlusion = lerp(1, IN.vertexColor.a, _Ao);
			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpStrength);
			// Metallic and smoothness come from slider variables
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			clip(c.a - _Cutoff);
		}


		ENDCG
	}
	FallBack "Diffuse"
}
