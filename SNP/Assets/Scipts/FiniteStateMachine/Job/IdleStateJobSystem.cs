using System;
using Leopotam.EcsLite;
using Scipts.FiniteStateMachine.Component;
using Leopotam.EcsLite.Threads.Unity;

namespace Scipts.FiniteStateMachine.Job {
    public class IdleStateJobSystem : EcsUnityJobSystem<IdleStateJob, IdleStateComponent> {
        protected override int GetChunkSize(EcsSystems systems) {
            return Environment.ProcessorCount;
        }

        protected override EcsFilter GetFilter(EcsWorld world) {
            EcsFilter filter = world.Filter<IdleStateComponent>().End();
            return filter;
        }

        protected override EcsWorld GetWorld(EcsSystems systems) {
            return systems.GetWorld();
        }
    }
}