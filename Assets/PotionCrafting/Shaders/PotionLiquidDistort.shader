Shader "Unlit/PotionLiquidDistort"
{
    Properties
    {
        // Color property for material inspector, default to white
        _Color ("Main Color", Color) = (1,1,1,1)
        _Noise ("Noise", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			sampler2D _Noise;

            // vertex shader
            // this time instead of using "appdata" struct, just spell inputs manually,
            // and instead of returning v2f struct, also just return a single output
            // float4 clip position
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
				float4 texcoord = float4( 0, 0, 10, 0 );
				texcoord.x = ( sin( _Time * 100 ) / 100.0f );
				texcoord.y = ( sin( _Time * 100 ) / 100.0f );
				float2 tex = tex2Dlod(_Noise, texcoord);
				tex.x += 0.9f;
				tex.y += 0.9f;
				vertex.x *= tex.x;
				vertex.y *= tex.y;
				vertex.z *= tex.x;
                return mul(UNITY_MATRIX_MVP, vertex);
            }
            
            // color from the material
            fixed4 _Color;

            // pixel shader, no inputs needed
            fixed4 frag () : SV_Target
            {
                return _Color; // just return it
            }
            ENDCG
        }
    }
}