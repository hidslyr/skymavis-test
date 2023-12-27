using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TurnBaseGame
{
    [Serializable]
    public struct Location
    {
        public int x;
        public int y;

        public Location(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Location Upper()
        {
            return new Location(x, y - 1);
        }

        public Location Under()
        {
            return new Location(x, y + 1);
        }

        public Location Right()
        {
            return new Location(x + 1, y);
        }

        public Location Left()
        {
            return new Location(x - 1, y);
        }

        public List<Location> GetNeighbours()
        {
            return new List<Location> { Upper(), Under(), Left(), Right() };
        }

        public static int Distance(Location a, Location b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}
