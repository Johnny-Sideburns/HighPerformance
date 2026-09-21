using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[WithAll(typeof(ToExplode))]
[BurstCompile]
public partial struct ExplodeRocket : IJobEntity
{
    public EntityCommandBuffer ecb;
    public RocketSpawner spawner;
    public void Execute(ref LocalTransform trans, ref Rocket rocket, ref Vel velocity, Entity entity)
    {

                var _random = new Random();
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


                    Entity flare = ecb.Instantiate(spawner.flare);
                    //ecb.SetComponentData(flare, LocalTransform.FromPosition(trans.Position));
                    ecb.AddComponent(flare, LocalTransform.FromPosition(trans.Position));
                    ecb.AddComponent(flare, new Flare
                    {
                        hangTime = 2f,
                        size = 0.8f,
                        shrinkage = 0.01f
                    });
                    /*
                    var flaredata = ecb.GetComponentData<Flare>(flare);
                    flaredata.hangTime = 2f;
                    ecb.SetComponentData(flare, flaredata);
                    ecb.SetComponentData(flare, new Vel
                    {
                       resistance = 4f,
                       velocity = new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 20 + velocity.velocity 
                    });
                    */
                    ecb.AddComponent(flare, new Vel
                    {
                       resistance = 4f,
                       velocity = new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 20 + velocity.velocity 
                    });
                    ecb.AddComponent(flare, new MyBaseColor
                    {
                        color = newColour
                    });
                }
                velocity.velocity += new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 10;
                ecb.RemoveComponent<ToExplode>(entity);
        /*
        */
    }
}
