using Leopotam.EcsLite;
using Random = UnityEngine.Random;

namespace EntityComponentSystem.Example {
    public class ExampleSystem : IEcsInitSystem, IEcsRunSystem{
        public void Init(EcsSystems systems) {
        }
        
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<ExampleComponent>().End();
            var pool = world.GetPool<ExampleComponent>();

            foreach (int entity in filter) {
                ref ExampleComponent component = ref pool.Get(entity);
                component.value = Random.value.ToString();
            }
        }
    }
}