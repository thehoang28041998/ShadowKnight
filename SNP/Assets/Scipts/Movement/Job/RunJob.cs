using Leopotam.EcsLite.Threads.Unity;
using Movement.Component;
using Unity.Collections;

namespace Movement.Job {
    public struct RunJob : IEcsUnityJob<VelocityComponent, RunRequestComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<VelocityComponent> pool1;
        private NativeArray<int> indices1;
        [NativeDisableParallelForRestriction] 
        private NativeArray<RunRequestComponent> pool2;
        private NativeArray<int> indices2;
        public float deltaTime { get; set; }

        public void Init(NativeArray<int> entities, NativeArray<VelocityComponent> pool1, NativeArray<int> indices1,
                         NativeArray<RunRequestComponent> pool2, NativeArray<int> indices2) {
            this.entities = entities;
            this.pool1 = pool1;
            this.pool2 = pool2;
            this.indices1 = indices1;
            this.indices2 = indices2;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int pool1Idx = indices1[entity];
            VelocityComponent velocityComponent = pool1[pool1Idx];
            
            int pool2Idx = indices2[entity];
            RunRequestComponent runRequest = pool2[pool2Idx];

            if (!runRequest.IsFinish) {
                runRequest.Update(deltaTime);
                velocityComponent.velocity += runRequest.GetVelocity(deltaTime);
            }

            pool1[pool1Idx] = velocityComponent;
            pool2[pool2Idx] = runRequest;
        }
    }
}