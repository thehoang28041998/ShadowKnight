using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using RSG;
using Scipts.EntityComponentSystem;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Job;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Job;
using Scipts.Skills.Component;
using Scipts.Skills.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using Scipts.UserInput.Job;
using Scipts.UserInput.Model;
using UnityEngine;

namespace Scipts {
    public class Logic : MonoBehaviour {
        public CharacterController player;

        private EcsWorld world;
        private EcsSystems systems;
        private EntityManager manager;

        private UserInputLogic userInputLogic;
        private bool started;
        
        private void Awake() {
            Application.targetFrameRate = 60;
            // skillFrameConfig
            // todo: using object (class) for config because it has address
            // todo: load config then push data to struct (save data in cache)
        }

        private void Start() {
            world = new EcsWorld();
            manager = new EntityManager(world);

            systems = new EcsSystems(world);
            systems.Add(new StateMachineSystem())
                   .Add(new IdleStateJobSystem())
                   .Add(new RunStateJobSystem())
                   .Add(new DashStateJobSystem())
                   .Add(new AttackStateJobSystem())
                   //
                   .Add(new RequestSystem())
                   .Add(new RunJobSystem())
                   .Add(new DashJobSystem())
                   //
                   .Add(new TranslateSystem()) // translate with velocity component
#if UNITY_EDITOR
                   .Add(new EcsWorldDebugSystem())
#endif
                   .Init();
           
            int entity = InitEntity();
            FinishInit(entity);
        }

        private void FinishInit(int entity) {
            object[] components = null;
            List<IPromise> promises = new List<IPromise>();
            world.GetComponents(entity, ref components);
            foreach (var component in components) {
                if (component is IPromiseComponent promiseComponent) {
                    promises.Add(promiseComponent.PromiseInit);
                }
            }

            Promise.All(promises)
                   .Then(delegate {
                       userInputLogic = new UserInputLogic(manager, entity);
                       started = true;
                       Debug.Log("Finish init entity : " + entity);
                   }).Catch(Debug.LogError);
        }

        private int InitEntity() {
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
            manager.AddComponent<AnimationComponent>(entity) =
                    new AnimationComponent(player.GetComponentInChildren<Animation>());

            // todo: add skill component & reference
            manager.AddComponent<SkillFactoryComponent>(entity)
                    = new SkillFactoryComponent(2, new List<SkillId> {
                        new SkillId(2, SkillCategory.Run, 1)
                    });

            // todo: add finite state machine component - state component
            // state component
            manager.AddComponent<IdleStateComponent>(entity).isRunning = true;
            manager.AddComponent<RunStateComponent>(entity);
            manager.AddComponent<DashStateComponent>(entity);
            manager.AddComponent<AttackStateComponent>(entity).combo = 1;

            // state machine component
            manager.AddComponent<StateMachineComponent>(entity) = new StateMachineComponent(
                StateName.IDLE,
                new Stack<StateName>(),
                new List<StateName> {
                    StateName.IDLE, StateName.RUN, StateName.DASH, StateName.ATTACK
                },
                FiniteStateMachineParameter()
            );

            return entity;
        }

        private Dictionary<StateName, StateName[]> FiniteStateMachineParameter() {
            Dictionary<StateName, StateName[]> transition = new Dictionary<StateName, StateName[]>();
            transition.Add(StateName.IDLE, new[] {StateName.RUN, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.RUN, new[] {StateName.IDLE, StateName.DASH, StateName.ATTACK});
            transition.Add(StateName.DASH, new[] {StateName.RUN, StateName.IDLE});
            transition.Add(StateName.ATTACK, new[] {StateName.IDLE, StateName.RUN, StateName.ATTACK});
            
            return transition;
        }

        private void Update() {
            if (!started) return;
            
            userInputLogic.Run();
        }

        private void FixedUpdate() {
            if (!started) return;
            
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