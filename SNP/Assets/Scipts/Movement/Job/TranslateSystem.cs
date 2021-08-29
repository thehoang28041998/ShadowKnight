using Leopotam.EcsLite;
using Scipts.Movement.Component;
using UnityEngine;

namespace Scipts.Movement.Job {
    public struct TranslateSystem : IEcsRunSystem {
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<TranslateComponent>().Inc<VelocityComponent>().End();
            var translatePool = world.GetPool<TranslateComponent>();
            var velocityPool = world.GetPool<VelocityComponent>();

            foreach (var entity in filter) {
                ref var translateComponent = ref translatePool.Get(entity);
                ref var velocityComponent = ref velocityPool.Get(entity);
                translateComponent.Move(velocityComponent.velocity);
                translateComponent.Rotate(velocityComponent.velocity);
                
                // todo: reset velocity
                if(velocityComponent.velocity != Vector3.zero) {
                    velocityComponent.saveVelocity = velocityComponent.velocity;
                }
                velocityComponent.velocity = Vector3.zero;
            }
        }
    }
}