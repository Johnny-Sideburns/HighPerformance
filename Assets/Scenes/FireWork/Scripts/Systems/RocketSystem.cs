using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity;
using UnityEngine;

partial struct RocketSystem : ISystem
{
    bool _initiated;
    float _gravity;
    Unity.Mathematics.Random _random;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _initiated = false;
        _gravity = 9.82f;
        _random = new Unity.Mathematics.Random(123124);
        state.RequireForUpdate<RocketSpawner>();

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = (float)SystemAPI.Time.DeltaTime;
        var spawner = SystemAPI.GetSingletonRW<RocketSpawner>();
        //var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntityCommandBuffer ecb = SystemAPI.GetSingleton <BeginSimulationEntityCommandBufferSystem .Singleton>().CreateCommandBuffer (state.WorldUnmanaged);

        foreach(var (rocket, velocity, trans, entity) in SystemAPI.Query<RefRW<Rocket>, RefRW<Vel>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            if (rocket.ValueRO.blown) continue;
            //check the fuse if there still is stuff left continue..
            if(rocket.ValueRO.fuse > 0)
            {
                rocket.ValueRW.fuse = rocket.ValueRO.fuse - dt <= 0? 0: rocket.ValueRO.fuse -dt;
                continue;
            }

            if(rocket.ValueRO.secondaryFuse > 0)
            {
                rocket.ValueRW.secondaryFuse = rocket.ValueRO.secondaryFuse - dt <= 0? 0: rocket.ValueRO.secondaryFuse -dt;
            } else
            {
                ecb.AddComponent<ToExplode>(entity);
                rocket.ValueRW.blown = true;
                /*
                var col = _random.NextInt(0,5);
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
                for (int i = 0; i < rocket.ValueRW.flares; i++)
                {


                    Entity flare = state.EntityManager.Instantiate(spawner.ValueRO.flare);
                    state.EntityManager.SetComponentData(flare, LocalTransform.FromPosition(trans.ValueRO.Position));
                    var flaredata = state.EntityManager.GetComponentData<Flare>(flare);
                    flaredata.hangTime = 2f;
                    state.EntityManager.SetComponentData(flare, flaredata);
                    state.EntityManager.SetComponentData(flare, new Vel
                    {
                       resistance = 4f,
                       velocity = new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 20 + velocity.ValueRO.velocity 
                    });
                    state.EntityManager.SetComponentData(
                    flare,
                    new MyBaseColor
                    {
                        color = newColour
                    });
                }
                velocity.ValueRW.velocity += new float3(_random.NextFloat(-1,1),_random.NextFloat(-1,1),_random.NextFloat(-1,1)) * 10;
                */
                
            }

            if (rocket.ValueRO.currentPower > 15)
            {
                if (_random.NextFloat(0,1) > 0.8 )
                {
                    quaternion randomOffset = quaternion.Euler(
                        _random.NextFloat(-0.1f, 0.1f),
                        _random.NextFloat(-0.1f, 0.1f),
                        _random.NextFloat(-0.1f, 0.1f)
                        );

                        trans.ValueRW.Rotation = math.mul(trans.ValueRO.Rotation, randomOffset);
                    
                }
            }


            //if the fire up time is past it should fire down instead
            if(rocket.ValueRW.fireUpTime > 0)
            {
                rocket.ValueRW.fireUpTime = rocket.ValueRO.fireUpTime -dt <= 0? 0: rocket.ValueRO.fireUpTime -dt;
                rocket.ValueRW.currentPower += rocket.ValueRO.fireUpPower * dt;
            }
            else
            {
                rocket.ValueRW.currentPower += -rocket.ValueRO.fireDownPower * dt;
            }

            if (rocket.ValueRO.currentPower <= 0) continue;
            //add to the velocity
            velocity.ValueRW.velocity += dt * rocket.ValueRO.currentPower * trans.ValueRO.Up();
            //Debug.Log(rocket.ValueRO.currentPower + ", " + rocket.ValueRW.fireUpTime);
            rocket.ValueRW.lastExhaust += dt;
            if (rocket.ValueRO.lastExhaust > rocket.ValueRO.exhaustInterval)
            {
                //spawn exhaust flares at random dependant on current power
                if (rocket.ValueRO.currentPower > _random.NextFloat(0, 20))
                {
                    float3 positionAdjustment = new float3(_random.NextFloat(-1f,1f), -1.8f,_random.NextFloat(-1f,1f));
                    Entity flare = state.EntityManager.Instantiate(spawner.ValueRO.flare);
                    state.EntityManager.SetComponentData(flare, LocalTransform.FromPosition(trans.ValueRO.Position + math.rotate(trans.ValueRO.Rotation, positionAdjustment)));
                    rocket.ValueRW.lastExhaust = 0;
                }
            }
        }

        //flares burn out...
        foreach (var (trans, flare, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Flare>>().WithEntityAccess())
        {
            if (flare.ValueRO.hangTime <= 0)
            {
                flare.ValueRW.size -= flare.ValueRO.shrinkage *dt;
                if (flare.ValueRO.size <= 0.001)
                {
                    ecb.DestroyEntity(entity);
                    continue;
                }
                trans.ValueRW.Scale = flare.ValueRO.size;
                continue;
            }
            flare.ValueRW.hangTime -= dt;
        }
        
        /*
        new ExplodeRocket
        {
          ecb = ecb,
          spawner = spawner
        }.Schedule();
        */

        new ApplyVelocity
        {
            dt = dt,
            _gravity = _gravity
        }.ScheduleParallel();
        
        //ecb.Playback(state.EntityManager);

        //ecb.Dispose();
        //apply velocity
        /*
        foreach (var (trans, thing) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Vel>>())
        {

            //reduce speed with at flat value
            float speed = math.length(thing.ValueRW.velocity);
            float newSpeed = math.max(0f, speed - thing.ValueRO.resistance * dt);
            thing.ValueRW.velocity = math.normalizesafe(thing.ValueRW.velocity) * newSpeed;

            //if (math.length(thing.ValueRO.velocity) < 0.001f) continue;
            trans.ValueRW.Position += thing.ValueRO.velocity * dt;
            trans.ValueRW.Position.y = trans.ValueRW.Position.y < 0? 0 : trans.ValueRW.Position.y;

            thing.ValueRW.velocity.y = trans.ValueRW.Position.y <= 0? 0 : thing.ValueRO.velocity.y - _gravity *dt;
        }
        */

        // Initiation of rockets... will only happen at first
        if (_initiated) return;
        float spacing = 1.5f;
            for (int c = 0; c < spawner.ValueRO.rocketColumns; c++)
            {
                for (int r = 0; r < spawner.ValueRO.rocketRows; r++)
                {
                    float row = r % 2 == 0? r*spacing : -r *spacing -spacing;
                    float col = c * spacing;
                    
                    Entity rocket = state.EntityManager.Instantiate(spawner.ValueRO.rocket);
                    state.EntityManager.SetComponentData(rocket, LocalTransform.FromPosition(new float3(row, 0, col)));
                    var rocketData = state.EntityManager.GetComponentData<Rocket>(rocket);
                    rocketData.fuse += _random.NextFloat(-4f,4f);
                    rocketData.secondaryFuse += _random.NextFloat(-1f,1f);
                    state.EntityManager.SetComponentData(rocket, rocketData);
                    
                }
            }
        _initiated = true;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
