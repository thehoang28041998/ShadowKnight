using Leopotam.EcsLite;
using Scipts.Movement.Component;
using UnityEngine;

namespace Scipts.Movement.Job {
    public class TranslateSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<TranslateComponent> pool1;
        private EcsPool<VelocityComponent> pool2;
        
        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            filter = world.Filter<TranslateComponent>().Inc<VelocityComponent>().End();
            pool1 = world.GetPool<TranslateComponent>();
            pool2 = world.GetPool<VelocityComponent>();
        }
        
        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var translateComponent = ref pool1.Get(entity);
                ref var velocityComponent = ref pool2.Get(entity);
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