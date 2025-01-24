using SFML.Graphics;
using SFML.System;
using System.Timers;


namespace Video_Game {
    internal class SideView : RectangleShape {

        private RectangleShape[,] sideView;
        private Font basic_font;
        private Text title_text;
        private Text timer_text;

        public SideView() {

            sideView = new RectangleShape[2, 9];

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 2; x++) {

                    sideView[x, y] = new RectangleShape();
                    sideView[x, y].Size = new Vector2f(100, 100);
                    sideView[x, y].Position = new Vector2f(900 + (x * 100f), y * 100f);
                    this.Position = new Vector2f(900, 0);
                    sideView[x, y].Texture = new Texture("Assets/img_sideview.jpg");
                }

                int minutes = 0;
                int seconds = 0;

                basic_font = new Font("Assets/basic_font.ttf");
                title_text = new Text("ANGRY BOX", basic_font);
                title_text.Position = this.Position + new Vector2f(20f, 20f);
                title_text.OutlineThickness = 0.5f;
                title_text.OutlineColor = Color.White;
                
                timer_text = new Text((Convert.ToString(minutes) + ":" + Convert.ToString(seconds)), basic_font);
                timer_text.Position = this.Position + new Vector2f(40f, 40f);

            }
        }

        public void DrawSideView(RenderWindow window) {

            for (int y = 0; y < 9; y++) {

                for (int x = 0; x < 2; x++) {

                    window.Draw(sideView[x, y]);
                }
            }

            window.Draw(title_text);
        }
    }
}
