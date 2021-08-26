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
                .Add(new AddRunSystem())
                .Add(new AddDashRequest())
                .Add(new RunJobSystem())
                .Add(new DashJobSystem())
                .Add(new TranslateSystem())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Init();

            InitEntity();
        }

        private void InitEntity() {
            int entity = world.NewEntity();
            
            ref var input = ref manager.AddComponent<InputComponent>(entity);
            input.inputFrom = InputFrom.User;
 
            ref var translate = ref manager.AddComponent<TranslateComponent>(entity);
            translate.Controller = player;

            ref var request = ref manager.AddComponent<RequestComponent>(entity);
            request.Init();
            
            manager.AddComponent<VelocityComponent>(entity);
            manager.AddComponent<RunRequestComponent>(entity);
            manager.AddComponent<DashRequestComponent>(entity);
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