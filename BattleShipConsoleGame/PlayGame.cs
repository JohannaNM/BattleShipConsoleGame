using BattleShipConsoleGame.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame
{
    // Singleton Pattern: PlayGame class is a singleton, ensuring only one instance exists to manage the game state.
    public class PlayGame
    {
        private static PlayGame? instance = null;
        private static readonly object padlock = new object();

        private PlayGame() { }

        public static PlayGame Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PlayGame();
                        }
                    }
                }
                return instance;
            }
        }

        public void Start()
        {
            do
            {
                PlaySingleGame();
            }
            while (PlayAgain());
        }

        private void PlaySingleGame()
        {
            Dictionary<string, ShipFactory> shipFactories = new Dictionary<string, ShipFactory>
            {
                { "Battleship", new BattleshipFactory() },
                { "Cruiser", new CruiserFactory() },
                { "Destroyer", new DestroyerFactory() },
                { "Submarine", new SubmarineFactory() }
            };

            GameBoard.Welcome();
            GameBoard.WelcomeMessage();
            Console.ReadKey();
            Console.Clear();
            GameBoard.Welcome();
            Console.Write("  Enter your name: ");
            string player1Name = Console.ReadLine();

            Player player1 = new Player(player1Name);
            AiPlayer player2 = new AiPlayer("Computer");
            Console.Clear();
            player1.PlaceShips(shipFactories);
            Console.Clear();
            player2.PlaceShips(shipFactories);
            Console.Clear();

            while (true)
            {
                Console.Clear();

                player2.DisplayHiddenBoard();
                player1.Attack(player2);
                if (player2.HasLost())
                {
                    Console.WriteLine($"  {player1.Name}, YOU WIN!");
                    Console.WriteLine($"  All enemy ships have been sunk!");
                    break;
                }
                Console.ReadKey();
                Console.Clear();

                player2.Attack(player1);
                player1.Board.DisplayBoard();
                if (player1.HasLost())
                {
                    Console.WriteLine("  You lose!");
                    Console.WriteLine("  All your ships have sunk.");
                    break;
                }
                Console.ReadKey();
            }
        }

        private bool PlayAgain()
        {
            while (true)
            {
                Console.Write("  Do you want to play again? (y/n): ");
                string response = Console.ReadLine().Trim().ToLower();
                if (response == "y" || response == "yes")
                {
                    return true;
                }
                else if (response == "n" || response == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("  Invalid input. Please enter 'y' for yes or 'n' for no.");
                }
            }
        }
        
    }
}
