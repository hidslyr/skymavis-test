using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class CharacterRenderer : MonoBehaviour
{
    [SerializeField] AxieFigure axieFigure;

    public void Jump()
    {
        axieFigure.DoJumpAnim();
    }

    public void Attack()
    {
        axieFigure.DoAttackMeleeAnim();
    }
}
