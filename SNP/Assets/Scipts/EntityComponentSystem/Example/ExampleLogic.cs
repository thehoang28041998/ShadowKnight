using System;
using EntityComponentSystem.Model;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace EntityComponentSystem.Example {
    public class ExampleLogic : MonoBehaviour {
        private EcsWorld world;
        private EcsSystems systems;
        private Entity entity;

        private void Start() {
            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems.Add(new ExampleSystem())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Init();

            entity = new Entity(world, world.NewEntity());
            ref var example = ref entity.AddComponent<ExampleComponent>();
            example.value = "hoang dep trai";
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