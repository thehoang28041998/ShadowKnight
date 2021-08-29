namespace Scipts.Helper {
    public class FrameHelper {
        private const int FRAME_RATE = 30;

        public int ToFrame(float second) {
            return (int) (second * FRAME_RATE);
        }
        
        public float ToSecond(float frame) {
            return frame / FRAME_RATE;
        }
    }
}