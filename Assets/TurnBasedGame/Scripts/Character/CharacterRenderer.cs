using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace TurnBaseGame
{
    public class CharacterRenderer : MonoBehaviour
    {
        [SerializeField] CustomizedAxieFigure axieFigure;

        public void Jump()
        {
            axieFigure.DoJumpAnim();
        }

        public void Attack()
        {
            axieFigure.DoAttackMeleeAnim();
        }

        public void TurnLeft()
        {
            axieFigure.TurnLeft();
        }

        public void TurnRight()
        {
            axieFigure.TurnRight();
        }

        public void PauseAnimation()
        {
            axieFigure.PauseAnimation();
        }

        public void ResumeAnimation()
        {
            axieFigure.ResumeAnimation();
        }

        public void HighLight()
        {
            axieFigure.HighLight();
        }

        public void UnHighLight()
        {
            axieFigure.UnHighLight();
        }
    }
}