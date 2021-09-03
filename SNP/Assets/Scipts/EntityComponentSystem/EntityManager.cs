using Leopotam.EcsLite;

namespace Scipts.EntityComponentSystem {
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

        public int GetComponents(int entity, ref object[] components) {
            return world.GetComponents(entity, ref components);
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

        public int NewEntity() {
            // todo: listen create entity
            return world.NewEntity();
        }

        public void DelEntity(int entity) {
            // todo: listen destroy entity
            world.DelEntity(entity);
        }

        public EcsWorld World {
            get => world;
        }

        public void Destroy() {
            world.Destroy();
        }
    }
}