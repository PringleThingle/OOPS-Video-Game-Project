using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Game {

    public class GameController {

        public RenderWindow gameWindow;

        public GameController() {
            gameWindow = new RenderWindow(new VideoMode(900, 600), "Assessment Program", Styles.Close);
        }

        public void Run () {
            while (gameWindow.IsOpen) {

                gameWindow.DispatchEvents();

                UpdateGame();

                RenderGame();


            }
            gameWindow.Close();
        }

        private void UpdateGame() {

        }

        private void RenderGame() {

            gameWindow.Clear(new Color(0, 150, 200));

            gameWindow.Display();


        }

    }
}
