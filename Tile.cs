using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Game {
    internal class Tile {

        public int x;
        public int y;

        public Tile(int X, int Y) {
            x = X;
            y = Y;
        }

        public virtual string SetTexture() {
            return "Default_Texture.jpg";
        }
    }
}
