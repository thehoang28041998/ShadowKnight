using UnityEngine;

namespace Movement.Model {
    public interface IRequest {
        System.Tuple<RequestType, MovementAction>[] Rule { get; }
        RequestType RequestType { get; }
        Vector3 GetVelocity(float dt);
        Reason Reason { get; }
        bool IsFinish { get; }
        void Update(float dt);
        void Abort();
    }
}