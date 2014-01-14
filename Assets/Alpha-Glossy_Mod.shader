Shader "Transparent/Double-Sided" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

	Alphatest Greater 0
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Shininess [_Shininess]
			Specular [_SpecColor]
		}
		Lighting On
		SeparateSpecular On
		Cull front
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE, texture * primary
		}
	}
	
	Pass {
		Tags { "LightMode" = "Vertex" }
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Shininess [_Shininess]
			Specular [_SpecColor]
		}
		Lighting On
		SeparateSpecular On
		Cull back
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE, texture * primary
		}
	}
}

}
