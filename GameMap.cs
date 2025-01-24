using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Video_Game {
    internal class GameMap {

        private RectangleShape[,] map;
        private Dictionary<string, Texture> textures;
        private int player_x = 0;
        private int player_y = 0;
        public List<Crate> crates = new List<Crate>();

        public GameMap() {

            map = new RectangleShape[9, 9];

            textures = new Dictionary<string, Texture>();

            textures.Add("floor", new Texture("Assets/img_floor.jpg"));
            textures.Add("player", new Texture("Assets/img_player.jpg"));
            textures.Add("crate", new Texture("Assets/img_crate.jpg"));
            textures.Add("wall", new Texture("Assets/img_wall.jpg"));
            textures.Add("diamond", new Texture("Assets/img_diamond.jpg"));
            textures.Add("filled_crate", new Texture("Assets/img_filled_crate.jpg"));
            textures.Add("player_diamond", new Texture("Assets/img_player_diamond.jpg"));



/*            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 9; x++) {

                    map[x, y] = new RectangleShape();
                    map[x, y].Size = new Vector2f(100, 100);
                    map[x, y].Position = new Vector2f(x * 100f, y * 100f);
                    map[x, y].Texture = textures["floor"]; //FLOOR
                }

            }*/

/*            map[player_x, player_y].Texture = textures["player"]; //PLAYER 

            Crate crate1 = new Crate(5, 5, false);
            crates.Add(crate1);
            Crate crate2 = new Crate(7, 7, false);
            crates.Add(crate2);

            map[3, 4].Texture = textures["wall"]; //WALL
            map[7, 2].Texture = textures["diamond"]; //WALL*/

        }

        public void DrawMap(RenderWindow window) {

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 9; x++) {

                    window.Draw(map[x, y]);
                }
            }

            foreach (var crate in crates) {
                if (crate.isOnDiamond) {
                    map[crate.x, crate.y].Texture = textures["filled_crate"];
                } else {
                    map[crate.x, crate.y].Texture = textures["crate"];
                }
            }
        }


        public void LoadMap(string filename) {
            string[] all_lines = File.ReadAllLines(filename);

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 9; x++) {

                    map[x, y] = new RectangleShape();
                    map[x, y].Size = new Vector2f(100, 100);
                    map[x, y].Position = new Vector2f(x * 100f, y * 100f);

                    switch (all_lines[y][x]) {
                        case 'W':
                            map[x, y].Texture = textures["wall"];
                            break;
                        case 'C':
                            Crate newCrate = new Crate(x, y, false);
                            crates.Add(newCrate);
                            break;
                        case 'F':
                            map[x, y].Texture = textures["floor"];
                            break;
                        case 'D':
                            map[x, y].Texture = textures["diamond"];
                            break;
                        case 'P':
                            map[x, y].Texture = textures["player"];
                            player_x = x;
                            player_y = y;
                            break;
                    }
                }
            }
        }
         
        public void MovePlayer(string direction) {

            //Old player location replaced with floor texture
            //map[player_x, player_y].Texture = textures["floor"];


            int targetX = player_x, targetY = player_y;

            switch (direction) {

                case ("up"):
                    targetY--;
                    break;
                case ("down"):
                    targetY++;
                    break;
                case ("left"):
                    targetX--;
                    break;
                case ("right"):
                    targetX++;
                    break;
            }

            if (targetX < 0 || 8 < targetX || targetY < 0 || 8 < targetY) {
                return;
            }

            Crate crate = null; // Find the crate at the target position
            foreach (Crate currentCrate in crates) {
                if (currentCrate.x == targetX && currentCrate.y == targetY) {
                    crate = currentCrate;
                    break;
                }
            }

            if (crate != null) // There's a crate at the target position
            {
                // Calculate the target position for the crate
                int crateTargetX = targetX + (targetX - player_x);
                int crateTargetY = targetY + (targetY - player_y);

                // Check if the crate's target position is valid
                if (crateTargetX < 0 || crateTargetX > 8 || crateTargetY < 0 || crateTargetY > 8) {
                    
                    return; // Crate cannot be pushed out of bounds

                }

                if (map[crateTargetX, crateTargetY].Texture == textures["wall"] || map[crateTargetX, crateTargetY].Texture == textures["crate"] || map[crateTargetX, crateTargetY].Texture == textures["filled_crate"]) {
                    
                    return; // Crate cannot be pushed into a wall, another crate, or a filled crate

                }

                // Check if the crate is being pushed onto a diamond
                bool isOnDiamond = (map[crateTargetX, crateTargetY].Texture == textures["diamond"]);

                // Move the crate
                map[crate.x, crate.y].Texture = crate.isOnDiamond ? textures["diamond"] : textures["floor"]; // Restore the tile under the crate
                map[crateTargetX, crateTargetY].Texture = isOnDiamond ? textures["filled_crate"] : textures["crate"]; // Update texture for the crate's new position
                crate.MoveTo(crateTargetX, crateTargetY, isOnDiamond); // Update the crate's position and state
            
            }


            if (map[targetX, targetY].Texture == textures["wall"]) {
                return;
            }

            // Check if the target tile is valid for movement
            if (map[targetX, targetY].Texture == textures["floor"] || map[targetX, targetY].Texture == textures["diamond"]) {
                // Restore the previous tile to either a diamond or a floor

                if (map[player_x, player_y].Texture == textures["player_diamond"]) {
                    map[player_x, player_y].Texture = textures["diamond"];
                } else {
                    map[player_x, player_y].Texture = textures["floor"];
                }

                // Update the player's position
                player_x = targetX;
                player_y = targetY;

                if (map[targetX, targetY].Texture == textures["diamond"]) {
                    map[targetX, targetY].Texture = textures["player_diamond"];
                } else {
                    map[targetX, targetY].Texture = textures["player"];
                }

            }

            Console.WriteLine("X: " + player_x);
            Console.WriteLine("Y: " + player_y);
            
        }
    }
}

