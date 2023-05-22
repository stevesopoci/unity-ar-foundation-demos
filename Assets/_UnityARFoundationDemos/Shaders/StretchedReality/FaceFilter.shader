Shader "StretchedReality/FaceFilter"
{
    Properties
    {
        _FaceFilterTex ("Main Source Texture", 2D) = "white" {} 
        _FaceFilterTexST("Main Source Texture Scale (XY) Translate (ZW)", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opqaque" }
        LOD 200
		Cull Off

        CGPROGRAM
        // Use the physically based Standard lighting model and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use the shader target model 5.0 to achieve better lighting
        #pragma target 5.0

		sampler2D _FaceFilterTex;
		fixed4 _FaceFilterTexST;

        struct Input
        {
            float2 uv_FaceFilterTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 color = tex2D(_FaceFilterTex, IN.uv_FaceFilterTex);
            o.Albedo = color.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}