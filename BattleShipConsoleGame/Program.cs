using BattleShipConsoleGame.Factory;
using BattleShipConsoleGame.Interface;
using BattleShipConsoleGame.Ships;


namespace BattleShipConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, ShipFactory> shipFactories = new Dictionary<string, ShipFactory>
            {
                { "Battleship", new BattleshipFactory() },
                { "Cruiser", new CruiserFactory() },
                { "Destroyer", new DestroyerFactory() },
                { "Submarine", new SubmarineFactory() }
            };

            GameBoard.Welcome();
            //Console.WriteLine("Welcome to Battleship!");
            Console.Write("Enter your name: ");
            string player1Name = Console.ReadLine();
           
            Player player1 = new Player(player1Name);
            AiPlayer player2 = new AiPlayer("Computer");
            Console.Clear();
            player1.PlaceShips(shipFactories);
            Console.Clear();
            player2.PlaceShips(shipFactories);
            Console.Clear();
            

            Player currentPlayer = player1;
            AiPlayer opponentPlayer = player2;

            while (true)
            {
                Console.Clear();
                
                player2.DisplayHiddenBoard();
                player1.Attack(player2);
                if (player2.HasLost())
                {
                    Console.WriteLine($"{player1.Name} wins! All ships of {player2.Name} have been sunk.");
                    break;
                }
                else if (player1.HasLost())
                {
                    Console.WriteLine($"{player2.Name} wins! All ships of {player1.Name} have been sunk.");
                    break;
                }
                Console.ReadKey();
                Console.Clear();
                player2.Attack(player1);
                player1.Board.DisplayBoard();
                Console.ReadKey();
               
            }
            

        }
    }
}
