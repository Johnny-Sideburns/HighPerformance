using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class FWSettings : MonoBehaviour
{
    public GameObject _rocketPrefab;
    public GameObject _flarePrefab;
    public int _rocketRows;
    public int _rocketColumns;
    
    class baker: Baker<FWSettings>
    {
        public override void Bake(FWSettings authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RocketSpawner
            {
                rocket = GetEntity(authoring._rocketPrefab , TransformUsageFlags.Dynamic),
                rocketRows = authoring._rocketRows,
                rocketColumns = authoring._rocketColumns,
                flare = GetEntity(authoring._flarePrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
    
}
public struct RocketSpawner : IComponentData
{
    public Entity rocket;
    public int rocketRows;
    public int rocketColumns;
    public Entity flare;
}
