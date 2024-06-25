using BattleShipConsoleGame.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Factory
{
    // Factory Method Pattern
    public class CruiserFactory : ShipFactory
    {
        public override Ship CreateShip()
        {
            return new Cruiser();
        }
    }
}
