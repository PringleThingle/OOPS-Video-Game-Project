
namespace Video_Game {
    internal class Crate : Tile{

        public bool isOnDiamond;

        public Crate(int X, int Y, bool IsOnDiamond = false) : base(X, Y){
            x = X;
            y = Y;
            isOnDiamond = IsOnDiamond; // Defaults to false unless specified
        }

        public void MoveTo(int new_x, int new_y, bool IsOnDiamond) {

            x = new_x;
            y = new_y;
            isOnDiamond = IsOnDiamond;

        }

        public override string SetTexture() {

            if (isOnDiamond) {
                 return "filled_crate";
            } else {
                return "crate";
            }
        }

    }
}
