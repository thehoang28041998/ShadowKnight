using Leopotam.EcsLite;
using UnityEngine;
using UserInput.Component;
using UserInput.Model;

namespace UserInput.Job {
    public class UserInputSystem :IEcsRunSystem {
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<InputComponent>().End();
            var pool = world.GetPool<InputComponent>();

            foreach (var entity in filter) {
                ref var component = ref pool.Get(entity);
                component.isRunning = false;
                if (component.inputFrom == InputFrom.User) {
                    component.direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    component.isRunning = true;
                }
            }
        }
    }
}