/* -*- mode:Shader coding:utf-8-with-signature -*-
 */

/**
 * refered from http://kylehalladay.com/blog/tutorial/bestof/2013/10/13/Multi-Light-Diffuse.html
 *
 */
Shader "Custom/diffuse"
{
    Properties 
    {
        [NoScaleOffset] _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_Specular ("Specular", Range (0, 1)) = 1
		_OutlineWidth ("Outline", Range (0.1, 2.0)) = 0.5
    }
    SubShader 
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}
        
		ColorMask RGB
		
		pass 
		{
		    
		    
		    ZWrite Off
		    
		    CGPROGRAM
		    
		    #pragma vertex vert
		    #pragma fragment frag
		    
		    #include "UnityCG.cginc"
		    
		    struct appdata
		    {
		        float4 position : POSITION;
		        float3 normal : NORMAL;
		    };
		    
		    struct v2f
		    {
		        float4 position : SV_POSITION;
		    };
		    
		    float _OutlineWidth;
		    
		    v2f vert(appdata i)
		    {
		        v2f o;
		        i.position.xyz *= _OutlineWidth;
		        o.position = UnityObjectToClipPos(i.position);
		        return o;
		    }
		    
		    fixed4 frag(v2f i): COLOR
		    {
		        return float4(1,1,1,1);
		    }
		    
		    ENDCG
		}

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            
            ZWrite On

           	CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            
            struct vertex_input
            {
            	float4 vertex : POSITION;
            	float3 normal : NORMAL;
            	float2 texcoord : TEXCOORD0;
            	UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct vertex_output
            {
                float4 pos         : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 lightDir    : TEXCOORD1;
                float3 normal      : TEXCOORD2;
            };
            
            UNITY_INSTANCING_BUFFER_START(Props)
////        UNITY_DEFINE_INSTANCED_PROP(float4, _color)
            UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _MainTex;			
            fixed4 _LightColor0; 
			
            
            vertex_output vert (vertex_input v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                vertex_output o;
                o.pos = UnityObjectToClipPos( v.vertex);
                o.uv = v.texcoord.xy;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.normal = v.normal;
                return o;
            }
            
            fixed4 frag(vertex_output i) : COLOR
            {
                i.lightDir = normalize(i.lightDir);
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed diff = saturate(dot(i.normal, i.lightDir));
                fixed4 c = fixed4((UNITY_LIGHTMODEL_AMBIENT.rgb + tex.rgb), 1);
                c.rgb += (tex.rgb * _LightColor0.rgb * diff);
//                half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
                return c;
            }
            ENDCG
        }
    }
    FallBack "VertexLit"    // Use VertexLit's shadow caster/receiver passes.
}
/*
 * End of diffuse_reflection.shader
 */
