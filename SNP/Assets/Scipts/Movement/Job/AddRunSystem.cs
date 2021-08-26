using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Movement.Component;
using Movement.Model;

namespace Movement.Job {
    public class AddRunSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;

        public void Init(EcsSystems systems) {
            entityManager = EntityManager.Instance;
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<RequestComponent>().Inc<RunRequestComponent>().End();
            var pool = world.GetPool<RequestComponent>();

            foreach (var entity in filter) {
                ref var requestComponent = ref pool.Get(entity);
                IRequest request = requestComponent.GetRequest(RequestType.Run);
                if (request == null) continue;

                // dashing?
                bool isContinue = true;
                var dash = entityManager.GetComponent<DashRequestComponent>(entity);
                if (!dash.IsFinish) {
                    isContinue = false;
                }

                // running?
                ref var run = ref entityManager.GetComponent<RunRequestComponent>(entity);
                if (!run.IsFinish) {
                    isContinue = false;
                }

                // add new run
                if (isContinue) {
                    var runRequest = (RunRequestComponent) request;
                    run = new RunRequestComponent(runRequest.Direction);
                }

                requestComponent.RemoveRequest(RequestType.Run);
            }
        }
    }
}