using System;
using Scipts.Movement.Model;
using UnityEngine;

namespace Scipts.Movement.Request {
    public struct RunRequest : IRequest {
        public readonly Vector3 direction;

        public RunRequest(Vector3 direction) {
            this.direction = direction;
        }
        
        public Tuple<RequestType, MovementAction>[] Rule {
            get => new[] {
                new Tuple<RequestType, MovementAction>(RequestType.Dash, MovementAction.AbortCurrent), // 
                new Tuple<RequestType, MovementAction>(RequestType.Run, MovementAction.AllowCurrent), // allow current 
            };
        }

        public RequestType RequestType {
            get => RequestType.Run;
        }
    }
}