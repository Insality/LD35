﻿Shader "Hidden/ShineShader" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
half _Intensity;

fixed4 frag (v2f_img i) : SV_Target
{	
	fixed4 original = tex2D(_MainTex, i.uv);
	
	return original + _Intensity;
}
ENDCG

	}
}

Fallback off

}
