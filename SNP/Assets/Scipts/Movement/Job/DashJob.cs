using System;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.Helper;
using Scipts.Movement.Component;
using Unity.Collections;
using UnityEngine;

namespace Scipts.Movement.Job {
    public struct DashJob : IEcsUnityJob<VelocityComponent, DashComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<VelocityComponent> pool1;
        private NativeArray<int> indices1;
        [NativeDisableParallelForRestriction] 
        private NativeArray<DashComponent> pool2;
        private NativeArray<int> indices2;
        
        public void Init(NativeArray<int> entities, NativeArray<VelocityComponent> pool1, NativeArray<int> indices1, NativeArray<DashComponent> pool2, NativeArray<int> indices2) {
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
            DashComponent dash = pool2[pool2Idx];

            if (!dash.IsFinish) {
                dash.elapsed += FrameHelper.TIME_DELTA;
                
                float progress = dash.elapsed / dash.dashDuration;
                progress = Math.Min(progress, 1.0f);
                
                float traveledDistance = calculate(0, dash.dashDistance, progress);
                float deltaDistance = traveledDistance - dash.previousDistance;
                dash.previousDistance = traveledDistance;
                
                velocityComponent.velocity += dash.direction * deltaDistance;
            }

            pool1[pool1Idx] = velocityComponent;
            pool2[pool2Idx] = dash;
        }
        
        // Line
        private float calculate(float s, float e, float t) {
            return Mathf.Lerp(s, e, t);
        }
    }
}