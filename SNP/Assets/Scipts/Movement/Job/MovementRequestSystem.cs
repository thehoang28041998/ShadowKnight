using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Movement.Component;
using UserInput.Component;

namespace Movement.Job {
    public class MovementRequestSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;
        
        public void Init(EcsSystems systems) {
            entityManager = EntityManager.Instance;
        }
        
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<InputComponent>().End();
            var pool = world.GetPool<InputComponent>();

            foreach (var entity in filter) {
                ref var inputComponent = ref pool.Get(entity);

                if (inputComponent.isRunning) {
                    ref var runComponent = ref entityManager.AddComponent<RunRequestComponent>(entity);
                    runComponent.SetDirection(inputComponent.direction);
                }
            }
        }

    }
}