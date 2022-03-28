using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class LifetimeSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimulationECBSystem;

    protected override void OnCreate()
    {
        endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {        
        float deltaTime = Time.DeltaTime;
        var ecb = endSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();
        
        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => 
        {
            lifetime.Value -= deltaTime;

            if(lifetime.Value <= 0f)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).ScheduleParallel();

        endSimulationECBSystem.AddJobHandleForProducer(Dependency);
    }
}
