using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame.Ships
{
    // Factory Method Pattern
    public class Battleship : Ship
    {
        public override int Size => 4;

        public override char Symbol => 'B';
    }
}
