    public interface IComponent {
    }
    
    public interface IComponentDebug {
#if UNITY_EDITOR
        void OnGUI();
#endif
    }
