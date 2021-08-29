using Leopotam.EcsLite;

namespace Scipts.EntityComponentSystem.Model {
    public class EntityManager {
        private readonly EcsWorld world;
        private static EntityManager instance;

        public static EntityManager Instance {
            get => instance;
        }

        public EntityManager(EcsWorld world) {
            this.world = world;
            instance = this;
        }

        public ref T AddComponent<T>(int entity) where T : struct, IComponent {
            EcsPool<T> pool = world.GetPool<T>();
            if(!pool.Has(entity)) {
                ref T component = ref pool.Add(entity);
                return ref component;
            }

            ref T _component = ref pool.Get(entity);
            return ref _component;
        }

        public ref T GetComponent<T>(int entity) where T : struct, IComponent {
            EcsPool<T> pool = world.GetPool<T>();
            ref T component = ref pool.Get(entity);
            return ref component;
        }

        public void DeleteComponent<T>(int entity) where T : struct, IComponent {
            EcsPool<T> pool = world.GetPool<T>();
            pool.Del(entity);
        }
    }
}