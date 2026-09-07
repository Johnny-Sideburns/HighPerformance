using Unity.Entities;
using UnityEngine;

public class RandomDataBaker : MonoBehaviour
{
    public float _speed;
    
    class baker : Baker<RandomDataBaker>
    {
        public override void Bake(RandomDataBaker authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<MyComp>(entity, new MyComp
            {
                speed = authoring._speed,
            });
            AddComponent<tag1>(entity);
        }
        
    }
    
}

public struct MyComp : IComponentData
{
    public float speed;
}

public struct tag1 : IComponentData
{
    
}
