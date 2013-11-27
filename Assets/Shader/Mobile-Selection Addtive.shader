// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Selection/AlphaIgnoreZDepth" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_MainColor ("Color", Color) = (1, 1, 1, 1)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend One OneMinusSrcAlpha
	Cull Off Lighting Off ZTest Always ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				constantColor [_MainColor]
				//combine constant * texture
				combine texture * constant
			}
		}
	}
}
}