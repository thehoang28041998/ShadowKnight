using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Job;
using Scipts.FiniteStateMachine.Model;
using Scipts.FiniteStateMachine.State;
using Scipts.Movement.Component;
using Scipts.Movement.Job;
using Scipts.Skill.Component;
using Scipts.Skill.Config.Model;
using Scipts.Skill.Job;
using Scipts.Skill.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using Scipts.UserInput.Job;
using Scipts.UserInput.Model;
using UnityEngine;

namespace Scipts.EntityComponentSystem.Example {
    public class ExampleLogic : MonoBehaviour {
        public CharacterController player;

        private EcsWorld world;
        private EcsSystems systems;
        private EntityManager manager;

        private void Awake() {
            // skillFrameConfig
            // todo: using object (class) for config because it has address
            // todo: load config then push data to struct (save data in cache)
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
                .Add(new SkillSystem())  // 
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
            object factoryParameter = DictionarySkillEquipped();
            manager.AddComponent<SkillFactoryComponent>(entity) = new SkillFactoryComponent(factoryParameter as Dictionary<SkillId, SkillFrameConfig>);
            manager.AddComponent<AnimationComponent>(entity) = new AnimationComponent(player.GetComponentInChildren<Animation>());
            
            // todo: add skill component & reference
            manager.AddComponent<SkillComponent>(entity) = new SkillComponent(manager, entity);
            
            // todo: add finite state machine component
            object[] stateMachineParameter = FiniteStateMachineParameter(entity);
            manager.AddComponent<StateMachineComponent>(entity) = new StateMachineComponent(
                stateMachineParameter[0] as IState,
                stateMachineParameter[1] as Stack<StateName>,
                stateMachineParameter[2] as Dictionary<StateName, IState>,
                stateMachineParameter[3] as Dictionary<StateName, StateName[]>
            );
            Dictionary<StateName, IState> define = stateMachineParameter[2] as Dictionary<StateName, IState>;
            manager.AddComponent<IdleStateComponent>(entity) = (IdleStateComponent) define[StateName.IDLE];
        }

        private object DictionarySkillEquipped() {
            Dictionary<SkillId, SkillFrameConfig> dictionary = new Dictionary<SkillId, SkillFrameConfig>();
            dictionary[new SkillId(1, SkillCategory.Attack, 1)] =
                    Resources.Load<SkillFrameConfig>("Character/2/Configs/SkillFrameConfig");
            return dictionary;
        }

        private object[] FiniteStateMachineParameter(int entity) {
            object[] parameter = new object[4];

            parameter[0] = new IdleStateComponent(manager, entity);

            parameter[1] = new Stack<StateName>();
            
            Dictionary<StateName, IState> define = new Dictionary<StateName, IState>();
            define.Add(StateName.IDLE, new IdleStateComponent(manager, entity));
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