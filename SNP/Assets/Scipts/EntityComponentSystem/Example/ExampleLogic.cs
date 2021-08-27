using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using Movement.Component;
using Movement.Job;
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

        private void Start() {
            world = new EcsWorld();
            manager = new EntityManager(world);

            systems = new EcsSystems(world, manager);
            systems
                .Add(new UserInputSystem()) // received input & (test: add request)
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
            // todo: add finite state machine component
        }

        private void Update() {
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