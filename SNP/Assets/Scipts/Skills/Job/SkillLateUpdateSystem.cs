using Leopotam.EcsLite;
using Scipts.Helper;
using Scipts.Skills.Component;

namespace Scipts.Skills.Job {
    public class SkillLateUpdateSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<SkillComponent> pool1;

        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<SkillComponent>().End();
            pool1 = world.GetPool<SkillComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var skill = ref pool1.Get(entity);
                skill.LateUpdate(FrameHelper.TIME_DELTA);
            }
        }
    }
}