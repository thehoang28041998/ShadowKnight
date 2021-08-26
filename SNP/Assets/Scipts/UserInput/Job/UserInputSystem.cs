using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Movement.Component;
using UnityEngine;
using UserInput.Component;
using UserInput.Model;

namespace UserInput.Job {
    public class UserInputSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;
        
        public void Init(EcsSystems systems) {
            entityManager = EntityManager.Instance;
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<InputComponent>().Inc<RequestComponent>().End();
            var pool = world.GetPool<InputComponent>();

            foreach (var entity in filter) {
                ref var inputComponent = ref pool.Get(entity);
                ref var requestComponent = ref entityManager.GetComponent<RequestComponent>(entity);
                
                inputComponent.Reset();
                
                if (inputComponent.inputFrom == InputFrom.User) {
                    inputComponent.direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    if(Input.GetKeyDown(KeyCode.L)) inputComponent.isDash = true;

                    if (inputComponent.direction != Vector3.zero) {
                        inputComponent.isRunning = true;
                    }

                    if (inputComponent.isDash) {
                        var velocity = entityManager.GetComponent<VelocityComponent>(entity);
                        requestComponent.AddRequest(new DashRequestComponent(10, 1.0f, velocity.saveVelocity));
                    }

                    if (inputComponent.isRunning) {
                       requestComponent.AddRequest(new RunRequestComponent(inputComponent.direction));
                    }
                }
            }
        }
    }
}