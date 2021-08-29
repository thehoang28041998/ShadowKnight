using System;
using Scipts.Movement.Model;
using UnityEngine;

namespace Scipts.Movement.Request {
    public struct DashRequest : IRequest {
        public readonly float dashDistance;
        public readonly float dashDuration;
        public readonly Vector3 direction;

        public DashRequest(float dashDistance, float dashDuration, Vector3 direction) {
            this.dashDistance = dashDistance;
            this.dashDuration = dashDuration;
            this.direction = direction;
        }

        public Tuple<RequestType, MovementAction>[] Rule {
            get => new[] {
                new Tuple<RequestType, MovementAction>(RequestType.Dash, MovementAction.AllowCurrent), // 
                new Tuple<RequestType, MovementAction>(RequestType.Run, MovementAction.AllowCurrent), // allow current 
            };
        }

        public RequestType RequestType {
            get => RequestType.Dash;
        }
    }
}