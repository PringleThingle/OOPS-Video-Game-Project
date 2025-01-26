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
        public int current_level = 3;
        public int player_moves;
        public uint windowHeightpx = 900;
        public uint windowWidthpx = 1100;
        private Font basic_font = new Font("Assets/basic_font.ttf");
        private int map_width;
        private int map_height;
        private bool levelComplete = false;
        public int player_total_moves;
        private bool levelIncremented = false;
        private string map_folder = ("Levels/");
        private SideView? sideView;
        private bool canMove = true;

        public GameMap() {

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

        public void SetSideView(SideView view) {
            sideView = view;
        }

        public void ResetScore() {
            player_moves = 0;
        }

        public void ResetFinalScore() {
            player_total_moves = 0;
        }


        public void LoadMap(int level) {

            canMove = true;
            levelIncremented = false;
            string[] all_lines = File.ReadAllLines("Levels/Map" + current_level + ".map");

            map_height = all_lines.Length;
            map_width = all_lines[0].Length;

            map = new RectangleShape[map_width, map_height];

            float tile_width = (windowWidthpx - 200) / map_width;
            float tile_height = windowHeightpx / map_height;

            float Size = tile_width;

            if (tile_width > tile_height) {
                tile_width = tile_height;
                Size = (int)tile_width;
                Console.WriteLine("Tile width: " + tile_width);
            }

            if (tile_height > tile_width) {
                tile_height = tile_width;
                Size = (int)tile_height;
                Console.WriteLine("Tile height: " + tile_width);
            }

            if (tile_width == tile_height) {
                Size = (int)tile_width;
                Console.WriteLine("Tile width: " + tile_width);
            }

            for (int y = 0; y < map_height; y++) {

                for (int x = 0; x < map_width; x++) {

                    map[x, y] = new RectangleShape();
                    map[x, y].Size = new Vector2f(Size, Size);
                    map[x, y].Position = new Vector2f(x * Size, y * Size);

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

        public void DrawMap(RenderWindow window) {

            for (int y = 0; y < map_height; y++) {

                for (int x = 0; x < map_width; x++) {

                    window.Draw(map[x, y]);
                }
            }

            foreach (var crate in crates) {
                if (crate.isOnDiamond) {
                    map[crate.x, crate.y].Texture = textures[crate.SetTexture()];
                } else {
                    map[crate.x, crate.y].Texture = textures[crate.SetTexture()];
                }
            }

            if (levelComplete) {
                DisplayWinPopup(window);
            }
        }

        public void DisplayWinPopup(RenderWindow window) {

            canMove = false;

            // Get the files in the folder
            string[] files = Directory.GetFiles(map_folder);

            // Count the files
            int mapCount = files.Length;



            if (mapCount == current_level) {

                //Final level complete popup
                RectangleShape win_popup = new RectangleShape(new Vector2f(500, 200));
                win_popup.FillColor = new Color(0, 0, 0, 150);
                win_popup.Position = new Vector2f(((windowWidthpx - 200) / 2) - 250, ((windowHeightpx - 100) / 2) - 100);
                window.Draw(win_popup);

                // Final level complete text
                Text win_message = new Text("Congratulations, you beat all the levels!", basic_font, 24);
                win_message.FillColor = Color.White;
                win_message.Position = new Vector2f(windowWidthpx / 2 - 300, windowHeightpx / 2 - 100);
                window.Draw(win_message);

                // Score text
                Text win_score = new Text("Your Total Score: " + player_total_moves, basic_font, 24);
                win_score.FillColor = Color.White;
                win_score.Position = new Vector2f(windowWidthpx / 2 - 200, windowHeightpx / 2 - 60);
                window.Draw(win_score);

                // Play again button
                RectangleShape play_again_button = new RectangleShape(new Vector2f(150, 50));
                play_again_button.FillColor = Color.Green;
                play_again_button.Position = new Vector2f(windowWidthpx / 2 - 180, windowHeightpx / 2 - 10);
                play_again_button.OutlineThickness = 2;
                play_again_button.OutlineColor = Color.Black;
                window.Draw(play_again_button);

                Text buttonText = new Text("Play again", basic_font, 22);
                buttonText.FillColor = Color.White;
                buttonText.Position = new Vector2f(windowWidthpx / 2 - 155, windowHeightpx / 2);
                window.Draw(buttonText);

                // Check for mouse click on next level button
                if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                    Vector2i mousePosition = Mouse.GetPosition(window);
                    FloatRect buttonBounds = play_again_button.GetGlobalBounds();
                    if (buttonBounds.Contains(mousePosition.X, mousePosition.Y) && !levelIncremented) {

                        levelIncremented = false;
                        current_level = 1;
                        ClearMap();
                        LoadMap(current_level);
                        ResetScore();
                        ResetFinalScore();
                        sideView.UpdateLevelText(current_level);
                        sideView.UpdatePlayerScore(player_moves);
                    }
                }

            } else {

                //Popup Window
                RectangleShape popup = new RectangleShape(new Vector2f(400, 200));
                popup.FillColor = new Color(0, 0, 0, 150);
                popup.Position = new Vector2f(((windowWidthpx - 200) / 2) - 200, ((windowHeightpx - 100) / 2) - 100);
                window.Draw(popup);

                // Level complete text
                Text message = new Text("Level Complete!", basic_font, 24);
                message.FillColor = Color.White;
                message.Position = new Vector2f(windowWidthpx / 2 - 180, windowHeightpx / 2 - 100);
                window.Draw(message);

                // Score text
                Text score = new Text("Score: " + player_moves, basic_font, 24);
                score.FillColor = Color.White;
                score.Position = new Vector2f(windowWidthpx / 2 - 180, windowHeightpx / 2 - 60);
                window.Draw(score);

                // Next level button
                RectangleShape next_level_button = new RectangleShape(new Vector2f(196, 50));
                next_level_button.FillColor = Color.Green;
                next_level_button.Position = new Vector2f(windowWidthpx / 2 - 98, windowHeightpx / 2);
                next_level_button.OutlineThickness = 2;
                next_level_button.OutlineColor = Color.Black;
                window.Draw(next_level_button);

                Text buttonText = new Text("Next Level", basic_font, 22);
                buttonText.FillColor = Color.White;
                buttonText.Position = new Vector2f(windowWidthpx / 2 - 75, windowHeightpx / 2 + 10);
                window.Draw(buttonText);

                // Next level button
                RectangleShape retry_level_button = new RectangleShape(new Vector2f(196, 50));
                retry_level_button.FillColor = Color.Red;
                retry_level_button.Position = new Vector2f(windowWidthpx / 2 - 298, windowHeightpx / 2);
                retry_level_button.OutlineThickness = 2;
                retry_level_button.OutlineColor = Color.Black;
                window.Draw(retry_level_button);

                Text retry_button_Text = new Text("Retry Level", basic_font, 22);
                retry_button_Text.FillColor = Color.White;
                retry_button_Text.Position = new Vector2f(windowWidthpx / 2 - 275, windowHeightpx / 2 + 10);
                window.Draw(retry_button_Text);

                // Check for mouse click on next level button
                if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                    Vector2i mousePosition = Mouse.GetPosition(window);
                    FloatRect buttonBounds = next_level_button.GetGlobalBounds();
                    if (buttonBounds.Contains(mousePosition.X, mousePosition.Y) && !levelIncremented) {

                        levelIncremented = true;
                        Console.WriteLine("Next level");
                        current_level += 1;
                        ClearMap();
                        LoadMap(current_level);
                        ResetScore();
                        sideView?.UpdateLevelText(current_level);
                        sideView?.UpdatePlayerScore(player_moves);

                    }
                }

                // Check for mouse click on next level button
                if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                    Vector2i mousePosition = Mouse.GetPosition(window);
                    FloatRect buttonBounds = retry_level_button.GetGlobalBounds();
                    if (buttonBounds.Contains(mousePosition.X, mousePosition.Y) && !levelIncremented) {

                        levelIncremented = false;
                        Console.WriteLine("Retry level");
                        ClearMap();
                        LoadMap(current_level);
                        ResetScore();
                        sideView?.UpdatePlayerScore(player_moves);

                    }
                }
            }



        }

        public void ClearMap() {

            levelComplete = false;
            map = null;
            crates.Clear();
            Console.WriteLine("Map cleared!");
        }


        public void CheckWin() {

            if (map == null) {
                return;
            }

            foreach (var tile in map) {

                if (tile.Texture == textures["diamond"]) {
                    return;
                }
            }

            levelComplete = true;
            Console.WriteLine("You win!");
        }

        public void MovePlayer(string direction) {

            //Old player location replaced with floor texture
            //map[player_x, player_y].Texture = textures["floor"]

            if (canMove) {

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

                    player_moves++;
                    player_total_moves++;

                }



            }

            CheckWin();

            sideView?.UpdateLevelText(current_level);
            sideView?.UpdatePlayerScore(player_moves);

            Console.WriteLine("X: " + player_x);
            Console.WriteLine("Y: " + player_y);
            
        }
    }
}

