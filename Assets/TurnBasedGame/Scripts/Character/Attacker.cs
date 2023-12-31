using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseGame
{
    public class Attacker : Character
    {
        public override bool IsOnDifferentTeam(Character other)
        {
            return !(other is Attacker);
        }

        public override string GetTeamName()
        {
            return "ATTACK";
        }
    }
}
