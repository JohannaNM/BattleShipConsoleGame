using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Interface
{
    // Observer pattern
    public interface ISubject
    {
        void Follow(IObserver observer);
        void UnFollow(IObserver observer);
        void Notify();
    }
}
