#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture2D lightMaskTexture;
sampler sampler0;
sampler lightSampler : register(s1) = sampler_state 
{
	Texture = <lightMaskTexture>;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VSOutput
{
	float4 position				: SV_Position;
	float4 color				: COLOR0;
	float2 textureCoordinate	: TEXCOORD0;
};

float4 PixelShaderFunction(VSOutput input) : SV_TARGET0
{
	float4 color = tex2D(sampler0, input.textureCoordinate);
	float4 colorOfLight = tex2D(lightSampler, input.textureCoordinate);
	return color * colorOfLight;
}

technique LightTechnique
{
	pass LightPass
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}