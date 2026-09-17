using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WithAll(typeof(MoveDownTag))]
[BurstCompile]
public partial struct MovePosNegY : IJobEntity
{
    public float dt;
    public void Execute(ref LocalTransform trans)
    {
        trans.Position += new float3(0,-1,0) * dt;
    }
}
