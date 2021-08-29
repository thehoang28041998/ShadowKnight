using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Job;
using Scipts.FiniteStateMachine.Model;
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
                // todo: input
                .Add(new UserInputSystem()) // received input & (test: add request)
                // todo: state machine
                .Add(new StateMachineSystem())
                .Add(new IdleStateJobSystem())
                .Add(new RunStateJobSystem())
                .Add(new DashStateJobSystem())
                .Add(new AttackStateJobSystem())
                // todo: movement
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
            manager.AddComponent<SkillFactoryComponent>(entity) =
                new SkillFactoryComponent(factoryParameter as Dictionary<SkillId, SkillFrameConfig>);
            manager.AddComponent<AnimationComponent>(entity) =
                new AnimationComponent(player.GetComponentInChildren<Animation>());

            // todo: add skill component & reference
            manager.AddComponent<SkillComponent>(entity) = new SkillComponent(manager, entity);

            // todo: add finite state machine component - state component
            // state component
            ref var idleState = ref manager.AddComponent<IdleStateComponent>(entity);
            idleState = new IdleStateComponent(entity);
            idleState.Enter(StateName.UNDEFINE, false, manager);
          
            ref var runState = ref manager.AddComponent<RunStateComponent>(entity);
            runState = new RunStateComponent(entity);

            ref var dashState = ref manager.AddComponent<DashStateComponent>(entity);
            dashState = new DashStateComponent(entity);

            ref var attackState = ref manager.AddComponent<AttackStateComponent>(entity);
            attackState = new AttackStateComponent(entity);
            
            // state machine component
            manager.AddComponent<StateMachineComponent>(entity) = new StateMachineComponent(
                StateName.IDLE,
                new Stack<StateName>(),
                new List<StateName> {
                    StateName.IDLE, StateName.RUN, StateName.DASH, StateName.ATTACK
                },
                FiniteStateMachineParameter()
            );
        }

        private object DictionarySkillEquipped() {
            Dictionary<SkillId, SkillFrameConfig> dictionary = new Dictionary<SkillId, SkillFrameConfig>();
            dictionary[new SkillId(1, SkillCategory.Attack, 1)] =
                Resources.Load<SkillFrameConfig>("Character/2/Configs/SkillFrameConfig");
            return dictionary;
        }

        private Dictionary<StateName, StateName[]> FiniteStateMachineParameter() {
            Dictionary<StateName, StateName[]> transition = new Dictionary<StateName, StateName[]>();
            transition.Add(StateName.IDLE, new[] {StateName.RUN, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.RUN, new[] {StateName.IDLE, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.DASH, new[] {StateName.RUN, StateName.IDLE});
            transition.Add(StateName.ATTACK, new[] {StateName.IDLE, StateName.RUN, StateName.ATTACK});
            
            return transition;
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