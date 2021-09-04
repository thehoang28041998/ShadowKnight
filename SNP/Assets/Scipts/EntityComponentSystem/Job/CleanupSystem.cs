using Leopotam.EcsLite;
using Scipts.EntityComponentSystem.Component;
using Scipts.EntityComponentSystem.Model;
using Scipts.Helper;

namespace Scipts.EntityComponentSystem.Job {
    public class CleanupSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;
        private EcsFilter filter;
        private EcsPool<CleanupComponent> pool1;
        
        public void Init(EcsSystems systems) {
            var world = systems.GetWorld();
            filter = world.Filter<CleanupComponent>().End();
            pool1 = world.GetPool<CleanupComponent>();
            entityManager = EntityManager.Instance;
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var cleanup = ref pool1.Get(entity);
                cleanup.elapsed -= FrameHelper.TIME_DELTA;

                if (!cleanup.IsEligibleToCleanup()) continue;
                
                object[] components = new object[0];
                entityManager.GetComponents(entity, ref components);
                foreach (var component in components) {
                    if (component is IComponentDestroy componentDestroy) {
                        componentDestroy.Destroy();
                    }
                }
                
                entityManager.DelEntity(entity);
            }
        }
    }
}