using cetest.engine;
using Mindmagma.Curses;
using System.Runtime.CompilerServices;


namespace curses0
{
    internal unsafe class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            bool collided = false;
            Random rand = new Random();


            var Window = NCurses.InitScreen();              // Initialize Curses and our window
            NCurses.Raw();                                  // Disables buffering and control characters 
            NCurses.Keypad(Window, enable: true);           // Enables keypad (function keys, arrow keys, etc...)
            NCurses.NoEcho();                               // Disables echo


            int maxX, maxY;
            NCurses.GetMaxYX(Window, out maxY, out maxX);
            maxX--;
            maxY--;


            NCurses.NoDelay(Window, true);

            NCurses.Flash();
            // @@@@@@@@@]]]]]]]]]======-------v-v-v-[    MENU SETUP & OBJECT INITIALIZATION    ]-v-v-v-------======||||[[[[[@@@@@@@@@

            Render mainView = new(Window);

            //mainView.AddObject(new Square("Border", 0, 0, maxX, maxY, false, outlinePattern: " .:-=+=-:. "));

            // Effect init

            var p1 = mainView.AddObject(new Square("P9", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "@"));
            var p2 = mainView.AddObject(new Square("P8", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "%"));
            var p3 = mainView.AddObject(new Square("P7", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "#"));
            var p4 = mainView.AddObject(new Square("P6", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "*"));
            var p5 = mainView.AddObject(new Square("P5", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "+"));
            var p6 = mainView.AddObject(new Square("P4", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "="));
            var p7 = mainView.AddObject(new Square("P3", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "-"));
            var p8 = mainView.AddObject(new Square("P2", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: ":"));
            var p9 = mainView.AddObject(new Square("P1", maxX / 2, maxY / 2, 0, 0, false, outlinePattern: "."));

            double effectXspeed = 0;
            double effectYspeed = 0;
            int mainEffectSize = 12;
            double mainEffectSpeedOffset = 0.01;
            bool effectEnd = false;
            double effectEndVal = 1;

            // --

            // other elements

            var mainMenuGameName = mainView.AddObject(new Label("gamName", (maxX / 2) - 29 / 2, (maxY / 2) - 1, 29, 2, "- - -=#| terminal |#=- - -\n \nP      O      N      G", alignment: 1));


            var mainMenuInstuctions = mainView.AddObject( new Label("Instructions", (maxX / 2)-51/2, maxY-4, 51, 2, "=| Space - start | F1 - Quit | A - Up | D - Down |=", alignment:1));

            // @@@@@@@@@]]]]]]]]]======-------^-^-^----------------------------------------------^-^-^-------======||||[[[[[@@@@@@@@@


            // @@@@@@@@@]]]]]]]]]======-------v-v-v-[ GAME SCENE SETUP & OBJECT INITIALIZATION ]-v-v-v-------======||||[[[[[@@@@@@@@@

            Render gameView = new(Window);

            // movement
            var player = gameView.AddObject(new Square("Player", 1, (maxY / 2) - 2, 1, 6, true, fillChar: '@', outlinePattern: "@"));
            bool yDirection = false;
            bool xDirection = false;

            var oponent = gameView.AddObject(new Square("Oponent", maxX - 2, (maxY / 2) - 2, 1, 6, true, fillChar: '@', outlinePattern: "@"));

            var ball = gameView.AddObject(new Square("Ball", (maxX / 2) - 1 , (maxY / 2) - 1, 1, 1, true, fillChar: '@', outlinePattern: "@"));

            // collision, win areas

            var topCollider = gameView.AddObject(new Square("TopCollider", 0, 0, maxX, 0, false, outlinePattern: "#"));
            var bottomCollider = gameView.AddObject(new Square("BottomCollider", 0, maxY, maxX, 0, false, outlinePattern: "#"));

            var playerWinArea = gameView.AddObject(new Square("PlayerWinArea", 0, 0, 0, maxY, false, outlinePattern: "#"));
            var oponentWinArea = gameView.AddObject(new Square("OponentWinArea", maxX, 0, 0, maxY, false, outlinePattern: "#"));

            // graphics

            //var border = gameView.AddObject(new Square("Border", 0, 0, maxX, maxY, false, outlinePattern: " .:-=+=-:. "));

            var sepratorLine = gameView.AddObject(new Square("SepratorLine", maxX / 2, 0, 0, maxY, false, outlinePattern: "| "));
            gameView.MoveToBottom(sepratorLine);

            var playerScoreLabel = gameView.AddObject(new Label("PlayerScore", (maxX / 2) - 15, 2, 10, 1, "0", alignment: 2));
            var oponentScoreLabel = gameView.AddObject(new Label("OponentScore", (maxX / 2) + 6, 2, 10, 1, "0", alignment: 0));

            int playerScore = 0;
            int oponentScore = 0;

            topCollider.Show = false;
            bottomCollider.Show = false;
            playerWinArea.Show = false;
            oponentWinArea.Show = false;

            int speed = 50;

            var winMessage = gameView.AddObject(new Label("WinMessage", (maxX / 2) - (22 / 2) + 1, (maxY / 2) - 1, 22, 2, "", alignment: 1));
            winMessage.Show = false;

            // @@@@@@@@@]]]]]]]]]======-------^-^-^----------------------------------------------^-^-^-------======||||[[[[[@@@@@@@@@

