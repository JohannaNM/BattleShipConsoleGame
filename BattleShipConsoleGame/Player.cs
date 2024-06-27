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
    //Observer Pattern
    public class Player : IObserver
    {
        public string Name { get; private set; }
        public GameBoard Board { get; private set; }

        public Player(string name)
        {
            Name = name;
            Board = new GameBoard();
            Board.Follow(this);
        }

        public void Update()
        {
            GameBoard.Welcome();
            //Console.WriteLine($"{Name}'s game board has been updated!");
            Board.DisplayBoard();
        }

        public void PlaceShips(Dictionary<string, ShipFactory> shipFactories)
        {
            foreach (var shipType in shipFactories.Keys)
            {
                Update();
                Console.WriteLine();
                Console.WriteLine($"  {Name}, place your {shipType}:");
                Ship ship = shipFactories[shipType].CreateShip();
                
                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.Write($"  Enter starting X coordinate for {shipType} (0-9): ");
                        int x = int.Parse(Console.ReadLine());
                        Console.Write($"  Enter starting Y coordinate for {shipType} (0-9): ");
                        int y = int.Parse(Console.ReadLine());
                        Console.Write("  Enter orientation (h for horizontal, v for vertical): ");
                        char orientation = char.Parse(Console.ReadLine().ToLower());

                        if (orientation != 'h' && orientation != 'v')
                        {
                            throw new FormatException("  Invalid orientation. Enter 'h' or 'v'.");
                        }

                        bool isHorizontal = orientation == 'h';
                        if (Board.PlaceShip(ship, x, y, isHorizontal))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("  Invalid placement. Try again.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"  Invalid input: {ex.Message}");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("  An error occurred. Please try again.");
                    }
                }
                
                
                
            }
        }


        public void Attack(Player opponent)
        {
            while (true)
            {
                try
                {
                    
                    Console.WriteLine($"  {Name}, make your attack:");
                    Console.Write("  Enter X coordinate (0-9): ");
                    int x = int.Parse(Console.ReadLine());
                    Console.Write("  Enter Y coordinate (0-9): ");
                    int y = int.Parse(Console.ReadLine());

                    if (x < 0 || x >= 10 || y < 0 || y >= 10)
                    {
                        throw new FormatException("  Coordinates must be between 0 and 9.");
                    }

                    string result = opponent.Board.Attack(x, y);
                    if (result == "  This coordinate has already been attacked. Try again.")
                    {
                        Console.WriteLine(result);
                        continue;
                    }
                    Console.WriteLine(result);
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"  Invalid input: {ex.Message}");
                }
                catch (Exception)
                {
                    Console.WriteLine("  An error occurred. Please try again.");
                }
            }
        }

        public bool HasLost()
        {
            return Board.AllShipsSunk();
        }
    }
}
