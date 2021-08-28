using System;
using System.Collections.Generic;
using System.IO;
using EntityComponentSystem.Model;
using FiniteStateMachine.Component;
using FiniteStateMachine.Job;
using FiniteStateMachine.Model;
using FiniteStateMachine.State;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using Movement.Component;
using Movement.Job;
using Skill.Model;
using Skill.Trigger;
using UnityAnimation.Component;
using UnityEngine;
using UserInput.Component;
using UserInput.Job;
using UserInput.Model;

namespace EntityComponentSystem.Example {
    public class ExampleLogic : MonoBehaviour {
        public CharacterController player;

        private EcsWorld world;
        private EcsSystems systems;
        private EntityManager manager;

#if UNITY_EDITOR
        public SkillFrameConfig skillFrameConfig;     
#endif

        private void Awake() {
            skillFrameConfig.Deserialization();
        }

        private void Start() {
            world = new EcsWorld();
            manager = new EntityManager(world);

            systems = new EcsSystems(world, manager);
            systems
                .Add(new UserInputSystem()) // received input & (test: add request)
                .Add(new StateMachineSystem())
                .Add(new RequestSystem()) // allocation request
                .Add(new RunJobSystem()) // handle run job
                .Add(new DashJobSystem()) // handle dash job
                .Add(new TranslateSystem()) // translate with velocity component
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Init();

            InitEntity();
        }

        private void InitEntity() {
            int entity = world.NewEntity();
            //todo: add input component
            manager.AddComponent<InputComponent>(entity) = new InputComponent(InputFrom.User);
            
            //todo: add translate/move component
            manager.AddComponent<TranslateComponent>(entity) = new TranslateComponent(player);
            manager.AddComponent<RequestComponent>(entity).Init();
            manager.AddComponent<VelocityComponent>(entity).Init();
            manager.AddComponent<RunComponent>(entity) = new RunComponent();
            manager.AddComponent<DashComponent>(entity) = new DashComponent();
           
            // todo: add animation component
            manager.AddComponent<AnimationComponent>(entity) = new AnimationComponent(player.GetComponentInChildren<Animation>());
            
            // todo: add finite state machine component
            object[] parameter = FiniteStateMachineParameter(entity);
            manager.AddComponent<StateMachineComponent>(entity) = new StateMachineComponent(
                parameter[0] as IState,
                parameter[1] as Stack<StateName>,
                parameter[2] as Dictionary<StateName, IState>,
                parameter[3] as Dictionary<StateName, StateName[]>
            );
        }

        private object[] FiniteStateMachineParameter(int entity) {
            object[] parameter = new object[4];

            parameter[0] = new IdleState(manager, entity);

            parameter[1] = new Stack<StateName>();
            
            Dictionary<StateName, IState> define = new Dictionary<StateName, IState>();
            define.Add(StateName.IDLE, new IdleState(manager, entity));
            define.Add(StateName.RUN, new RunState(manager, entity));
            define.Add(StateName.DASH, new DashState(manager, entity));
            define.Add(StateName.ATTACK, new AttackState(manager, entity));
            parameter[2] = define;

            Dictionary<StateName, StateName[]> transition = new Dictionary<StateName, StateName[]>();
            transition.Add(StateName.IDLE, new[] {StateName.RUN, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.RUN, new[] {StateName.IDLE, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.DASH, new[] {StateName.RUN, StateName.IDLE});
            transition.Add(StateName.ATTACK, new[] {StateName.IDLE, StateName.RUN, StateName.ATTACK});
            parameter[3] = transition;
            
            return parameter;
        }


        private void LateUpdate() {
            ref var baseEvent = ref skillFrameConfig.baseEvent;
            ref var trigger = ref baseEvent.trigger;
            switch (baseEvent.trigger.TriggerType) {
                case TriggerType.Event:
                    var _event = (EventTrigger) trigger;
                    Debug.Log($"{_event.eventId}");
                    break;
                case TriggerType.Frame:
                    var _frame = (TimelineTrigger) trigger;
                    Debug.Log($"{_frame.frame}, {_frame.scale}");
                    break;
            }
        }

        private void FixedUpdate() {
            systems?.Run();
        }

        private void OnDestroy() {
            if (systems != null) {
                systems.Destroy();
                world.Destroy();
                systems = null;
            }
        }
    }
}