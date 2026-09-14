using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct MovePosX : IJobEntity
{
    public float dt;
    public void Execute(ref LocalTransform trans)
    {
        trans.Position += new float3(1,0,0) * dt;
    }
}
