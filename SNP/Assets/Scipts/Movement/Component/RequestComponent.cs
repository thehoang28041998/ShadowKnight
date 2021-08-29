using System.Collections.Generic;
using Scipts.Movement.Model;
using Scipts.Movement.Request;
using UnityEditor;
#if UNITY_EDITOR

#endif

namespace Scipts.Movement.Component {
    public struct RequestComponent : IComponent , IComponentDebug{
        /// <summary>
        /// don't add, get, remove 
        /// </summary>
        private List<IRequest> requests;

        public void Init() {
            requests = new List<IRequest>();
        }

        public bool HasRequest(RequestType requestType) {
            foreach (var r in requests) {
                if (r.RequestType == requestType) return true;
            }

            return false;
        }

        public void AddRequest(IRequest request) {
            foreach (var r in requests) {
                if (r.RequestType == request.RequestType) return;
            }
            
            requests.Add(request);
        }

        public IRequest GetRequest(RequestType requestType) {
            foreach (var r in requests) {
                if (r.RequestType == requestType) return r;
            }

            return null;
        }

        public void RemoveRequest(RequestType requestType) {
            List<IRequest> remove = new List<IRequest>();
            foreach (var r in requests) {
                if (r.RequestType == requestType) {
                    remove.Add(r);
                }
            }

            foreach (var r in remove) {
                requests.Remove(r);
            }
        }

        public List<IRequest> Requests {
            get => requests;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            foreach (var r in requests) {
                EditorGUILayout.LabelField(r.RequestType.ToString());
            }
        }
#endif
    }
}