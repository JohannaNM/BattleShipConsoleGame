using BattleShipConsoleGame.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Factory
{
    public class SubmarineFactory : ShipFactory
    {
        public override Ship CreateShip()
        {
            return new Submarine();
        }
    }
}
