using System.Collections.Generic;
using System.Linq;
using RSG;
using Scipts.Skills.Core.Config;
using Scipts.Skills.Model;
using Scipts.Utility;

namespace Scipts.Skills {
    public class SkillResourcePreload {
        public Promise<System.Tuple<SkillId, SkillConfig>[]> LoadSkillConfigs(int groupId, List<SkillId> skillIds) {
            int skillCount = skillIds.Count;
            IPromise<SkillConfig>[] configPromises = new IPromise<SkillConfig>[skillCount];
            for (int i = 0; i < skillCount; i++) {
                configPromises[i] = ResourcesLoadSync.Load<SkillConfig>(GetPath(groupId, skillIds[i]));
            }

            System.Tuple<SkillId, SkillConfig>[] skillIdsAndSkillFrameConfigs;
            Promise<System.Tuple<SkillId, SkillConfig>[]> p = new Promise<System.Tuple<SkillId, SkillConfig>[]>();
            Promise<SkillConfig>.All(configPromises)
                                .Then(configs => {
                                    List<System.Tuple<SkillId, SkillConfig>> list = new List<System.Tuple<SkillId, SkillConfig>>();
                                    var enumerator = configs.ToArray();
                                    for (int i = 0; i < enumerator.Length; i++) {
                                        enumerator[i].Deserialization();
                                        list.Add(new System.Tuple<SkillId, SkillConfig>(skillIds[i], enumerator[i]));
                                    }

                                    skillIdsAndSkillFrameConfigs = list.ToArray();
                                    p.Resolve(skillIdsAndSkillFrameConfigs);
                                }).Catch(p.Reject);

            return p;
        }

        private string GetPath(int groupId, SkillId skillId) {
            return $"Character/{groupId}/Configs/{skillId}";
        }
    }
}