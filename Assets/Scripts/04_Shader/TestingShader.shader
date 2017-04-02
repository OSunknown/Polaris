Shader "Custom/TestingShader" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Alpha("Alpha (A)", 2D) = "white" {}
	_MainTex3("Albedo (RGB)", 2D) = "white" {}

	_BumpMap("Normal", 2D) = "bump" {}  //r,g
	}
		SubShader{
		Tags{ "RenderType" = "Transpaarent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _Alpha;
	sampler2D _MainTex3;
	sampler2D _BumpMap;

	struct Input {
		float2 uv_MainTex;
		float2 uv_Alpha;
		float2 uv_MainTex3;
		float2 uv_BumpMap;
	};



	void surf(Input IN, inout SurfaceOutputStandard o) {

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 m = tex2D(_Alpha,IN.uv_Alpha);
		fixed4 ao = tex2D(_MainTex3,IN.uv_MainTex);

		o.Occlusion = ao.r;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		o.Albedo = c.rgb;
		o.Metallic = 0;
		o.Smoothness = 0.5;
		o.Alpha = m;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
