using BattleShipConsoleGame.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Factory
{
    // Factory Method Pattern: the abstract ShipFactory class and its concrete implementations create different types of ships
    public abstract class ShipFactory
    {
        public abstract Ship CreateShip();
    }
}
