using RSG;

namespace Scipts.EntityComponentSystem.Model {
    public interface IPromiseComponent {
        IPromise PromiseInit { get; }
    }
}