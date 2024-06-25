using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Interface
{
    // Observer Pattern: the Player class implements IObserver and gets notified by the GameBoard (subject) when the game state changes
    public interface IObserver
    {
        void Update();
    }
}
