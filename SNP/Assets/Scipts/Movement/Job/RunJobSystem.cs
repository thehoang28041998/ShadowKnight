using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;
using Leopotam.EcsLite.Threads.Unity;
using Movement.Component;
using UnityEngine;
using UserInput.Component;

namespace Movement.Job {
    public class RunJobSystem : EcsUnityJobSystem<RunJob, VelocityComponent, RunRequestComponent> {
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

        protected override void SetData(EcsSystems systems, ref RunJob job) {
            base.SetData(systems, ref job);
            job.deltaTime = Time.deltaTime;
        }
    }
}