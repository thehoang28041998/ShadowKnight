using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.FiniteStateMachine.Component;

namespace Scipts.FiniteStateMachine.Job {
    public class DashStateJobSystem : EcsUnityJobSystem<DashStateJob, DashStateComponent> {
        protected override int GetChunkSize(EcsSystems systems) {
            return Environment.ProcessorCount;
        }

        protected override EcsFilter GetFilter(EcsWorld world) {
            EcsFilter filter = world.Filter<DashStateComponent>().End();
            return filter;
        }

        protected override EcsWorld GetWorld(EcsSystems systems) {
            return systems.GetWorld();
        }
    }
}