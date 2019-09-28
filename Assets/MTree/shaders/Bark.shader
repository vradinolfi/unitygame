
Shader "Mtree/Bark" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_BumpMap("Normal map", 2D) = "bump" {}
	_BumpStrength("Normal map strength", float) = 1
		_Glossiness("Smoothness", Range(0,1)) = 0.1
		_Ao("AO strength", Range(0,1)) = 0.5
		[PerRendererData]_WindStrength("WindStrength", float) = 0.1
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma target 3.0
#pragma surface surf Standard vertex:vert addshadow dithercrossfade // Important note: the "automatically setting target of LOD_FADE_CROSSFADE to 3.0" warning is to be ignored, there is no way around it.

#pragma instancing_options procedural:setup 
#pragma multi_compile GPU_FRUSTUM_ON __
#include "VS_indirect.cginc"


		struct Input {
		float2 uv_MainTex;
		float4 vertexColor;
		float3 worldPos;
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
		v_pos.y = y * cos_a - z * sin_a;
		v_pos.z = y * sin_a + z * cos_a;

		v.vertex.xyz = v_pos;
	}

	sampler2D _MainTex;
	sampler2D _BumpMap;
	half _Glossiness;
	fixed4 _Color;
	half _Ao;
	half _BumpStrength;

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


		void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		fixed3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		normal.xy *= _BumpStrength;
		o.Normal = normalize(normal);
		//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		o.Occlusion = lerp(1, IN.vertexColor.a, _Ao);
		// Metallic and smoothness come from slider variables
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}


	ENDCG
	}
		FallBack "Diffuse"
}
