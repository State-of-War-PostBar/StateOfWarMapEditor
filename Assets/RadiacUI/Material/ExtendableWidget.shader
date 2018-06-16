Shader "RadiacUI/ExtendableWidget"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _DrawRect ("Drawing rectangle", Vector) = (0, 0, 0, 0)
        _ScreenSize ("Screen size", Vector) = (1, 1, 0, 0)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
		
        Pass
        {
            Name "Default"
        
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
			
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
			
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
			
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 normTexCoord  : TEXCOORD0;
                float4 worldCoord : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
			
            fixed4 _Color;
            float4 _MainTex_TexelSize;
			float4 _DrawRect;
			float4 _ScreenSize;
            sampler2D _MainTex;
            float4 _ClipRect;
			
            
            v2f vert(appdata_t v)
            {
                v2f OUT;
				
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
                OUT.worldCoord = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldCoord);
				
                OUT.normTexCoord = v.texcoord;
				
                OUT.color = v.color * _Color;
                return OUT;
            }
            
            
            fixed4 frag(v2f IN) : SV_Target
            {
                float2 texSize = _MainTex_TexelSize.zw;
                float2 texCoord = IN.normTexCoord * texSize;
                float2 sampleCoord = texCoord;
                float2 fragCoord = IN.worldCoord + (_ScreenSize.xy) * 0.5f;
                
                float l = _DrawRect.x;
                float r = _DrawRect.x + _DrawRect.z;
                float b = _DrawRect.y;
                float t = _DrawRect.y + _DrawRect.w;
                
                if(fragCoord.x - l <= 0.5f * texSize.x)
                    sampleCoord.x = fragCoord.x - l;
                else if(r - fragCoord.x <= 0.5f * texSize.x)
                    sampleCoord.x = texSize.x - r + fragCoord.x;
                else
                    sampleCoord.x = texSize.x * 0.5f;
                
                if(fragCoord.y - b <= 0.5f * texSize.y)
                    sampleCoord.y = fragCoord.y - b;
                else if(t - fragCoord.y <= 0.5f * texSize.y)
                    sampleCoord.y = texSize.y - t + fragCoord.y;
                else
                    sampleCoord.y = texSize.y * 0.5f;
                
                float2 normSampleCoord = sampleCoord / texSize;
                float4 color = tex2D(_MainTex, normSampleCoord) * IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldCoord.xy, _ClipRect);
                #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                
                return color;
            }
        ENDCG
        
        }
    }
}
