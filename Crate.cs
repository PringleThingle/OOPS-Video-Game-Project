namespace Video_Game {

    //Basic crate class so that I can say I used inheritance, polymorphism and overloading
    internal class Crate : Tile{

        public bool isOnDiamond;

        //Basic crate has a position and a bool for if its on a diamond or not
        public Crate(int X, int Y, bool IsOnDiamond = false) : base(X, Y) {
            x = X;
            y = Y;
            isOnDiamond = IsOnDiamond;
        }

        public new void MoveTo(int new_x, int new_y, bool IsOnDiamond) {

            x = new_x;
            y = new_y;
            isOnDiamond = IsOnDiamond;

        }

        //Sets the texture of the crate, overloaded from the default SetTexture function
        public override string SetTexture() {

            if (isOnDiamond) {
                 return "filled_crate";
            } else {
                return "crate";
            }
        }

    }
}
