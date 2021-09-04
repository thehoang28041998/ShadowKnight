using System;
using System.Collections.Generic;
using MEC;
using RSG;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scipts.Utility {
    public class ResourcesLoadSync {
        private static readonly List<System.Tuple<string, IPromise<Object>, Object>> cache =
                new List<System.Tuple<string, IPromise<Object>, Object>>();

        public static IPromise<T> Load<T>(string path)
                where T : Object {
#if RESOURCE_LOADER
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
            Promise<T> p = new Promise<T>();
            if (string.IsNullOrEmpty(path)) {
                p.Reject(new System.Exception("Path is null or empty"));
                return p;
            }

            IPromise<Object> objPromise = FindInCache(path);
            if (objPromise != null) {
#if RESOURCE_LOADER
                DLog.Log(null, "ResourcesLoadSync:Load: Cache hit '" + path + "'");
#endif
                objPromise.Then(o => {
#if RESOURCE_LOADER
                DLog.Log(null, "ResourcesLoadSync:Load: Cache hit: resolve value '" + path + "'");
#endif
                    p.Resolve((T) o);
                }).Catch(exception => p.Reject(exception));
                return p;
            }

#if RESOURCE_LOADER
                DLog.Log(null, "ResourcesLoadSync:Load: Cache miss '" + path + "'");
#endif
            IPromise<Object> promise = Load(path);
            var tuple = new System.Tuple<string, IPromise<Object>, Object>(path, promise, null);

            promise.Then(o => {
#if RESOURCE_LOADER
				sw.Stop();
				DLog.Log(null, "ResourcesLoadSync:Load: finish '" + path + "' tooks " + sw.ElapsedMilliseconds + " millis");
#endif
                ReplaceInCache(tuple.Item1, tuple.Item2, o);
                p.Resolve((T) o);
            }).Catch(
                    exception => {
#if RESOURCE_LOADER
                DLog.Log(null, "ResourcesLoadSync:Load: finish (failed to load) '" + path + "' tooks " + sw.ElapsedMilliseconds + " millis");
#endif
                        p.Reject(exception);
                    });
            return p;
        }

        private static void ReplaceInCache(string path, IPromise<Object> promise, Object obj) {
            for (int i = 0; i < cache.Count; i++) {
                if (cache[i].Item1.Equals(path)) {
                    cache[i] = new System.Tuple<string, IPromise<Object>, Object>(path, promise, obj);
                    return;
                }
            }
        }

        private static IPromise<Object> FindInCache(string path) {
            for (int i = 0; i < cache.Count; i++) {
                if (cache[i].Item1.Equals(path)) return cache[i].Item2;
            }

            return null;
        }

    #region Logic

        private static IPromise<Object> Load(string path) {
            Promise<Object> p = new AsyncLoad().Load(path);
            return p;
        }

        private interface ILoad {
            Promise<Object> Load(string path);
        }

        private class AsyncLoad : ILoad {
            public Promise<Object> Load(string path) {
                Promise<Object> p = new Promise<Object>();
                ResourceRequest rr = Resources.LoadAsync<Object>(path);

                Timing.RunCoroutine(WaitForDoneThenInvoke(rr, delegate {
                    if (rr.asset == null) {
                        Debug.LogError(new System.Exception($"Resource with path '{path}' does not exist"));
                        p.Reject(new System.Exception($"Resource with path '{path}' does not exist"));
                        return;
                    }

                    p.Resolve(rr.asset); 
                }));
                return p;
            }
        }

    #endregion

        private static IEnumerator<float> WaitForDoneThenInvoke(ResourceRequest rr, Action action) {
            while (!rr.isDone) {
                yield return Timing.WaitForOneFrame;
            }

            action.Invoke();
        }
    }
}