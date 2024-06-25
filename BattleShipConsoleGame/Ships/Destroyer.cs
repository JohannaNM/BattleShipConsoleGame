using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Ships
{
    // Factory Method Pattern
    public class Destroyer : Ship
    {
        public override int Size => 2;

        public override char Symbol => 'D';
    }
}
