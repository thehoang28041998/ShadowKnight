using Leopotam.EcsLite.Threads;
using Leopotam.EcsLite.Threads.Unity;
using Movement.Component;
using Unity.Collections;
using UnityEngine;
using UserInput.Component;
using UserInput.Model;

namespace Movement.Job {
    public struct VelocityJob : IEcsUnityJob<VelocityComponent, InputComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<VelocityComponent> pool1;
        private NativeArray<int> indices1;
        [NativeDisableParallelForRestriction] 
        private NativeArray<InputComponent> pool2;
        private NativeArray<int> indices2;

        public void Init(NativeArray<int> entities, NativeArray<VelocityComponent> pool1, NativeArray<int> indices1,
                         NativeArray<InputComponent> pool2, NativeArray<int> indices2) {
            this.entities = entities;
            this.pool1 = pool1;
            this.indices1 = indices1;
            this.pool2 = pool2;
            this.indices2 = indices2;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int index_1 = indices1[entity];
            VelocityComponent velocityComponent = pool1[index_1];

            int index_2 = indices2[entity];
            InputComponent inputComponent = pool2[index_2];

            if (inputComponent.inputFrom == InputFrom.User) {
                velocityComponent.velocity += inputComponent.direction;
                pool1[index_1] = velocityComponent;
            }
        }
    }
}