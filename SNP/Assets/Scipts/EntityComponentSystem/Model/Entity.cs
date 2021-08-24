using Leopotam.EcsLite;

namespace EntityComponentSystem.Model {
    public class Entity {
        private readonly EcsWorld world;
        private readonly int uniqueId;

        public Entity(EcsWorld world, int uniqueId) {
            this.uniqueId = uniqueId;
            this.world = world;
        }

        public ref T AddComponent<T>() where T : struct, IComponent {
            EcsPool<T> pool = world.GetPool<T>();
            ref T component = ref pool.Add(uniqueId);
            return ref component;
        }

        public ref T GetComponent<T>() where T : struct, IComponent {
            EcsPool<T> pool = world.GetPool<T>();
            ref T component = ref pool.Get(uniqueId);
            return ref component;
        }
    }
}