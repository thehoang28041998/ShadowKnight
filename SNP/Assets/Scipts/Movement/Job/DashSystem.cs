using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.Helper;
using Scipts.Movement.Component;

namespace Scipts.Movement.Job {
    public class DashSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<VelocityComponent> pool1;
        private EcsPool<DashComponent> pool2;
        
        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<VelocityComponent>().Inc<RunComponent>().End();
            pool1 = world.GetPool<VelocityComponent>();
            pool2 = world.GetPool<DashComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var velocity = ref pool1.Get(entity);
                ref var dash = ref pool2.Get(entity);

                if (!dash.IsFinish) {
                    velocity.velocity += dash.GetDirection(FrameHelper.TIME_DELTA);
                }
            }
        }
    }
}