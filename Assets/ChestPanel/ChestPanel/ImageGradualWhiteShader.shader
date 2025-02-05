Shader "Custom/ImageGradualWhiteShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Slider("Slider", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Slider;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                half3 c = color.rgb + half3(1, 1, 1) * _Slider;
                return half4(c, color.a);
            }
            ENDCG
        }
    }
}