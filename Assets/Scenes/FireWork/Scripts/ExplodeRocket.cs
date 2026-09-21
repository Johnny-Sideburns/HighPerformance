using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[WithAll(typeof(ToExplode))]
//[BurstCompile]
public partial struct ExplodeRocket : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public RocketSpawner spawner;
    public Random _random;
    public void Execute([ChunkIndexInQuery] int sortKey, ref LocalTransform trans, ref Rocket rocket, ref Vel velocity, Entity entity)
    {

                var newColour = float4.zero;
                switch (_random.NextInt(0,5))
                {
                    case 0:
                        newColour = new float4(1,0,0,1);
                        break;
                    case 1:
                        newColour = new float4(1,0,1,1);
                        break;
                    case 2:
                        newColour = new float4(0,0,1,1);
                        break;
                    case 3:
                        newColour = new float4(1,1,0,1);
                        break;
                    case 4:
                        newColour = new float4(0,1,0,1);
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < rocket.flares; i++)
                {


                    Entity flare = ecb.Instantiate(sortKey, spawner.flare);
                    ecb.AddComponent(sortKey, flare, LocalTransform.FromPosition(trans.Position));
                    ecb.AddComponent(sortKey, flare, new Flare
                    {
                        hangTime = 2f,
                        size = 0.8f,
                        shrinkage = 0.8f
                    });
                    ecb.AddComponent(sortKey, flare, new Vel
                    {
                       resistance = 8f,
                       velocity = new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 20 + velocity.velocity 
                    });
                    ecb.AddComponent(sortKey, flare, new MyBaseColor
                    {
                        color = newColour
                    });
                }
                velocity.velocity += new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 10;
                ecb.RemoveComponent<ToExplode>(sortKey, entity);
    }
}
