using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Character
{
    public override bool CanMove()
    {
        return false;
    }
}
