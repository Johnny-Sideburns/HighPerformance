using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WithAll(typeof(MoveLeftTag))]
[BurstCompile]
public partial struct MovePosNegX : IJobEntity
{
    public float dt;
    public void Execute(ref LocalTransform trans)
    {
        trans.Position += new float3(-1,0,0) * dt;
    }
}
