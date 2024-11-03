Shader "Custom/LowPolyWater"
{
    Properties
    {
        _WaveStrength ("Wave Strength", Range(0, 2)) = 0.1
        _WaveSpeed ("Wave Speed", Range(0.0, 1.0)) = 0.5
        _WaveDirection ("Wave Direction", Vector) = (1, 0, 0, 0)
        _WaveHeight ("Wave Height", Range(0, 1)) = 0.1
        _ShallowColor ("Shallow Color", Color) = (0.2, 0.6, 0.8, 1)
        _DeepColor ("Deep Color", Color) = (0.1, 0.2, 0.4, 1)
    }

    SubShader
    {
        Tags { "Queue" = "Geometry" }
        LOD 200

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
                float4 vertex : SV_POSITION;
            };

            float4 _ShallowColor;
            float4 _DeepColor;
            half _WaveStrength;
            half _WaveSpeed;
            half4 _WaveDirection;
            half _WaveHeight;

            // Função para mover os vértices com base em coordenadas mundiais
            v2f vert(appdata v)
            {
                v2f o;
                
                // Converter a posição do vértice para coordenadas mundiais
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calcular o deslocamento das ondas usando coordenadas XZ do mundo
                float waveX = sin(worldPos.x * _WaveStrength + _Time.y * _WaveSpeed) * _WaveHeight;
                float waveZ = sin(worldPos.z * _WaveStrength + _Time.y * _WaveSpeed) * _WaveHeight;

                // Aplicar o deslocamento no eixo Y
                v.vertex.y += waveX + waveZ;

                // Transformar a posição do vértice para o espaço de tela
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Interpolação de cor baseada na profundidade
                return lerp(_DeepColor, _ShallowColor, saturate(i.vertex.y));
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
