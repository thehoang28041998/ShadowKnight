using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Movement.Component;
using Movement.Model;

namespace Movement.Job {
    public struct AddDashRequest : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;

        public void Init(EcsSystems systems) {
            entityManager = EntityManager.Instance;
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<RequestComponent>().Inc<DashRequestComponent>().End();
            var pool = world.GetPool<RequestComponent>();

            foreach (var entity in filter) {
                ref var requestComponent = ref pool.Get(entity);
                IRequest request = requestComponent.GetRequest(RequestType.Dash);
                if (request == null) continue;

                bool isContinue = true;
                // dashing?
                ref var dash = ref entityManager.GetComponent<DashRequestComponent>(entity);
                if (!dash.IsFinish) {
                    isContinue = false;
                }

                // abort running
                ref var run = ref entityManager.GetComponent<RunRequestComponent>(entity);
                run.Abort();

                // add new dash
                if (isContinue) {
                    var dashRequest = (DashRequestComponent) request;
                    dash = new DashRequestComponent(dashRequest.DashDistance, dashRequest.DashDuration);
                }
                
                requestComponent.RemoveRequest(RequestType.Dash);
            }
        }
    }
}