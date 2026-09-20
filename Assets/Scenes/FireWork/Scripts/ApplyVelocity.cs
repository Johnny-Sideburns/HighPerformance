using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct ApplyVelocity : IJobEntity
{
    public float dt;
    public float _gravity;
    public void Execute(ref LocalTransform trans, ref Vel vel)
    {

        //reduce speed with at flat value
        float speed = math.length(vel.velocity);
        float newSpeed = math.max(0f, speed - vel.resistance * dt);
        vel.velocity = math.normalizesafe(vel.velocity) * newSpeed;

        trans.Position += vel.velocity * dt;
        trans.Position.y = trans.Position.y < 0? 0 : trans.Position.y;

        vel.velocity.y = trans.Position.y <= 0? 0 : vel.velocity.y - _gravity *dt;
    }
}
