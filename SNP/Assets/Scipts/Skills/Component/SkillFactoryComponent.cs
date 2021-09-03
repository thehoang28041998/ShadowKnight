using System.Collections.Generic;
using System.Linq;
using RSG;
using Scipts.EntityComponentSystem.Model;
using Scipts.Skills.Core.Config;
using Scipts.Skills.Model;

namespace Scipts.Skills.Component {
    public struct SkillFactoryComponent : IComponent, IPromiseComponent{
        private Dictionary<string, SkillConfig> cacheDict;
        private Promise promiseInit;

        public SkillFactoryComponent(int groupId, List<SkillId> skillIds) {
            cacheDict = new Dictionary<string, SkillConfig>();
            promiseInit = new Promise();

            var promise = new SkillResourcePreload().LoadSkillConfigs(groupId, skillIds);
            Dictionary<string, SkillConfig> dict = cacheDict;
            Promise tmpThis = promiseInit;
            promise.Then(tuples => {
                var array = tuples.ToArray();
                foreach (var pair in array) {
                    dict[pair.Item1.ToString()] = pair.Item2;
                }

                tmpThis.Resolve();
            }).Catch(tmpThis.Reject);
        }

        public IPromise PromiseInit {
            get => promiseInit;
        }
    }
}