using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;

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
            Color _color = authoring.GetComponent<Renderer>().sharedMaterial.color;
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
            AddComponent(entity, new MyBaseColor
            {
                color = new float4(_color.r,_color.g,_color.b,_color.a)
            });
        }
    }
}

public struct Flare : IComponentData{
    public float size;
    public float shrinkage;
    public float hangTime;
}

[MaterialProperty("_BaseColor")]
public struct MyBaseColor : IComponentData
{
    public float4 color;
}