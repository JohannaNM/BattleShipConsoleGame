using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Ships
{
    // Factory Method Pattern
    public abstract class Ship
    {
        public abstract int Size { get; }
        public abstract char Symbol { get; }
        private int health;

        public Ship()
        {
            health = Size;
        }
        public void TakeHit()
        {
            health--;
        }
        public bool IsSunk => health <= 0;
    }
}
