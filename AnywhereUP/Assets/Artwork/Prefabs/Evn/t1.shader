// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0.7622456,fgcb:0.9632353,fgca:1,fgde:0.01,fgrn:8,fgrf:34.3,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:349,x:32958,y:32496,varname:node_349,prsc:2|emission-2934-RGB;n:type:ShaderForge.SFN_ScreenPos,id:2161,x:31422,y:32641,varname:node_2161,prsc:2,sctp:2;n:type:ShaderForge.SFN_Multiply,id:6996,x:32080,y:32496,varname:node_6996,prsc:2|A-1610-OUT,B-9458-OUT;n:type:ShaderForge.SFN_Fresnel,id:586,x:31627,y:32777,varname:node_586,prsc:2|EXP-3870-OUT;n:type:ShaderForge.SFN_OneMinus,id:8488,x:31923,y:32793,varname:node_8488,prsc:2|IN-586-OUT;n:type:ShaderForge.SFN_Power,id:9458,x:32174,y:32832,varname:node_9458,prsc:2|VAL-8488-OUT,EXP-1098-OUT;n:type:ShaderForge.SFN_Slider,id:5575,x:31726,y:33034,ptovrint:False,ptlb:node_5575,ptin:_node_5575,varname:node_5575,prsc:2,min:0,cur:2.102564,max:3;n:type:ShaderForge.SFN_Slider,id:3870,x:31190,y:32843,ptovrint:False,ptlb:1,ptin:_1,varname:node_3870,prsc:2,min:0,cur:2,max:2;n:type:ShaderForge.SFN_Add,id:1134,x:32390,y:32621,varname:node_1134,prsc:2|A-6996-OUT,B-2161-UVOUT;n:type:ShaderForge.SFN_SceneColor,id:2934,x:32673,y:32770,varname:node_2934,prsc:2|UVIN-1134-OUT;n:type:ShaderForge.SFN_Vector1,id:1098,x:32007,y:32985,varname:node_1098,prsc:2,v1:1;n:type:ShaderForge.SFN_OneMinus,id:603,x:32586,y:32496,varname:node_603,prsc:2|IN-1134-OUT;n:type:ShaderForge.SFN_RemapRange,id:1610,x:31739,y:32508,varname:node_1610,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-2161-UVOUT;proporder:5575-3870;pass:END;sub:END;*/

Shader "Shader Forge/t1" {
    Properties {
        _node_5575 ("node_5575", Range(0, 3)) = 2.102564
        _1 ("1", Range(0, 2)) = 2
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _1;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float2 node_1134 = (((sceneUVs.rg*2.0+-1.0)*pow((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_1)),1.0))+sceneUVs.rg);
                float3 emissive = tex2D(_GrabTexture, node_1134).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
