namespace Scipts.Skill.Model {
    public class SkillId {
        public readonly int id;
        public readonly SkillCategory category;
        public readonly int subId;
        public readonly string stringValue;

        public SkillId(int id, SkillCategory category, int subId) {
            this.id = id;
            this.category = category;
            this.subId = subId;
            stringValue = $"{id}_{category}_{subId}";
        }

        public SkillId Clone() {
            return new SkillId(id, category, subId);
        }
        
        public override string ToString() {
            return stringValue;
        }

        public bool Compare(SkillId skillId) {
            return stringValue.Equals(skillId.ToString());
        }
    }
}