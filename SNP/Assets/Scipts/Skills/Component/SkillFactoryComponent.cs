using System;
using System.Collections.Generic;
using System.Linq;
using RSG;
using Scipts.EntityComponentSystem;
using Scipts.EntityComponentSystem.Model;
using Scipts.Exception;
using Scipts.Skills.Combat.Model;
using Scipts.Skills.Core.Config;
using Scipts.Skills.Core.Event;
using Scipts.Skills.Core.Model;
using Scipts.Skills.Model;
using UnityEngine;

namespace Scipts.Skills.Component {
    public struct SkillFactoryComponent : IComponent, IPromiseComponent{
        private Dictionary<string, SkillConfig> cacheDict;
        private int entity;
        private Promise promiseInit;

        public SkillFactoryComponent(int entity, int groupId, List<SkillId> skillIds) {
            this.cacheDict = new Dictionary<string, SkillConfig>();
            this.promiseInit = new Promise();
            this.entity = entity;

            var promise = new SkillResourcePreload().LoadSkillConfigs(groupId, skillIds);
            SkillFactoryComponent tmpThis = this;
            promise.Then(tuples => {
                var array = tuples.ToArray();
                foreach (var pair in array) {
                    tmpThis.cacheDict[pair.Item1.ToString()] = pair.Item2;
                }

                tmpThis.promiseInit.Resolve();
                
            }).Catch(tmpThis.promiseInit.Reject);
        }

        public BaseSkill Generation(SkillId skillId) {
            if (!cacheDict.ContainsKey(skillId.ToString())) {
                throw new NotFoundException(typeof(SkillConfig), "with category " + skillId);
            }

            SkillConfig config = cacheDict[skillId.ToString()];
            BaseSkill baseSkill = GetSkill(new DefaultSkill.Dependencies(config, entity));

            for (int i = 0; i < config.eventCollection.Count; i++) {
                EventCollection collection = config.eventCollection[i];
                foreach (var evt in collection.events) {
                    if (!evt.enable) continue;
                    baseSkill.AddEventFrame(i, evt);
                }
            }

            return baseSkill;
        }

        private BaseSkill GetSkill(DefaultSkill.Dependencies dependencies) {
            string fullname = "Scipts.Skills.Combat.Model." + dependencies.skillConfig.skillClassName;
            Type t = Type.GetType(fullname);
            return (BaseSkill) t.GetConstructor(new[] {
                                    typeof(EntityManager),
                                    typeof(DefaultSkill.Dependencies)
                                })
                                .Invoke(new object[] {EntityManager.Instance, dependencies});
        }

        public IPromise PromiseInit {
            get => promiseInit;
        }
    }
}