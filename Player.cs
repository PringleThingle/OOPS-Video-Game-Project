using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Game {
    internal class Player : Tile {

        public Player(Vector2i position, float Size) : base(position, Size) {

            SetTexture();

        }

        public override void SetTexture() {
            // Set the default texture (can be overridden)
            texture = new Texture("Assets/img_player.jpg");
            shape.Texture = texture;
        }

        public void MoveTo(Vector2i newPosition) {
            UpdatePosition(newPosition, shape.Size.X);
            SetTexture();
        }
    }
}
