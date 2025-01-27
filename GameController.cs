using SFML.Graphics;
using SFML.Window;

namespace Video_Game {

    internal class GameController {

        private RenderWindow gameWindow;
        private GameMap gameMap;
        private SideView sideView;

        //Used to create a new instance of the map, window and sideview. Loads the map, 
        public GameController() {
            gameMap = new GameMap();
            gameWindow = new RenderWindow(new VideoMode(gameMap.windowWidthpx, gameMap.windowHeightpx), "Assessment Program", Styles.Close);
            gameWindow.Closed += OnClosed;
            gameWindow.KeyPressed += OnKeyPressed;

            sideView = new SideView(gameMap);
            gameMap.SetSideView(sideView);
            gameMap.LoadMap(gameMap.current_level);
        }

        //Allows the user to close the window by clicking the X
        private void OnClosed(object? sender, EventArgs e) {
            gameWindow.Close();
        }

        //Calls different functions depending on different key presses, used to feed directions to the MovePlayer function.
        //Also allows the player to restart a level by pressing R, and exit the game by pressing Escape
        private void OnKeyPressed(object? sender, KeyEventArgs e) {

            switch (e.Code) {

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
                case Keyboard.Key.R:
                    Console.WriteLine("Level reset");
                    gameMap.RestartMap();
                    break;
                case Keyboard.Key.Escape:
                    gameWindow.Close();
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

            gameMap.DrawMap(gameWindow);
            sideView?.DrawSideView(gameWindow);
            gameWindow.Display();

        }

    }
}
