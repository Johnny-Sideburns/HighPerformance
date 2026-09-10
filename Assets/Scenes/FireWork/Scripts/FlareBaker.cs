using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class FlareBaker : MonoBehaviour
{
    public float _resistance;
    public float _size;
    public float _shrinkage;
    public float _hangTime;

    class baker: Baker<FlareBaker>
    {
        public override void Bake(FlareBaker authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Flare
            {
                size = authoring._size,
                shrinkage = authoring._shrinkage,
                hangTime = authoring._hangTime
            });
            AddComponent(entity, new Vel
            {
                velocity = float3.zero,
                resistance = authoring._resistance
            });
        }
    }
}

public struct Flare : IComponentData{
    public float size;
    public float shrinkage;
    public float hangTime;
}