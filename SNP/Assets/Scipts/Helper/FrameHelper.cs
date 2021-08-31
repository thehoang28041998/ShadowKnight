namespace Scipts.Helper {
    public class FrameHelper {
        public const float FRAME_RATE = 60;
        public const float TIME_DELTA = 0.0166666666666667f;

        public int ToFrame(float second) {
            return (int) (second * TIME_DELTA);
        }
        
        public float ToSecond(float frame) {
            return frame * FRAME_RATE;
        }
    }
}