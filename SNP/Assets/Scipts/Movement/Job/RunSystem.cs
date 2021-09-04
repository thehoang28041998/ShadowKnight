using Leopotam.EcsLite;
using Scipts.Helper;
using Scipts.Movement.Component;

namespace Scipts.Movement.Job {
    public class RunSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<VelocityComponent> pool1;
        private EcsPool<RunComponent> pool2;
        
        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<VelocityComponent>().Inc<RunComponent>().End();
            pool1 = world.GetPool<VelocityComponent>();
            pool2 = world.GetPool<RunComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var velocity = ref pool1.Get(entity);
                ref var run = ref pool2.Get(entity);

                if (!run.IsFinish) {
                    velocity.velocity += run.GetDirection(FrameHelper.TIME_DELTA);
                }
            }
        }
    }
}