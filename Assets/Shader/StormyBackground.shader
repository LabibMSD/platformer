Shader "UI/StormyBackground"
{
    Properties
    {
        _Color1 ("Deep Color", Color) = (0.05, 0.05, 0.15, 1) // Dark Blue/Black
        _Color2 ("Highlight Color", Color) = (0.2, 0.1, 0.4, 1) // Purple/Stormy
        _Speed ("Wind Speed", Float) = 0.2
        _Scale ("Cloud Scale", Float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100

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

            fixed4 _Color1;
            fixed4 _Color2;
            float _Speed;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Pseudo-random function
            float random (float2 st) {
                return frac(sin(dot(st.xy, float2(12.9898,78.233)))*43758.5453123);
            }

            // 2D Noise
            float noise (float2 st) {
                float2 i = floor(st);
                float2 f = frac(st);

                // Four corners
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x) +
                        (c - a)* u.y * (1.0 - u.x) +
                        (d - b) * u.x * u.y;
            }

            // Fractal Brownian Motion for Storm Clouds
            float fbm (float2 st) {
                float v = 0.0;
                float a = 0.5;
                float2 shift = float2(100.0, 100.0);
                // Rotate to reduce axial bias
                float2x2 rot = float2x2(cos(0.5), sin(0.5),
                                        -sin(0.5), cos(0.50));
                for (int i = 0; i < 5; ++i) {
                    v += a * noise(st);
                    st = mul(rot, st) * 2.0 + shift;
                    a *= 0.5;
                }
                return v;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize UV to avoid stretching if possible, or just use raw UV
                float2 st = i.uv * _Scale;
                float time = _Time.y * _Speed;
                
                // Domain Wrapping to create fluid motion
                float2 q = float2(0.0, 0.0);
                q.x = fbm( st + 0.00*time);
                q.y = fbm( st + float2(1.0, 1.0));

                float2 r = float2(0.0, 0.0);
                r.x = fbm( st + 1.0*q + float2(1.7,9.2)+ 0.15*time );
                r.y = fbm( st + 1.0*q + float2(8.3,2.8)+ 0.126*time);

                float f = fbm(st+r);

                // Mix colors based on noise value
                // Use power function to increase contrast (make the storm darker/sharper)
                float mixFactor = pow(f, 1.5); 
                fixed4 color = lerp(_Color1, _Color2, mixFactor);
                
                // Subtle bright aesthetic lines/lightning feel?
                // For now, keep it moody and gradients.
                
                return color;
            }
            ENDCG
        }
    }
}
