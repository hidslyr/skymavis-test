using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseGame
{
    public class Defender : Character
    {
        public override bool IsOnDifferentTeam(Character other)
        {
            return !(other is Defender);
        }

        public override string GetTeamName()
        {
            return "DEFENSE";
        }
    }
}
