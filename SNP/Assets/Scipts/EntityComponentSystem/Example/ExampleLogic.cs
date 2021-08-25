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
        public GameObject player;
        
        private EcsWorld world;
        private EcsSystems systems;

        private void Start() {
            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems.Add(new ExampleSystem())
                .Add(new UserInputSystem())
                .Add(new VelocityJobSystem())
                .Add(new TranslateSystem())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Init();

            InitEntity();
        }

        private void InitEntity() {
            var entity = new Entity(world, world.NewEntity());
            ref var example = ref entity.AddComponent<ExampleComponent>();

            ref var input = ref entity.AddComponent<InputComponent>();
            input.inputFrom = InputFrom.User;

            ref var velocity = ref entity.AddComponent<VelocityComponent>();

            ref var transform = ref entity.AddComponent<TranslateComponent>();
            transform.transform = player.transform;
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