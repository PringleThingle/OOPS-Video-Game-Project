using SFML.Graphics;
using SFML.System;

namespace Video_Game {

    internal class Tile {
        public Vector2i Map_Position;
        public Texture texture;
        public RectangleShape shape;
        private float Tilesize;

        public Tile (Vector2i position, float Size) {

            Map_Position = position;
            Tilesize = Size;
            texture = new Texture("Assets/img_newsideview.jpg"); //Default to floor texture
            shape = new RectangleShape(new Vector2f(Size, Size));
            shape.Position = GetPixelPosition();
            shape.Texture = texture;

        }

        public virtual void SetTexture() {
            // Set the default texture (can be overridden)
            texture = new Texture("Assets/img_newsideview.jpg");
            shape.Texture = texture;
        }

        public void UpdatePosition(Vector2i newPosition, float Size) {
            Map_Position = newPosition; // Update grid position
            shape.Position = GetPixelPosition();
        }

        public Vector2f GetPixelPosition() {
            return new Vector2f(Map_Position.X * Tilesize, Map_Position.Y * Tilesize);
        }

        public void Draw(RenderWindow window) {
            window.Draw(shape);
        }


    }
}
