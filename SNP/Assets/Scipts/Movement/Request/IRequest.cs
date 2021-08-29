using Scipts.Movement.Model;

namespace Scipts.Movement.Request {
    public interface IRequest {
        System.Tuple<RequestType, MovementAction>[] Rule { get; }
        RequestType RequestType { get; }
    }
}