using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Scipts.Movement.Component;
using Scipts.Movement.Model;
using Scipts.Movement.Request;

namespace Scipts.Movement.Job {
    public class RequestSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<RequestComponent> pool1;
        private EcsPool<DashComponent> pool2;
        private EcsPool<RunComponent> pool3;

        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            filter = world.Filter<RequestComponent>().Inc<RunComponent>().Inc<DashComponent>().End();
            pool1 = world.GetPool<RequestComponent>();
            pool2 = world.GetPool<DashComponent>();
            pool3 = world.GetPool<RunComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var request = ref pool1.Get(entity);

                // todo: filter : abort request with low priority
                AbortRequest(ref request, entity);

                // todo: invoke request after filter
                InvokeRequest(ref request, entity);
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
                        ref var dash = ref pool2.Get(entity);
                        if (dash.IsFinish) {
                            var dashRequest = (DashRequest) r;
                            dash = new DashComponent(dashRequest.dashDistance, dashRequest.dashDuration,
                                dashRequest.direction);
                            //Debug.Log("Dash success");
                        }

                        break;
                    case RequestType.Run:
                        ref var run = ref pool3.Get(entity);
                        if (run.IsFinish) {
                            var runRequest = (RunRequest) r;
                            run = new RunComponent(runRequest.direction);
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
                IRequest first = requests[i];

                RequestType firstType = first.RequestType;
                Tuple<RequestType, MovementAction>[] firstRule = first.Rule;
                if (firstRule == null) {
                    string msg = "MovementHandleUnit: mixing configuration not found for request of type " + firstType;
                    throw new System.Exception(msg);
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
                                    ref var dash = ref pool2.Get(entity);
                                    dash.Abort();
                                    break;
                                case RequestType.Run:
                                    ref var run = ref pool3.Get(entity);
                                    run.Abort();
                                    break;

                            }

                            requestComponent.RemoveRequest(firstType);
                            break;
                    }
                }
            }
        }
    }
}