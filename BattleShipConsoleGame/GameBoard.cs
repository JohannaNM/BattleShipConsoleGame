using BattleShipConsoleGame.Interface;
using BattleShipConsoleGame.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleShipConsoleGame
{
    public class GameBoard : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();
        private char[,] board;
        private const int Size = 10;
        private List<Ship> ships = new List<Ship>();
       
  
        public void Follow(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        public void UnFollow(IObserver observer)
        {
            observers.Remove(observer);
        }

        public GameBoard() 
        {
            board = new char[Size, Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    board[i, j] = '~'; // water
                }
            } 
        }

        public bool PlaceShip(Ship ship, int x, int y, bool isHorizontal)
        {
            if (CanPlaceShip(ship, x, y, isHorizontal))
            {
                for (int i = 0; i < ship.Size; i++)
                {
                    if (isHorizontal)
                        board[x, y + i] = ship.Symbol;
                    else
                        board[x + i, y] = ship.Symbol;
                }
                ships.Add(ship);
                //Notify();
                Console.Clear();
                return true;
            }
            return false;
        }

        private bool CanPlaceShip(Ship ship, int x, int y, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int nx = isHorizontal ? x : x + i;
                int ny = isHorizontal ? y + i : y;
                if (nx >= Size || ny >= Size || board[nx, ny] != '~')
                {
                    return false;
                }
            }
            return true;
        }

        public string Attack(int x, int y)
        {

            if (board[x, y] == 'X' || board[x, y] == 'O')
            {
                return "  This coordinate has already been attacked. Try again.";
            }

            if (board[x, y] != '~' && board[x, y] != 'X' && board[x, y] != 'O')
            {
                char hitSymbol = board[x, y];
                board[x, y] = 'X'; // Hit
                Ship hitShip = ships.FirstOrDefault(s => s.Symbol == hitSymbol);
                hitShip?.TakeHit();
                

                if (hitShip != null && hitShip.IsSunk)
                {
                    return ConsoleColors.Red + $"  Hit and sunk a {hitShip.GetType().Name}!" + ConsoleColors.Reset;
                }
              
                return ConsoleColors.Red + "  Hit!" + ConsoleColors.Reset;
            }
            else
            {
                board[x, y] = 'O'; // Miss
                return ConsoleColors.Blue + "  Miss!" + ConsoleColors.Reset;
            }
            
        }

        public bool AllShipsSunk()
        {
            return ships.All(ship => ship.IsSunk);
        }

        public void DisplayBoard(bool revealShips = true)
        {
            //Welcome();
            
            Console.WriteLine();

            Console.Write("    ");
            for (int i = 0; i < Size; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < Size; i++)
            {
                Console.Write("  " + i + " ");
                for (int j = 0; j < Size; j++)
                {
                    char displayChar = board[i, j];
                    if (!revealShips && displayChar != 'X' && displayChar != 'O')
                    {
                        displayChar = '~';
                    }

                    if (displayChar == 'X')
                    {
                        Console.Write(ConsoleColors.Red + displayChar + ConsoleColors.Reset + " ");
                    }
                    else if (displayChar == '~')
                    {
                        Console.Write(ConsoleColors.Blue + displayChar + ConsoleColors.Reset + " ");
                    }
                    else if (displayChar == 'O')
                    {
                        Console.Write(ConsoleColors.Green + displayChar + ConsoleColors.Reset + " ");
                    }
                    else
                    {
                        Console.Write(ConsoleColors.Yellow + displayChar + ConsoleColors.Reset + " ");
                    }
                }

                if (i < ships.Count)
                {
                    var ship = ships.ElementAt(i);
                    if (ship.IsSunk)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write($"     {ship.GetType().Name} [{ship.Size}]");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

        }

        public static void Welcome()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  ██████╗  █████╗ ████████╗████████╗██╗     ███████╗███████╗██╗  ██╗██╗██████╗ ██████╗ ███████╗\r\n  ██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║     ██╔════╝██╔════╝██║  ██║██║██╔══██╗██╔══██╗██╔════╝\r\n  ██████╔╝███████║   ██║      ██║   ██║     █████╗  ███████╗███████║██║██████╔╝██████╔╝███████╗\r\n  ██╔══██╗██╔══██║   ██║      ██║   ██║     ██╔══╝  ╚════██║██╔══██║██║██╔═══╝ ██╔═══╝ ╚════██║\r\n  ██████╔╝██║  ██║   ██║      ██║   ███████╗███████╗███████║██║  ██║██║██║     ██║     ███████║\r\n  ╚═════╝ ╚═╝  ╚═╝   ╚═╝      ╚═╝   ╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝╚═╝╚═╝     ╚═╝     ╚══════╝\r\n                                                                                               ");
            Console.WriteLine();
        }

        public static void WelcomeMessage()
        {
            Console.WriteLine();
            Console.WriteLine("  This is a guessing game where you will place your battle ships");
            Console.WriteLine("  on a grid, and then shoot locations of the enemy grid trying");
            Console.WriteLine("  to find and sink all of their ships.The first player to sink");
            Console.WriteLine("  all the enemy ships wins.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to start the game.");
            
        }
    }
}
