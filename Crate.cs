
namespace Video_Game {
    internal class Crate {

        public int x;
        public int y;
        public bool isOnDiamond;


        public Crate(int X, int Y, bool IsOnDiamond = false) {
            x = X;
            y = Y;
            isOnDiamond = IsOnDiamond; // Defaults to false unless specified
        }

        public void MoveTo(int new_x, int new_y, bool IsOnDiamond) {

            x = new_x;
            y = new_y;
            isOnDiamond = IsOnDiamond;

        }




    }
}
