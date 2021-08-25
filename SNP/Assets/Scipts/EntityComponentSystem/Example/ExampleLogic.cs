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
            
            systems = new EcsSystems(world);
            systems
                .Add(new UserInputSystem())
                .Add(new RunJobSystem())
                .Add(new DashJobSystem())
                .Add(new TranslateSystem())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Add(new ClearInputSystem())
                .Init();

            InitEntity();
        }

        private void InitEntity() {
            int entity = world.NewEntity();
            
            ref var input = ref manager.AddComponent<InputComponent>(entity);
            input.inputFrom = InputFrom.User;

            ref var velocity = ref manager.AddComponent<VelocityComponent>(entity);

            ref var transform = ref manager.AddComponent<TranslateComponent>(entity);
            transform.Controller = player;
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