using Leopotam.EcsLite;
using Scipts.FiniteStateMachine.Component;
using Scipts.Helper;

namespace Scipts.FiniteStateMachine.Job {
    public class AttackStateSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<StateMachineComponent> pool1;
        private EcsPool<AttackStateComponent> pool2;
        
        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<StateMachineComponent>().Inc<IdleStateComponent>().End();
            pool1 = world.GetPool<StateMachineComponent>();
            pool2 = world.GetPool<AttackStateComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                pool2.Get(entity).Run(ref pool1.Get(entity), entity, FrameHelper.TIME_DELTA);
            }
        }
    }
}