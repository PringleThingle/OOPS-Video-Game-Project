using SFML.Graphics;
using SFML.System;
using System.Timers;


namespace Video_Game {
    internal class SideView : RectangleShape {

        private RectangleShape[,] sideView;
        private Font? basic_font;
        private Text? title_text;
        private Text? level_text;
        private Text? player_score;
        private Text? timer_text;

        public SideView(GameMap gameMap) {

            sideView = new RectangleShape[2, 9];

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 2; x++) {

                    sideView[x, y] = new RectangleShape();
                    sideView[x, y].Size = new Vector2f(100, 100);
                    sideView[x, y].Position = new Vector2f(900 + (x * 100f), y * 100f);
                    this.Position = new Vector2f(900, 0);
                    sideView[x, y].Texture = new Texture("Assets/img_sideview.jpg");
                }
            }

            int minutes = 0;
            int seconds = 0;

            basic_font = new Font("Assets/basic_font.ttf");
            title_text = new Text("ANGRY BOX", basic_font);
            title_text.Position = this.Position + new Vector2f(20f, 20f);
            title_text.OutlineThickness = 0.5f;
            title_text.OutlineColor = Color.White;

            level_text = new Text("Level " + gameMap.current_level, basic_font);
            level_text.Position = this.Position + new Vector2f(50f, 60f);
            level_text.OutlineThickness = 0.3f;
            level_text.OutlineColor = Color.White;

            player_score = new Text("Score: " + gameMap.player_moves, basic_font);
            player_score.Position = this.Position + new Vector2f(50f, 100f);
            player_score.OutlineThickness = 0.2f;
            player_score.OutlineColor = Color.White;
        }

        public void DrawSideView(RenderWindow window) {

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 2; x++) {

                    window.Draw(sideView[x, y]);
                }
            }

            window.Draw(title_text);
            window.Draw(level_text);
            window.Draw(player_score);
        }

        public void UpdateLevelText(int currentLevel) {
            if (level_text != null) {
                level_text.DisplayedString = "Level " + currentLevel;
            }
        }

        public void UpdatePlayerScore(int playerScore) {
            if (player_score != null) {
                player_score.DisplayedString = "Score " + playerScore;
            }
        }
    }
}