            // - MENU -
            while (true)
            {
                if (Utils.IsKeyPressed((int)ConsoleKey.Spacebar) && effectEnd == false)
                {
                    effectEnd = true;
                    mainMenuGameName.Show = false;
                    mainMenuInstuctions.Show = false;
                    p9.ShowToggle();
                    p8.ShowToggle();
                    p7.ShowToggle();
                    p6.ShowToggle();
                    p5.ShowToggle();
                    p4.ShowToggle();
                    p3.ShowToggle();
                    p2.ShowToggle();
                    //break;
                }

                if (Utils.IsKeyPressed((int)ConsoleKey.F1))
                {
                    return;
                }

                effectXspeed = (effectXspeed + (0.03 - mainEffectSpeedOffset)) % 12.5;
                effectYspeed = (effectYspeed + (0.08 - mainEffectSpeedOffset)) % 12.5;

                for (int point = 0; point <= 8; point++)
                {
                    IRenderable o = mainView.GetRenderableObject($"P{point + 1}");
                    o.XPosition = (int)Math.Round(Math.Sin(effectXspeed + (0.2 * point)) * ((mainEffectSize + 25)) / effectEndVal) + maxX / 2;
                    o.YPosition = (int)Math.Round(Math.Cos(effectYspeed + (0.2 * point)) * (mainEffectSize) / effectEndVal) + maxY / 2;

                    if (effectEnd)
                    {
                        effectEndVal += 0.01;
                    }
                }

                if (effectEndVal >= 10)
                {
                    break;
                }
                else if (effectEndVal >= 5)
                {
                    p1.Width = 1;
                    p1.Height = 1;
                }

                mainView.RenderObjects();
                Thread.Sleep(5);
            }

            
            //gameView.RenderObjects();
            //Thread.Sleep(500);

            // - GAME -
            while (running)
            {
                // user input
                if (Utils.IsKeyPressed((int)ConsoleKey.A))
                {
                    player.YPosition -= 1;
                    collided = Collide.CollidesWithBottom(player, topCollider);
                    player.YPosition += 1 * Unsafe.As<bool, int>(ref collided);

                }
                if (Utils.IsKeyPressed((int)ConsoleKey.D))
                {
                    player.YPosition += 1;
                    collided = Collide.CollidesWithTop(player, bottomCollider);
                    player.YPosition -= 1 * Unsafe.As<bool, int>(ref collided);
                }
                if (Utils.IsKeyPressed((int)ConsoleKey.F1))
                {
                    running = false;
                }

                // ball logic
                xDirection ^= Collide.CollidesWithRight(ball, oponent) || Collide.CollidesWithLeft(ball, player);
                yDirection ^= Collide.CollidesWith(ball, topCollider) || Collide.CollidesWith(ball, bottomCollider);

                // score keeping
                if (Collide.CollidesWith(ball, playerWinArea))
                {
                    oponentScore++;
                    resetGame(ball, player, oponent, xDirection, yDirection, maxX, maxY, rand);

                    winMessage.ShowToggle();
                    winMessage.Text = "Oponent scored!\n+1 point to oponent.";
                    gameView.RenderObjects();
                    Thread.Sleep(3000);
                    winMessage.ShowToggle();

                    speed--;
                }
                else if (Collide.CollidesWith(ball, oponentWinArea))
                {
                    playerScore++;
                    resetGame(ball, player, oponent, xDirection, yDirection, maxX, maxY, rand);

                    winMessage.ShowToggle();
                    winMessage.Text = "Player scored!\n+1 point to player.";
                    gameView.RenderObjects();
                    Thread.Sleep(3000);
                    winMessage.ShowToggle();

                    speed--;
                }
                oponentScoreLabel.Text = oponentScore.ToString();
                playerScoreLabel.Text = playerScore.ToString();

                // ball movement
                ball.XPosition += 1 * (xDirection ? 1 : -1);
                ball.YPosition += 1 * (yDirection ? 1 : -1);

                // pc logic

                int positionError = ((ball.YPosition + ball.Height) / 2) - ((oponent.YPosition + oponent.Height) / 2);
                if (oponent.YPosition + positionError >= 1 && rand.Next(0, 101) <= 78)
                {
                    oponent.YPosition += positionError >= 0 ? 1 : -1;

                }
                else
                {
                    oponent.YPosition += positionError >= 0 ? -1 : 1;
                }

                if (oponent.YPosition < 0)
                {
                    oponent.YPosition += 1;
                }
                else if (oponent.YPosition + oponent.Height > maxY - 1)
                {
                    oponent.YPosition -= 1;
                }

                gameView.RenderObjects();

                Thread.Sleep(Math.Clamp(speed, 15, int.MaxValue));
            }

            NCurses.Refresh();
            NCurses.EndWin();
        }

        private static void resetGame(Square ball, Square player, Square oponent, bool xDirection, bool yDirection, int maxX, int maxY, Random rand)
        {
            ball.XPosition = (maxX / 2) - 1;
            ball.YPosition = (maxY / 2) - 1;
            player.YPosition = (maxY / 2) - (player.Height / 2);
            oponent.YPosition = (maxY / 2) - (oponent.Height / 2);
            xDirection = rand.Next(0, 2) == 1 ? true : false;
            yDirection = rand.Next(0, 2) == 1 ? true : false;
        }
    }
}

