
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity;
using UnityEngine;
using Unity.Collections;
partial struct whatever : ISystem
{
    float dumbtime;
    private FixedList128Bytes<ComponentType> tags;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<tag1>();
        dumbtime = 0f;
        tags = new FixedList128Bytes<ComponentType> {ComponentType.ReadWrite<MoveRightTag>(), ComponentType.ReadWrite<MoveUpTag>(), ComponentType.ReadWrite<MoveLeftTag>(), ComponentType.ReadWrite<MoveDownTag>()};
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

            new AddTag
            {
                elapsedTime = dumbtime,
                ecb = ecb,
                addTag = tags[0],
                removeTag = tags[2]

            }.Schedule();
            var tmp = tags[0];
            tags.Remove(tmp);
            tags.Add(tmp);
        }
        dumbtime = dumbtime > 1? 0 : dumbtime;

        new MovePosX
        {
            dt = dt
        }.Schedule();

        new MovePosY
        {
            dt = dt
        }.Schedule();
        

        new MovePosNegX
        {
            dt = dt
        }.Schedule();

        new MovePosNegY
        {
            dt = dt
        }.Schedule();

        //state.Dependency = job2;

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