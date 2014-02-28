// Oculus Rift barrel distortion shader adapted from Oculus Word Demo.

sampler2D input : register(s0);
float eyeMode: register(C0);

static const float2 ScreenCenter = float2(0.5f,0.5f);
static const float2 Scale = float2(0.291612267f, 0.233289823f);
static const float2 ScaleIn = float2(2.0f, 2.5f);
static const float4 HmdWarpParam = float4(1.0f, 0.22f, 0.24f, 0.0f);
static const float	lenseOffset = 0.038f;
static const float4 ChromAbParam = float4(0.996f, -0.004f, 1.014f, 0.0f);

// Scales input texture coordinates for distortion.
// ScaleIn maps texture coordinates to Scales to ([-1, 1]), although top/bottom will be
// larger due to aspect ratio.
float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 LensCenter = ScreenCenter - float2(lenseOffset*eyeMode,0.0f);
	float2 theta = (uv - LensCenter) * ScaleIn; // Scales to [-1, 1]
	float  rSq = theta.x * theta.x + theta.y * theta.y;
	float2 theta1 = theta * (
		HmdWarpParam.x +
		HmdWarpParam.y * rSq +
		HmdWarpParam.z * rSq * rSq +
		HmdWarpParam.w * rSq * rSq * rSq
		);
	
	// Do green lookup (no scaling).
	float2 tcGreen = LensCenter + Scale * theta1;

	// Detect whether unscaled texture coordinates are out of range
	if ( any( clamp(tcGreen, float2(0,0), float2(1, 1)) - tcGreen ) )
	{
		return 0;
	}
	
	float4 greenColor = tex2D( input, tcGreen );
	float  green = greenColor.g;
	float  alpha = greenColor.a;
	
	// Now do blue texture lookup.
	float2 thetaBlue = theta1 * (ChromAbParam.z + ChromAbParam.w * rSq);
	float2 tcBlue = LensCenter + Scale * thetaBlue;
	float  blue = tex2D( input, tcBlue ).b;
	
	// Do red scale and lookup.
	float2 thetaRed = theta1 * (ChromAbParam.x + ChromAbParam.y * rSq);
	float2 tcRed = LensCenter + Scale * thetaRed;
	float  red = tex2D( input, tcRed ).r;
	
	return float4(red, green, blue, alpha);
}