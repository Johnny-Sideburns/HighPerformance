
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity;
using UnityEngine;
partial struct whatever : ISystem
{
    float dumbtime;
    bool addTag;
    bool removeTag;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       //state.RequireForUpdate<tag1>();
        dumbtime = 0f;
        addTag = true;
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb =
        SystemAPI.GetSingleton <BeginSimulationEntityCommandBufferSystem .Singleton>()
        .CreateCommandBuffer (state.WorldUnmanaged );
        var elapsedTime = (float)SystemAPI.Time.ElapsedTime;
        float dt = (float)SystemAPI.Time.DeltaTime;
        dumbtime += dt;
        if (dumbtime > 1)
        {
            addTag = addTag == false;
        }

        var job1 = new MovePosX
        {
            dt = dt
        }.Schedule(state.Dependency);

        var job2 = new MovePosY
        {
            dt = dt
        }.Schedule(job1);
        
        var job3 = new AddTag
        {
            elapsedTime = dumbtime,
            ecb = ecb,
            addTag = addTag
        }.Schedule(job2);

        state.Dependency = job3;
        dumbtime = dumbtime > 1? 0 : dumbtime;

        //RefRW - read and write data
        // RefRO - Readonly data
        /*
        foreach (var (trans, myComp) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MyComp>>().WithAll<tag1>())
        {
            var waveMovement = myComp.ValueRO.speed * math.sin(elapsedTime * myComp.ValueRO.speed);
            //var waveMovement = myComp.ValueRO.speed * elapsedTime + trans.ValueRW.Position.y;
            trans.ValueRW.Position = new float3(trans.ValueRO.Position.x, waveMovement, trans.ValueRO.Position.z);
        }

        foreach (var (trans, myComp) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MyComp>>().WithNone<tag1>())
        {
            //var waveMovement = waveData.ValueRO.amplitude * math.sin(elapsedTime * waveData.ValueRO.frequency);
            var waveMovement = trans.ValueRW.Position.y - myComp.ValueRO.speed * elapsedTime;
            trans.ValueRW.Position = new float3(trans.ValueRO.Position.x, waveMovement, trans.ValueRO.Position.z);
        }
        */

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    
}
