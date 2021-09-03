namespace Scipts.Skills.Model {
    public class SkillId {
        private readonly int id;
        private readonly SkillCategory category;
        private readonly int subId;
        private readonly string stringValue;

        public SkillId(int id, SkillCategory category, int subId) {
            this.id = id;
            this.category = category;
            this.subId = subId;
            stringValue = $"{id}_{category}_{subId}";
        }

        public SkillId Clone() {
            return new SkillId(id, category, subId);
        }

        public SkillCategory SkillCategory {
            get => category;
        }

        public int Id {
            get => id;
        }

        public override string ToString() {
            return stringValue;
        }

        public bool Compare(SkillId skillId) {
            return stringValue.Equals(skillId.ToString());
        }
    }
}