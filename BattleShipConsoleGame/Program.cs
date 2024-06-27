using BattleShipConsoleGame.Factory;
using BattleShipConsoleGame.Interface;
using BattleShipConsoleGame.Ships;


namespace BattleShipConsoleGame
{
    class Program
    {
        // Factory Method Pattern: the abstract ShipFactory class and its concrete implementations create different types of ships
        // Observer Pattern: the Player class implements IObserver and gets notified by the GameBoard (subject) when the game state changes
        // Singleton Pattern: PlayGame class is a singleton, ensuring only one instance exists to manage the game state.
        static void Main(string[] args)
        {
            PlayGame.Instance.Start();
        }   
    }
}
