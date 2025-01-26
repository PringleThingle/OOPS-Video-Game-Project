using SFML.Graphics;
using SFML.System;

namespace Video_Game {

    internal class Wall : Tile {

        public Wall(Vector2i position, float Size) : base(position, Size) {

            SetTexture();

        }

        public override void SetTexture() {
            // Set the default texture (can be overridden)
            texture = new Texture("Assets/img_wall.jpg");
            shape.Texture = texture;
        }
    }
}