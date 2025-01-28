using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Video_Game {
    internal class GameMap {

        private RectangleShape[,]? map;
        private readonly Dictionary<string, Texture>? textures;
        private int player_x = 0;
        private int player_y = 0;
        private readonly Font basic_font = new Font("Assets/basic_font.ttf");
        private int map_width;
        private int map_height;
        private bool levelComplete = false;
        private bool levelIncremented = false;
        private readonly string map_folder = ("Levels/");
        private SideView? sideView;
        private bool canMove = true;
        private bool mapExists = false;

        public int player_total_moves;
        public int current_level = 1;
        public int player_moves;
        public uint windowHeightpx = 850;
        public uint windowWidthpx = 1050;
        public List<Crate> crates = new List<Crate>();

        public GameMap() {

            //Initialises textures so they can be used from memory
            try {
                textures = new Dictionary<string, Texture> {
                { "floor", new Texture("Assets/img_floor.jpg") },
                { "player", new Texture("Assets/img_player.jpg") },
                { "crate", new Texture("Assets/img_crate.jpg") },
                { "wall", new Texture("Assets/img_wall.jpg") },
                { "diamond", new Texture("Assets/img_diamond.jpg") },
                { "filled_crate", new Texture("Assets/img_filled_crate.jpg") },
                { "player_diamond", new Texture("Assets/img_player_diamond.jpg") },
                { "happy_player", new Texture("Assets/img_happy_player.jpg") }
            };
            }

            catch {
                Console.WriteLine("Textures failed to load, please try restarting.");
                return;
            }

        }

        //Used to take the created SideView from GameController and use it in GameMap
        public void SetSideView(SideView view) {
            sideView = view;
        }

        //Used to reset the players score for each level
        private void ResetScore() {
            player_moves = 0;
        }
        
        //Used to reset the players overall score
        private void ResetFinalScore() {
            player_total_moves = 0;
        }


        //This function loads / creates the map using data provided by a map file and an int representing the level
        //It also calculates the size tiles should be based on how many tiles there are in each axis on the map file and maintains their aspect ratio
        //It calculates the location in pixels where each tile should be (Needed to actually draw the tile)
        //It then sets each tiles texture in the 2D map array with different letters representing the different types of tile (C - Crate, D - Diamond, P - Player, F - Floor, W - Wall)
        public void LoadMap(int level) {

            if (textures == null) {
                Console.WriteLine("Textures did not load correctly");
                return;
            }

            canMove = true;
            levelIncremented = false;
            string[] all_lines = File.ReadAllLines("Levels/Map" + level + ".map");

            try {
                map_height = all_lines.Length;
                map_width = all_lines[0].Length;
            }

            catch {
                Console.WriteLine("Map is empty");
                return;
            }


            map = new RectangleShape[map_width, map_height];

            float tile_width = (windowWidthpx - 200) / map_width;
            float tile_height = windowHeightpx / map_height;

            float Size = tile_width;

            if (tile_width > tile_height) {
                tile_width = tile_height;
                Size = (int)tile_width;
            }

            if (tile_height > tile_width) {
                tile_height = tile_width;
                Size = (int)tile_height;
            }

            if (tile_width == tile_height) {
                Size = (int)tile_width;
            }

            bool playerOnMap = false;

            for (int y = 0; y < map_height; y++) {

                for (int x = 0; x < map_width; x++) {

                    map[x, y] = new RectangleShape();
                    map[x, y].Size = new Vector2f(Size, Size);
                    map[x, y].Position = new Vector2f(x * Size, y * Size);

                    if (textures == null) {
                        Console.WriteLine("Textures dont exist?");
                        return;
                    }

                    

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
                        default:
                            map[x, y].Texture = textures["floor"];
                            break;
                    }

                    if (map[x, y].Texture == textures["player"]) {

                        playerOnMap = true;
                    }
                }
            }

            if (!playerOnMap) {
                Console.WriteLine("Player not detected on the map");
            }

            mapExists = true;
        }

        //This function draws each map tile from the map array (created by LoadMap()) onto the game window
        public void DrawMap(RenderWindow window) {

            if (mapExists == false || map == null || textures == null) {
                Console.WriteLine("Map did not load correctly");
                Console.ReadLine();
                return;
            }

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



        //This function displays a popup when the player completes a level (All diamonds covered by crates)
        //The popup allows the user to retry the level or go to the next level, and displays a game complete screen when completing the last level.
        public void DisplayWinPopup(RenderWindow window) {

            canMove = false;

            if (map == null || textures == null) {
                Console.WriteLine("Map or textures do not exist");
                return;
            }


            for (int y = 0; y < map_height; y++) {

                for (int x = 0; x < map_width; x++) {

                    if (map[x, y].Texture == textures["player"]) {
                        map[x, y].Texture = textures["happy_player"];
                    }
                }
            }
            


            string[] files = Directory.GetFiles(map_folder);
            int mapCount = files.Length;

            //If the current level is the final level, then display the final level complete popup (instead of the normal level complete popup)
            if (mapCount == current_level) {

                //Final level complete popup
                RectangleShape win_popup = new RectangleShape(new Vector2f(500, 200));
                win_popup.FillColor = new Color(0, 0, 0, 200);
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
                        sideView?.UpdateLevelText(current_level);
                        sideView?.UpdatePlayerScore(player_moves);
                    }
                }

              //Else we display the normal level complete popup, allowing the user to progress to the next level
            } else {

                //Popup Window
                RectangleShape popup = new RectangleShape(new Vector2f(400, 200));
                popup.FillColor = new Color(0, 0, 0, 175);
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

                // Retry level button
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


        //Clears the map and resets the level completion status, used before loading a new map.
        public void ClearMap() {

            levelComplete = false;
            map = null;
            crates.Clear();
            Console.WriteLine("Map cleared!");
        }

        public void RestartMap() {

            ClearMap();
            LoadMap(current_level);
            Console.WriteLine("Map restarted!");
        }


        //Checks if any tiles are a diamond, if a tile is a diamond function returns nothing.
        //If no tiles are a diamond, then the player has won and levelComplete is set to true.
        public void CheckWin() {

            if (map == null || textures == null) {
                Console.WriteLine("Map or textures do not exist");
                return;
            }

            foreach (var tile in map) {

                if (tile.Texture == textures["diamond"] || tile.Texture == textures["player_diamond"]) {
                    return;
                }
            }

            levelComplete = true;
            Console.WriteLine("You win!");
        }

        //This function handles all the logic for the player moving and crates being pushed.
        //This logic includes textures changing when a crate is pushed onto a diamond, and when a player steps on a diamond.
        //This function also handles the logic to stop incorrect movement, such as moving out of bounds, or standing on a crate, or crates being pushed onto crates etc.
        //The direction the player moves (Based on which key they pressed) is fed in from GameController and then their actual position is updated in this function.
        //The player "score" is also updated each time the player successfully moves (not counting incorrect movement)
        public void MovePlayer(string direction) {

            if (map == null || textures == null) {
                Console.WriteLine("Map or textures do not exist");
                return;
            }

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

                Crate? crate = null; // Find the crate at the target position
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

            sideView?.UpdatePlayerScore(player_moves);

            Console.WriteLine("X:" + player_x + " Y:" + player_y);
            
        }
    }
}

