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
                if (component.inputFrom == InputFrom.User) {
                    component.direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
            }
        }
    }
}