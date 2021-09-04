using System.Collections.Generic;
using Scipts.EntityComponentSystem;
using Scipts.Exception;
using Scipts.Skills.Core.Model;
using Scipts.Skills.Model;

namespace Scipts.Skills.Component {
    public struct SkillComponent : IComponent {
        public readonly int entity;
        private readonly EntityManager manager;

        private Dictionary<BaseSkill, SkillId> containerOnGoingSkill;
        private List<BaseSkill> ongoingSkills;
        private BaseSkill channelingBaseSkill;
        private bool isUnderChanneling;

        public SkillComponent(int entity) {
            this.entity = entity;
            this.manager = EntityManager.Instance;
            this.ongoingSkills = new List<BaseSkill>();
            this.containerOnGoingSkill = new Dictionary<BaseSkill, SkillId>();
            this.channelingBaseSkill = null;
            this.isUnderChanneling = false;
        }

        public BaseSkill CastSkill(SkillId skillId) {
            CheckSkillCastingRequirementExisted(skillId);
            CheckSkillIsCastAble(skillId);
            
            //requirementEquipped[skillId.ToString()].StartCooldown();
            ref var factory = ref manager.GetComponent<SkillFactoryComponent>(entity);
            var skill = factory.Generation(skillId);
            if (skill == null) {
                throw new NotFoundException(typeof(BaseSkill), $"{skillId} not have been equipped");
            }
            
            ongoingSkills.Add(skill);
            skill.OnCast(skillId);
            channelingBaseSkill = skill;

            if (!channelingBaseSkill.isChannelingFinish) isUnderChanneling = true;

            containerOnGoingSkill[skill] = skillId;
            return skill;
        }

        public void Update(float dt) {
            // update condition & cooldown
            /*foreach (var pair in requirementEquipped.Values) {
                pair.Update(dt);
            }*/

            for (int kIndex = ongoingSkills.Count - 1; kIndex >= 0; kIndex--) {
                BaseSkill baseSkill = ongoingSkills[kIndex];
                baseSkill.Update(dt);
            }
            
            //characterStatus.Update(dt);

            if (isUnderChanneling && channelingBaseSkill.isChannelingFinish) {
                isUnderChanneling = false;
            }
        }

        public void LateUpdate(float dt) {
            for (int kIndex = ongoingSkills.Count - 1; kIndex >= 0; kIndex--) {
                BaseSkill baseSkill = ongoingSkills[kIndex];
                baseSkill.LateUpdate(dt);

                if (baseSkill.isSkillFinish) {
                    ongoingSkills.RemoveAt(kIndex);
                    baseSkill.OnFinish();
                    containerOnGoingSkill.Remove(baseSkill);
                }
            }

            //characterStatus.LateUpdate(dt);
        }
        
        public void InterruptSkill(SkillId skillId) {
            if (skillId == null || !containerOnGoingSkill.ContainsValue(skillId)) {
                return;
            }

            BaseSkill baseSkill = null;
            foreach (var pair in containerOnGoingSkill) {
                if (pair.Value.Equals(skillId)) {
                    baseSkill = pair.Key;
                    break;
                }
            }

            ongoingSkills.Remove(baseSkill);
            baseSkill.Interrupt();
            baseSkill.OnFinish();
            containerOnGoingSkill.Remove(baseSkill);
        }

        private void CheckSkillCastingRequirementExisted(SkillId skillId) {
            if (skillId == null) {
                throw new NotFoundException(typeof(SkillId), $"No skill casting requirement found/ skillId = null");
            }

            /*if (!requirementEquipped.ContainsKey(skillId.ToString())) {
                throw new NotFoundException(typeof(SkillId), $"No skill casting requirement found/" + skillId);
            }*/
        }
        
        private void CheckSkillIsCastAble(SkillId skillId) {
            /*SkillCastingRequirement req = requirementEquipped[skillId.ToString()];
            if (!req.IsCastable()) {
                throw new NotFoundException(typeof(SkillCastingRequirement),
                    $"Skill is not castable, reason: {req.Reasons()}/{skillId}");
            }*/
        }
    }
}