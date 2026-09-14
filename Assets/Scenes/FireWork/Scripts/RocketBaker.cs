using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class RocketBaker : MonoBehaviour
{
    public float _fuseMax;
    public float _fireUpTimeMax;
    public float _fireUpPower;
    public float _fireDownPower;
    public float _resistance;
    public float _secondaryFuse;
    public int _flares;
    public float _exhaustInterval;
    public float _lastExhaust;
    class baker: Baker<RocketBaker>
    {
        public override void Bake(RocketBaker authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Rocket
            {
                fuse = authoring._fuseMax,
                fireUpTime = authoring._fireUpTimeMax,
                fireUpPower = authoring._fireUpPower,
                fireDownPower = authoring._fireDownPower,
                currentPower = 0f,
                exhaustInterval = authoring._exhaustInterval,
                lastExhaust = 0,
                secondaryFuse = authoring._secondaryFuse,
                flares = authoring._flares,
                blown = false
            });
            AddComponent(entity, new Vel
            {
                velocity = float3.zero,
                resistance = authoring._resistance
            });        
        }
    }
}

public struct Rocket : IComponentData
{
    public float fuse;
    public float fireUpTime;
    public float fireUpPower;
    public float fireDownPower;
    public float currentPower;
    public float exhaustInterval;
    public float lastExhaust;
    public float secondaryFuse;
    public float flares;
    public bool blown;
}

public struct Vel : IComponentData
{
    public float3 velocity;
    public float resistance;
}