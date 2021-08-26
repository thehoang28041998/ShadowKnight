using System;
using System.Collections.Generic;
using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Movement.Component;
using Movement.Model;
using UnityEngine;

namespace Movement.Job {
    public struct RequestSystem : IEcsInitSystem, IEcsRunSystem {
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

                // todo: filter : abort request with low priority
                AbortRequest(ref requestComponent, entity);

                // todo: invoke request after filter
                InvokeRequest(ref requestComponent, entity);
            }
        }

        private void InvokeRequest(ref RequestComponent requestComponent, int entity) {

            // todo: Allocation request
            // todo: check if request is running, remove request

            List<IRequest> queue = new List<IRequest>();
            queue.AddRange(requestComponent.Requests);
            
            foreach (var r in queue) {
                //Debug.Log($"Allow: {r.RequestType}");
                switch (r.RequestType) {
                    case RequestType.Dash:
                        ref var dash = ref entityManager.GetComponent<DashRequestComponent>(entity);
                        if (dash.IsFinish) {
                            var dashRequest = (DashRequestComponent) r;
                            dash = new DashRequestComponent(dashRequest.DashDistance, dashRequest.DashDuration,
                                dashRequest.DashDirection);
                            //Debug.Log("Dash success");
                        }

                        break;
                    case RequestType.Run:
                        ref var run = ref entityManager.GetComponent<RunRequestComponent>(entity);
                        if (run.IsFinish) {
                            var runRequest = (RunRequestComponent) r;
                            run = new RunRequestComponent(runRequest.Direction);
                            //Debug.Log("Run success");
                        }

                        break;
                }

                requestComponent.Requests.Remove(r);
            }
        }

        private void AbortRequest(ref RequestComponent requestComponent, int entity) {
            var requests = requestComponent.Requests;
            if (requests.Count < 2) return;

            for (int i = 0; i < requests.Count; i++) {
                IRequest r = requests[i];
                if (r.IsFinish) continue;

                IRequest first = r;

                RequestType firstType = first.RequestType;
                Tuple<RequestType, MovementAction>[] firstRule = first.Rule;
                if (firstRule == null) {
                    string msg = "MovementHandleUnit: mixing configuration not found for request of type " + firstType;
                    throw new Exception(msg);
                }

                for (int k = i + 1; k < requests.Count; k++) {
                    var after = requests[k];
                    MovementAction action = MovementAction.AllowCurrent;
                    bool found = false;
                    for (int j = 0; j < firstRule.Length; j++) {
                        Tuple<RequestType, MovementAction> dataTuple = firstRule[j];
                        if (dataTuple.Item1 == after.RequestType) {
                            found = true;
                            action = dataTuple.Item2;
                            break;
                        }
                    }

                    if (!found) continue;

                    switch (action) {
                        case MovementAction.AbortCurrent:
                            //Debug.Log($"Abort: {after.RequestType}");
                            switch (after.RequestType) {
                                case RequestType.Dash:
                                    ref var dash = ref entityManager.GetComponent<DashRequestComponent>(entity);
                                    dash.Abort();
                                    break;
                                case RequestType.Run:
                                    ref var run = ref entityManager.GetComponent<RunRequestComponent>(entity);
                                    run.Abort();
                                    break;

                            }

                            requestComponent.RemoveRequest(r.RequestType);
                            break;
                    }
                }
            }
        }
    }
}