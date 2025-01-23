using SFML.Graphics;
using SFML.System;

namespace Video_Game {
    internal class GameMap {

        private RectangleShape[,] map;
        public enum MoveDirections { left, right, up, down }
        private int player_x = 0;
        private int player_y = 0;

        public GameMap() {

            map = new RectangleShape[9, 9];

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 9; x++) {

                    map[x, y] = new RectangleShape();
                    map[x, y].Size = new Vector2f(100,100);
                    map[x, y].Position = new Vector2f(x * 100f, y * 100f);
                    map[x, y].Texture = new Texture("Assets/img_floor.jpg");

                    
                }

            }

            map[player_x, player_y].Texture = new Texture("Assets/img_player.jpg");

        }

        public void DrawMap(RenderWindow window) {

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 9; x++) {

                    window.Draw(map[x, y]);

                }
            }
        }

        public void MovePlayer (string direction) {


            //Old player location replaced with floor texture
            map[player_x, player_y].Texture = new Texture("Assets/img_floor.jpg");

            switch (direction) {

                case ("up"):
                    if (player_y <= 0) {
                        player_y = 0;
                    } else {
                        player_y--;
                    }
                    break;
                case ("down"):
                    if (player_y >= 8) {
                        player_y = 8;
                    } else {
                        player_y++;
                    }
                    break;
                case ("left"):
                    if (player_x <= 0) {
                        player_x = 0;
                    } else {
                        player_x--;
                    }
                    break;
                case ("right"):
                    if (player_x >= 8) {
                        player_x = 8;
                    } else {
                        player_x ++;
                    }
                    break;
            }



            //New player location updated to show player texture
            map[player_x, player_y].Texture = new Texture("Assets/img_player.jpg");

        }



    }
}
