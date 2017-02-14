Shader "Separate Alpha Mask" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Alpha("Alpha (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off
		Cull Off
		ZTest Always
		ZWrite Off
		Fog{ Mode Off }


		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass{
		SetTexture[_MainTex]{
		Combine texture
	}
		SetTexture[_Alpha]{
		Combine previous, texture
	}
	}
	}
}
