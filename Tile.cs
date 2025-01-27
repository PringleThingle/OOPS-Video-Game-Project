
namespace Video_Game {

    //Tile class used to meet requirements of the brief

    internal class Tile {

        public int x;
        public int y;

        public Tile(int X, int Y) {
            x = X;
            y = Y;
        }

        //Function that is overridden in another class
        public void MoveTo(int new_x, int new_y, bool IsOnDiamond) {

            Console.WriteLine("Move the tile!");

        }

        //Function used to set the texture of a tile
        public virtual string SetTexture() {
            return "img_sideview.jpg";
        }
    }
}
