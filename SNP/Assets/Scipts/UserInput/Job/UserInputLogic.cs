using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.Movement.Component;
using Scipts.UserInput.Component;
using Scipts.UserInput.Model;
using UnityEngine;

namespace Scipts.UserInput.Job {
    public class UserInputLogic {
        private EcsPool<InputComponent> pool;
        private int entity;

        public UserInputLogic(EntityManager manager, int entity) {
            this.pool = manager.World.GetPool<InputComponent>();
            this.entity = entity;
        }

        public void Run() {
            ref var input = ref pool.Get(entity);
            
            input.Reset();

            if (input.inputFrom == InputFrom.User) {
                input.direction =
                        new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                if (Input.GetKey(KeyCode.L)) {
                    input.isDash = true;
                }

                if (Input.GetKey(KeyCode.J)) {
                    input.isAttack = true;
                }

                if (input.direction != Vector3.zero) {
                    input.isRunning = true;
                }
            }
        }
    }
}