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
            var filter = world.Filter<InputComponent>().End();
            var pool = world.GetPool<InputComponent>();

            foreach (var entity in filter) {
                ref var component = ref pool.Get(entity);
                
                if (component.inputFrom == InputFrom.User) {
                    component.direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    if(Input.GetKeyDown(KeyCode.L)) component.isDash = true;

                    if (component.direction != Vector3.zero) {
                        component.isRunning = true;
                    }

                    if (component.isDash) {
                        if(entityManager.HasComponent<RunRequestComponent>(entity)) {
                            ref var run = ref entityManager.GetComponent<RunRequestComponent>(entity);
                            run.Abort();
                        }

                        ref var dash = ref entityManager.AddComponent<DashRequestComponent>(entity);
                        dash.SetDash(10, 5.0f);
                    }

                    if (component.isRunning) {
                        bool isContinue = true;
                        if(entityManager.HasComponent<DashRequestComponent>(entity)) {
                            ref var dash = ref entityManager.GetComponent<DashRequestComponent>(entity);
                            if (!dash.IsFinish) {
                                isContinue = false;
                            }
                        }

                        if (isContinue) {
                            ref var run = ref entityManager.AddComponent<RunRequestComponent>(entity);
                            run.SetDirection(component.direction);
                        }
                    }
                }
            }
        }
    }
}