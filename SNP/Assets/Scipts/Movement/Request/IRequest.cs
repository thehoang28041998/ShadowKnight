using Movement.Model;
using UnityEngine;

namespace Movement.Request {
    public interface IRequest {
        System.Tuple<RequestType, MovementAction>[] Rule { get; }
        RequestType RequestType { get; }
    }
}