using BattleShipConsoleGame.Factory;
using BattleShipConsoleGame.Interface;
using BattleShipConsoleGame.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame
{
    public class AiPlayer : Player
    {
        private Random random = new Random();
        private List<(int x, int y)> hitTargets = new List<(int x, int y)> ();
        private Queue<(int x, int y)> targetQueue = new Queue<(int x, int y)> ();

        public AiPlayer(string name) : base(name) { }
        
        new public void PlaceShips(Dictionary<string, ShipFactory> shipFactories)
        {
            foreach (var shipType in shipFactories.Keys)
            {
                Ship ship = shipFactories[shipType].CreateShip();
                while (true)
                {
                    int x = random.Next(0, 10);
                    int y = random.Next(0, 10);
                    bool isHorizontal = random.Next(0, 2) == 0;

                    if (Board.PlaceShip(ship, x, y, isHorizontal))
                    {
                        break;
                    }
                }
            }
        }

        new public void Attack(Player opponent)
        {
            while (true)
            {
                int x, y;

                if (targetQueue.Count > 0)
                {
                    (x, y) = targetQueue.Dequeue();
                }
                //else
                //{
                //    do
                //    {
                //        x = random.Next(0, 10);
                //        y = random.Next(0, 10);
                //    } 
                //    while (opponent.Board.Attack(x, y) == "This coordinate has already been attacked. Try again.");
                //}
                else
                {
                    
                    
                        x = random.Next(0, 10);
                        y = random.Next(0, 10);
                   
                }

                string result = opponent.Board.Attack(x, y);
                if (result != "  Invalid attack. Try again.")
                {
                    Console.WriteLine($"  {Name} attacked ({x}, {y}) and it was a {result.ToLower()}.");

                    if (result.Contains("Hit"))
                    {
                        AddAdjacentTargets(x, y);
                    }

                    break;
                }
            }
        }

        private void AddAdjacentTargets(int x, int y)
        {
            var potentialTargets = new List<(int, int)>
        {
            (x - 1, y),
            (x + 1, y),
            (x, y - 1),
            (x, y + 1)
        };

            foreach (var (nx, ny) in potentialTargets)
            {
                if (nx >= 0 && nx < 10 && ny >= 0 && ny < 10 && !hitTargets.Contains((nx, ny)))
                {
                    targetQueue.Enqueue((nx, ny));
                    hitTargets.Add((nx, ny));
                }
            }
        }

        public void DisplayHiddenBoard()
        {
            GameBoard.Welcome();
            Console.WriteLine($"  {Name}´s board:");
            Board.DisplayBoard(false);
        }
    }
}
