// Oculus Rift barrel distortion shader adapted from Oculus Word Demo.

sampler2D input : register(s0);
float eyeMode: register(C0);

static const float2 ScreenCenter = float2(0.5f,0.5f);
static const float2 Scale = float2(0.291612267f, 0.233289823f);
static const float2 ScaleIn = float2(2.0f, 2.5f);
static const float4 HmdWarpParam = float4(1.0f, 0.22f, 0.24f, 0.0f);
static const float	lenseOffset = 0.038f;

float2 HmdWarp(float2 in01)
{
	float2 LensCenter = ScreenCenter - float2(lenseOffset*eyeMode,0.0f);
	float2 theta = (in01 - LensCenter) * ScaleIn; // Scales to [-1, 1]
	float rSq = theta.x * theta.x + theta.y * theta.y;
	float2 rvector = theta * (
		HmdWarpParam.x +
		HmdWarpParam.y * rSq +
		HmdWarpParam.z * rSq * rSq +
		HmdWarpParam.w * rSq * rSq * rSq
		);
	return LensCenter + Scale * rvector;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 tc = HmdWarp(uv);
	if ( any( clamp(tc, float2(0,0), float2(1, 1)) - tc ) )
	{
		return 0;
	}
	return tex2D( input, tc );
};