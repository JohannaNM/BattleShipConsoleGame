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
        private List<(int x, int y)> hitTargets = new List<(int x, int y)>();
        private Queue<(int x, int y)> targetQueue = new Queue<(int x, int y)>();
        private int[,] probabilityMap;

        public AiPlayer(string name) : base(name)
        {
            InitializeProbabilityMap();
        }

        private void InitializeProbabilityMap()
        {
            probabilityMap = new int[10, 10];
        }

        private void UpdateProbabilityMap()
        {
            Array.Clear(probabilityMap, 0, probabilityMap.Length);

            var ships = new List<Ship> { new Battleship(), new Cruiser(), new Destroyer(), new Submarine() };

            foreach (var ship in ships)
            {
                if (ship.IsSunk)
                {
                    continue;
                }

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (CanPlaceShip(ship, x, y, true))
                        {
                            for (int i = 0; i < ship.Size; i++)
                            {
                                probabilityMap[x, y + i]++;
                            }
                        }
                        if (CanPlaceShip(ship, x, y, false))
                        {
                            for (int i = 0; i < ship.Size; i++)
                            {
                                probabilityMap[x + i, y]++;
                            }
                        }
                    }
                }
            }

            IncreaseProbabilityForAdjacentHits();
        }

        private bool CanPlaceShip(Ship ship, int x, int y, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int nx = isHorizontal ? x : x + i;
                int ny = isHorizontal ? y + i : y;
                if (nx >= 10 || ny >= 10 || Board.GetCell(nx, ny) != '~')
                {
                    return false;
                }
            }
            return true;
        }

        private void IncreaseProbabilityForAdjacentHits()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (Board.GetCell(x, y) == 'X')
                    {
                        foreach (var (nx, ny) in GetAdjacentCells(x, y))
                        {
                            if (nx >= 0 && nx < 10 && ny >= 0 && ny < 10 && Board.GetCell(nx, ny) == '~')
                            {
                                probabilityMap[nx, ny] += 2; // Increase probability for adjacent cells
                            }
                        }
                    }
                }
            }
        }

        private List<(int, int)> GetAdjacentCells(int x, int y)
        {
            return new List<(int, int)>
        {
            (x - 1, y),
            (x + 1, y),
            (x, y - 1),
            (x, y + 1)
        };
        }

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
            UpdateProbabilityMap();

            while (true)
            {
                int x, y;

                if (targetQueue.Count > 0)
                {
                    (x, y) = targetQueue.Dequeue();
                }
                else
                {
                    (x, y) = GetHighestProbabilityCell();
                }

                string result = opponent.Board.Attack(x, y);
                if (result != "  Invalid attack. Try again.")
                {
                    if (result == "  This coordinate has already been attacked. Try again.")
                    {
                        continue;
                    }

                    GameBoard.Welcome();
                    Console.WriteLine(ConsoleColors.Red + $"  {Name} attacked ({x}, {y}) and it was a {result}." + ConsoleColors.Reset);

                    if (result.Contains("Hit"))
                    {
                        AddAdjacentTargets(x, y);
                    }

                    break;
                }
            }
        }

        private (int, int) GetHighestProbabilityCell()
        {
            int maxProbability = -1;
            List<(int, int)> maxCells = new List<(int, int)>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (probabilityMap[x, y] > maxProbability && Board.GetCell(x, y) == '~')
                    {
                        maxProbability = probabilityMap[x, y];
                        maxCells.Clear();
                        maxCells.Add((x, y));
                    }
                    else if (probabilityMap[x, y] == maxProbability)
                    {
                        maxCells.Add((x, y));
                    }
                }
            }

            return maxCells[random.Next(maxCells.Count)];
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
            Console.WriteLine($"  {Name}'s board:");
            Board.DisplayBoard(false);
        }

    }
}
