using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity;
using UnityEngine;
[BurstCompile]
public partial struct ApplyVelocity : IJobEntity
{
    public float dt;
    public float _gravity;
    public void Execute(ref LocalTransform trans, ref Vel thing)
    {

        //reduce speed with at flat value
        float speed = math.length(thing.velocity);
        float newSpeed = math.max(0f, speed - thing.resistance * dt);
        thing.velocity = math.normalizesafe(thing.velocity) * newSpeed;

        trans.Position += thing.velocity * dt;
        trans.Position.y = trans.Position.y < 0? 0 : trans.Position.y;

        thing.velocity.y = trans.Position.y <= 0? 0 : thing.velocity.y - _gravity *dt;
    }
}
