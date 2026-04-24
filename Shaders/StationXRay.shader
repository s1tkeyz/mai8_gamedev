Shader "Custom/StationXRay"
{
    Properties
    {
        _HullColor ("Цвет обшивки", Color) = (0.12, 0.14, 0.18, 1)
        _PanelColor ("Цвет стыков панелей", Color) = (0.04, 0.05, 0.07, 1)
        _WireColor ("Цвет каркаса", Color) = (0, 1, 0.85, 1)
        _ScannerActive ("Активность (0/1)", Float) = 0
        _GridSize ("Размер ячейки (м)", Float) = 2.0
        _WireThickness ("Толщина линии каркаса", Range(0.01, 0.4)) = 0.12
        _ScanOpacity ("Прозрачность при скане", Range(0.05, 0.7)) = 0.25
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _HullColor;
                float4 _PanelColor;
                float4 _WireColor;
                float _ScannerActive;
                float _GridSize;
                float _WireThickness;
                float _ScanOpacity;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.worldPos = TransformObjectToWorld(v.vertex);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // 1. Подготовка данных
                float3 normalWS = normalize(i.normalWS);
                float3 viewDir  = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 lightDir = normalize(float3(0.6, 0.8, 0.3)); // Имитация направленного света
                float NdotL = saturate(dot(normalWS, lightDir));

                // 2. Эффект обшивки (панели + стыки + блик)
                float diff = NdotL * 0.7 + 0.3; // Мягкое освещение
                float3 reflectDir = reflect(-lightDir, normalWS);
                float spec = pow(saturate(dot(viewDir, reflectDir)), 48) * 0.35;

                // Панельная сетка (тонкие стыки)
                float3 p = i.worldPos / _GridSize;
                float3 dx = fwidth(p);
                dx = max(dx, 0.001); // Защита от деления на ноль
                float3 f = abs(frac(p) - 0.5);
                float panelLine = 1.0 - smoothstep(0.0, 0.06, min(min(f.x/dx.x, f.y/dx.y), f.z/dx.z));
                
                float3 hullBase = _HullColor.rgb * diff + _PanelColor.rgb * panelLine * 0.5 + _PanelColor.rgb * spec;

                // 3. Каркас сканера (яркие линии)
                float lineDist = min(min(f.x / dx.x, f.y / dx.y), f.z / dx.z);
                float wireframe = 1.0 - smoothstep(0.0, _WireThickness, lineDist);

                // 4. Мгновенное переключение цвета и прозрачности
                float isScanning = _ScannerActive;
                float alpha = lerp(1.0, _ScanOpacity, isScanning);
                float3 finalColor = lerp(hullBase, _WireColor.rgb, wireframe * isScanning);

                return half4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
}