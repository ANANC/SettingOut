Shader "Unlit/Sprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", color) = (1, 1 ,1, 1)
		_Brightness("Brightness",Float)=1
		_MaxBrightness("MaxBrightness",Float) = 1
		_MinBrightness("MinBrightness",Float) = 1
		_Frequency("Frequency",Float)=1
		_NoiseTex("Texture",2D) = "white"{}
		_XSpeed("Speed",Float)=0
		_YSpeed("Speed",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Always
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 worldpos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float4 _Color;
			float _Brightness;
			float _MaxBrightness;
			float _MinBrightness;
			float _Frequency;
			float _XSpeed;
			float _YSpeed;

            v2f vert (appdata v)
            {
                v2f o;
				float4 clipPos = UnityObjectToClipPos(v.vertex);
                o.vertex = clipPos;
				o.worldpos.xyz = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.screenPos = ComputeGrabScreenPos(clipPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _NoiseTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float2 offset = float2(_Time.y*_XSpeed,_Time.y*_YSpeed);
				fixed4 noisevar = tex2D(_NoiseTex,i.uv2+offset);
				fixed4 noiseScreen = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_NoiseTex,i.screenPos/i.screenPos.w);
				fixed4 col = tex2D(_MainTex, i.uv) * _Color*lerp(_MinBrightness,_MaxBrightness,saturate(sin(_Time.y*_Frequency+ noiseScreen.r*10)))*_Brightness;
				col = col* noisevar.r;
				fixed alpha= tex2D(_MainTex, i.uv).a * _Color.a;
                return fixed4(col.rgb,alpha);
            }
            ENDCG
        }
    }
}
