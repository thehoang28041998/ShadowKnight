using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.Movement.Component;

namespace Scipts.Movement.Job {
    public class DashJobSystem : EcsUnityJobSystem<DashJob, VelocityComponent, DashComponent> {
        protected override int GetChunkSize(EcsSystems systems) {
            return Environment.ProcessorCount;
        }

        protected override EcsFilter GetFilter(EcsWorld world) {
            EcsFilter filter = world.Filter<VelocityComponent>().Inc<DashComponent>().End();
            return filter;
        }

        protected override EcsWorld GetWorld(EcsSystems systems) {
            return systems.GetWorld();
        }
    }
}