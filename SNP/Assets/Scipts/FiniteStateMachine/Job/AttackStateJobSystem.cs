using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.FiniteStateMachine.Component;

namespace Scipts.FiniteStateMachine.Job {
    public class AttackStateJobSystem : EcsUnityJobSystem<AttackStateJob, AttackStateComponent> {
        protected override int GetChunkSize(EcsSystems systems) {
            return Environment.ProcessorCount;
        }

        protected override EcsFilter GetFilter(EcsWorld world) {
            EcsFilter filter = world.Filter<AttackStateComponent>().End();
            return filter;
        }

        protected override EcsWorld GetWorld(EcsSystems systems) {
            return systems.GetWorld();
        }
    }
}