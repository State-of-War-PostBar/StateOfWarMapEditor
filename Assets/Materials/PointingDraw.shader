Shader "MapEditor/PointingDraw"
{
	Properties
	{
		_CursorPositon ("Cursor Posision", Vector) = (0, 0, 0, 0)
		_Color ("Color", Vector) = (0, 0, 0, 0)
		_Width ("Width", Float) = 1
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
				// float4 vertColor : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				// float4 vertColor : COLOR0;
				float4 wc : TEXCOORD1;
				float4 vertex : SV_POSITION;
				
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.wc = v.vertex;
				o.uv = v.uv;
				// o.vertColor = v.vertColor;
				return o;
			}
			
			float4 _CursorPosition;
			float4 _Color;
			float _Width;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 tex = i.wc.xy;
				float2 cursor = _CursorPosition;
				float w = _Width * 0.5f;
				
				bool insideX = cursor.x - w <= tex.x && tex.x <= cursor.x + w;
				bool insideY = cursor.y - w <= tex.y && tex.y <= cursor.y + w; 
				
				if(!insideX && !insideY) discard;
				
				return _Color;
			}
			ENDCG
		}
	}
}
