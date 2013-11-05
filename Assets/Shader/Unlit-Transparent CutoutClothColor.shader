// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color
Shader "Unlit/Transparent ClothColor" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_ClothTex ("Cloth (A)", 2D) = "white" {}
	_ClothColor ("Cloth Color", Color) = (1,1,1,1)
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
 
SubShader {
	Tags {"RenderType"="Opaque"}
	LOD 200
 
	CGPROGRAM
	#pragma surface surf Lambert alphatest:_Cutoff
 
	sampler2D _MainTex;
	sampler2D _ClothTex;
	fixed4 _ClothColor;
 
	struct Input {
		float2 uv_MainTex;
	};

	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 m = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c = tex2D(_ClothTex, IN.uv_MainTex) * _ClothColor;
	
		//if(c.a >= 0.8)
		//{
		//	o.Albedo = c.rgb * c.a;
		//}
		//else
		//{
			o.Albedo = m.rgb * m.a + c.rgb * c.a;
		//}
		o.Albedo = m.rgb * m.a + c.rgb * c.a;
		o.Alpha = m.a + c.a;
	}
ENDCG
}
 
//Fallback "Transparent/VertexLit"
}