using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct whatever : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        /*
        var e = state.EntityManager.CreateEntity();
        state.EntityManager.SetName(e, "myEntity");
        */
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var elapsedTime = (float)SystemAPI.Time.ElapsedTime;
        //RefRW - read and write data
        // RefRO - Readonly data
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
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
