Shader "MapEditor/PointingDraw"
{
	Properties
	{
		_CursorPositon ("Cursor Posision", Vector) = (0, 0, 0, 0)
		_Color ("Color", Vector) = (0, 0, 0, 0)
		_Width ("Width", Float) = 1
		_Bold ("Bold", Float) = 0.5
		_Step ("Step", Float) = 2.0
	}
	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
		
		Cull Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 wc : TEXCOORD1;
				float4 vertex : SV_POSITION;
				
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.wc = v.vertex;
				o.uv = v.uv;
				return o;
			}
			
			float4 _CursorPosition;
			float4 _Color;
			float _Width;
			float _Bold;
			float _Step;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 tex = i.wc.xy;
				float2 cursor = _CursorPosition;
				float w = _Width * 0.5f;
				
				bool insideX = cursor.x - w <= tex.x && tex.x <= cursor.x + w;
				bool insideY = cursor.y - w <= tex.y && tex.y <= cursor.y + w; 
				
				if(!insideX && !insideY) discard;
				if(insideX && insideY) discard;
				
				// Get uv.
				float2 uv = float2(0, 0);
				if(insideY) uv = float2(tex.x - cursor.x, tex.y - cursor.y);
				if(insideX) uv = float2(tex.y - cursor.y, tex.x - cursor.x);
				if(uv.x < 0.0f) uv = -uv;
				uv.x -= w;
				uv.x %= w * _Step;
				
				// Sampling from calculated texture.
				uv.y = max(uv.y, -uv.y);
				float k = w / (2 * w - _Bold);
				float bx = uv.y / k;
				if(!(bx <= uv.x)) discard;
				if(!(uv.x <= bx + _Bold * w * 2)) discard;
				return _Color;
			}
			ENDCG
		}
	}
}
