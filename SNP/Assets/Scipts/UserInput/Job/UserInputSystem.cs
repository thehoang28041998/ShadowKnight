using Leopotam.EcsLite;
using Scipts.EntityComponentSystem.Model;
using Scipts.Movement.Component;
using Scipts.Skill.Component;
using Scipts.Skill.Model;
using Scipts.UserInput.Component;
using Scipts.UserInput.Model;
using UnityEngine;

namespace Scipts.UserInput.Job {
    public struct UserInputSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;

        public void Init(EcsSystems systems) {
            entityManager = systems.GetShared<EntityManager>();
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<InputComponent>().Inc<RequestComponent>().End();
            var pool = world.GetPool<InputComponent>();

            foreach (var entity in filter) {
                ref var inputComponent = ref pool.Get(entity);
                ref var requestComponent = ref entityManager.GetComponent<RequestComponent>(entity);

                inputComponent.Reset();

                if (inputComponent.inputFrom == InputFrom.User) {
                    inputComponent.direction =
                        new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                    if (Input.GetKey(KeyCode.L)) {
                        inputComponent.isDash = true;
                    }

                    if (Input.GetKey(KeyCode.J)) {
                        inputComponent.isAttack = true;
                        entityManager.GetComponent<SkillComponent>(entity).queueSkillId = new SkillId(1, SkillCategory.Attack, 1);
                    }

                    if (inputComponent.direction != Vector3.zero) {
                        inputComponent.isRunning = true;
                    }
                }
            }
        }
    }
}