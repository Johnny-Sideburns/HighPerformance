using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct AddTag : IJobEntity
{
    public float elapsedTime;
    public EntityCommandBuffer ecb;
    public bool addTag;
    public void Execute(ref LocalTransform trans, Entity entity)
    {
        if (elapsedTime > 1)
        {
            if (addTag)
            {
                ecb.AddComponent<tag1>(entity);
            } else
            {
                ecb.RemoveComponent<tag1>(entity);
            }

        }
    }
}
