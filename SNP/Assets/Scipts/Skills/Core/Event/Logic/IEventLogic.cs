namespace Scipts.Skills.Core.Event.Logic {
    public interface IEventLogic {
        void Enter();
        void Update(float dt);
        void LateUpdate(float dt);
        void Exit();
        
        bool IsActived { get; }
        bool IsFinished { get; }
    }
}