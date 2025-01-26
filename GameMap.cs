using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Runtime.InteropServices;

namespace Video_Game {
    internal class GameMap {

        private Dictionary<string, Texture> textures;
        private int player_x = -1;
        private int player_y = -1;
        private int map_width;
        private int map_height;
        public uint windowHeightpx = 900;
        public uint windowWidthpx = 1100;
        private Dictionary<Vector2i, Tile> tiles;
        private bool levelComplete = false;
        public int current_level = 1;
        public int player_moves = 0;
        public int player_total_moves = 0;
        private bool levelIncremented = false;
        private Font basic_font = new Font("Assets/basic_font.ttf");
        private string map_folder = ("Levels/");
        private SideView? sideView;
        private bool canMove = true;
        private Player player;
        private float Size;

        public GameMap() {

            textures = new Dictionary<string, Texture>();
            tiles = new Dictionary<Vector2i, Tile>();

            textures.Add("floor", new Texture("Assets/img_floor.jpg"));
            textures.Add("player", new Texture("Assets/img_player.jpg"));
            textures.Add("crate", new Texture("Assets/img_crate.jpg"));
            textures.Add("wall", new Texture("Assets/img_wall.jpg"));
            textures.Add("diamond", new Texture("Assets/img_diamond.jpg"));
            textures.Add("filled_crate", new Texture("Assets/img_filled_crate.jpg"));
            textures.Add("player_diamond", new Texture("Assets/img_player_diamond.jpg"));

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

            float tile_width = (windowWidthpx-200) / map_width;
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

                    Vector2i tilePos = new Vector2i(x, y);

                    switch (all_lines[y][x]) {
                        case 'W':
                            tiles[tilePos] = new Wall(new Vector2i(x, y), Size);
                            break;
                        case 'C':
                            tiles[tilePos] = new Crate(new Vector2i(x, y), Size, false);         
                            break;
                        case 'F':
                            tiles[tilePos] = new Floor(new Vector2i(x, y), Size);
                            break;
                        case 'D':
                            tiles[tilePos] = new Diamond(new Vector2i(x, y), Size);
                            break;
                        case 'P':
                            player = new Player(new Vector2i(x, y), Size);
                            tiles[tilePos] = player;
                            break;
                    }
                }
            }
        }

        public void DrawMap(RenderWindow window) {

            for (int y = 0; y < map_height; y++) {

                for (int x = 0; x < map_width; x++) {

                    tiles[new Vector2i(x, y)].Draw(window);
                }

                if (levelComplete) {
                    DisplayWinPopup(window);
                }
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
                        sideView.UpdateLevelText(current_level);
                        sideView.UpdatePlayerScore(player_moves);

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
                        sideView.UpdatePlayerScore(player_moves);

                    }
                }
            }



        }

        public void ClearMap() {

            levelComplete = false;
            tiles.Clear();
            Console.WriteLine("Map cleared!");
        }


        public void CheckWin() {

            if (tiles == null) {
                return;
            }

            foreach (var tile in tiles.Values) {

                if (tile is Diamond) {
                    return;
                } 
            }

            levelComplete = true;
            Console.WriteLine("You win!");
        }

        public void MovePlayer(string direction) {

            if (!canMove) return;

            player_moves++;
            player_total_moves++;
            sideView.UpdatePlayerScore(player_moves);

            Vector2i targetPosition = (Vector2i)(player.Map_Position);

            switch (direction) {

                case ("up"):
                    //targetY--;
                    targetPosition.Y--;
                    break;
                case ("down"):
                    //targetY++;
                    targetPosition.Y++;
                    break;
                case ("left"):
                    //targetX--;
                    targetPosition.X--;
                    break;
                case ("right"):
                    //targetX++;
                    targetPosition.X++;
                    break;
            }

            if (targetPosition.X < 0 || (map_width - 1) < targetPosition.X || targetPosition.Y < 0 || (map_height - 1) < targetPosition.Y) {
                return;
            }

            Crate? crate = tiles[targetPosition] as Crate;

            if (crate != null) {

                Console.WriteLine("Crate thing");

                Vector2i crateTargetPosition = new Vector2i((int)(targetPosition.X + (targetPosition.X - player.Map_Position.X)), (int)(targetPosition.Y + (targetPosition.Y - player.Map_Position.Y)));

                //If the crates target position is out of bounds, a wall, or another crate, then return (dont move the crate).
                if (crateTargetPosition.X < 0 || (map_width - 1) < crateTargetPosition.X || crateTargetPosition.Y < 0 || (map_height - 1) < crateTargetPosition.Y || tiles[crateTargetPosition] is Wall || tiles[crateTargetPosition] is Crate) {
                    return;
                }

                bool isOnDiamond = tiles[crateTargetPosition] is Diamond;

                tiles[crateTargetPosition] = crate;
                crate.MoveTo(new Vector2i(crateTargetPosition.X, crateTargetPosition.Y), isOnDiamond);

            }

            if (tiles[targetPosition] is Floor && tiles[player.Map_Position] is Floor) {

                //Replace players old position with a floor
                tiles[player.Map_Position] = new Floor(player.Map_Position, Size);

                tiles[targetPosition] = player;
                player.MoveTo(targetPosition);

            }

            CheckWin();
            Console.WriteLine("X: " + targetPosition.X);
            Console.WriteLine("Y: " + targetPosition.Y);
        }
            


    }
}

