using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;
using Leopotam.EcsLite.Threads.Unity;
using Movement.Component;
using UserInput.Component;

namespace Movement.Job {
    public class VelocityJobSystem : EcsUnityJobSystem<VelocityJob, VelocityComponent, InputComponent> {
        protected override int GetChunkSize(EcsSystems systems) {
            return Environment.ProcessorCount;
        }

        protected override EcsFilter GetFilter(EcsWorld world) {
            EcsFilter filter = world.Filter<VelocityComponent>().Inc<InputComponent>().End();
            return filter;
        }

        protected override EcsWorld GetWorld(EcsSystems systems) {
            return systems.GetWorld();
        }
    }
}