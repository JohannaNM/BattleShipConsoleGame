using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Ships
{
    // Factory Method Pattern
    public class Submarine : Ship
    {
        public override int Size => 1;

        public override char Symbol => 'S';
    }
}
