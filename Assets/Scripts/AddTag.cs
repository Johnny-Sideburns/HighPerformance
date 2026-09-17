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
    public ComponentType addTag;
    public ComponentType removeTag;
    public void Execute(Entity entity)
    {
        ecb.RemoveComponent(entity, removeTag);
        ecb.AddComponent(entity, addTag);
    }
}
