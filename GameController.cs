using SFML.Graphics;
using SFML.Window;

namespace Video_Game {

    internal class GameController {

        private RenderWindow gameWindow;
        private GameMap gameMap;
        private SideView sideView;

        public GameController() {

            gameMap = new GameMap();
            gameWindow = new RenderWindow(new VideoMode(gameMap.windowWidthpx, gameMap.windowHeightpx), "Assessment Program", Styles.Close);
            gameWindow.Closed += OnClosed;
            gameWindow.KeyPressed += OnKeyPressed;

            sideView = new SideView(gameMap);
            gameMap.SetSideView(sideView);
            gameMap.LoadMap(gameMap.current_level);
        }

        private void OnClosed(object? sender, EventArgs e) {
            gameWindow.Close();
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e) {

            switch(e.Code) {

                case Keyboard.Key.W:
                    Console.WriteLine("Move up");
                    gameMap.MovePlayer("up");
                    break;
                case Keyboard.Key.A:
                    Console.WriteLine("Move left");
                    gameMap.MovePlayer("left");
                    break;
                case Keyboard.Key.S:
                    Console.WriteLine("Move down");
                    gameMap.MovePlayer("down");
                    break;
                case Keyboard.Key.D:
                    Console.WriteLine("Move right");
                    gameMap.MovePlayer("right");
                    break;
                case Keyboard.Key.Escape:
                    gameWindow.Close();
                    break;
                case Keyboard.Key.R:
                    gameMap.ClearMap();
                    gameMap.LoadMap(gameMap.current_level);
                    break;
            }
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

            gameWindow.Clear(new Color(Color.Black));

            gameMap.DrawMap(gameWindow);

            sideView.DrawSideView(gameWindow);

            gameWindow.Display();


        }

    }
}
