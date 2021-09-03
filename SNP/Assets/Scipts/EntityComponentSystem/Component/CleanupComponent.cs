namespace Scipts.EntityComponentSystem.Component {
    public struct CleanupComponent : IComponent{
        private bool immediatelyCleanup;
        private bool init;
        public float elapsed;

        public CleanupComponent(float duration) {
            this.init = true;
            this.elapsed = duration;
            this.immediatelyCleanup = false;
        }

        public void ForceCleanup() {
            init = true;
            immediatelyCleanup = true;
        }

        public bool IsEligibleToCleanup() {
            if (!init) {
                return false;
            }
            
            return immediatelyCleanup || !immediatelyCleanup && elapsed <= 0;
        }
    }
}