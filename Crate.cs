
using SFML.Graphics;
using SFML.System;

namespace Video_Game {
    internal class Crate : Tile  {

        
        public bool isOnDiamond;


        public Crate(Vector2i position, float Size, bool IsOnDiamond = false) : base(position, Size) {

            SetTexture();
            isOnDiamond = IsOnDiamond; // Defaults to false 
        }

        public override void SetTexture() {
            // Set the default texture (can be overridden)
            texture = new Texture("Assets/img_crate.jpg");
            shape.Texture = texture;
        }

        public void MoveTo(Vector2i new_pos, bool IsOnDiamond) {
            UpdatePosition(new_pos, shape.Size.X);
            isOnDiamond = IsOnDiamond;

        }
    }
}
