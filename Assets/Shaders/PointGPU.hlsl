// Each #kernel tells which function to compile; you can have many kernels
#pragma kernel FunctionKernel
#define PI 3.14159265358979323846
RWStructuredBuffer<float3> _Positions;

uint _Resolution;

float _Step, _Time;

float2 GetUV (uint3 id) {
	return (id.xy + 0.5) * _Step - 1.0;
}

void SetPosition (uint3 id, float3 position) {
	if (id.x < _Resolution && id.y < _Resolution) {
		_Positions[id.x + id.y * _Resolution] = position;
	}
}

[numthreads(8, 8, 1)]

void FunctionKernel (uint3 id: SV_DispatchThreadID) {
    float2 uv = GetUV(id);
	SetPosition(id, Wave(uv.x, uv.y, _Time));
}

/**/