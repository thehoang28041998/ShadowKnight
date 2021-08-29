using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;

namespace Scipts.FiniteStateMachine.State {
    public struct RunState : IState {
        private EntityManager entityManager;
        private int entity;

        public RunState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
        }

        public StateName StateName {
            get => StateName.RUN;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play animation run in here
            entityManager.GetComponent<AnimationComponent>(entity).PlayRun();
        }

        public void Update(ref StateMachineComponent component, float dt) {
            var inputComponent = entityManager.GetComponent<InputComponent>(entity);

            if (inputComponent.isAttack) {
                component.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                return;
            }
            
            if (inputComponent.isDash) {
                // todo: change state to the dash state
                component.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isRunning) {
                // todo: update request run
                entityManager.GetComponent<RequestComponent>(entity).AddRequest(new RunRequest(inputComponent.direction.normalized));
                return;
            }
            
            // todo: go to the idle state
            component.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
        }

        public void Exit() {
            
        }
    }
}