using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using UserInput.Component;

namespace UserInput.Job {
    public class ClearInputSystem : IEcsInitSystem, IEcsRunSystem {
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
                component.Reset();
            }
        }
    }
}