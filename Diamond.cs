using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Game {
    internal class Diamond : Tile {

        public Diamond(Vector2i position, float Size) : base(position, Size) {

            SetTexture();

        }

        public override void SetTexture() {
            // Set the default texture (can be overridden)
            texture = new Texture("Assets/img_diamond.jpg");
            shape.Texture = texture;
        }
    }
}
