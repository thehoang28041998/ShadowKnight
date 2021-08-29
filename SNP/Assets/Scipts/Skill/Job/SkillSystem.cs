using Leopotam.EcsLite;
using Scipts.EntityComponentSystem.Model;
using Scipts.Skill.Component;

namespace Scipts.Skill.Job {
    public class SkillSystem : IEcsInitSystem, IEcsRunSystem{
        private EntityManager entityManager;

        public void Init(EcsSystems systems) {
            entityManager = systems.GetShared<EntityManager>();
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<SkillComponent>().End();
            var pool = world.GetPool<SkillComponent>();

            foreach (var entity in filter) {
                ref var skillComponent = ref pool.Get(entity);
                var factory = entityManager.GetComponent<SkillFactoryComponent>(entity);
                
                // todo: listen queue skill & cast
                if (skillComponent.queueSkillId != null) {
                    // todo: cast skill id

                    var piece = factory.Generation(skillComponent.queueSkillId);
                    ref var pieceData = ref piece;
                    skillComponent.ongoingSkills.Add(pieceData);

                    skillComponent.queueSkillId = null;
                }
            }
        }
    }
}