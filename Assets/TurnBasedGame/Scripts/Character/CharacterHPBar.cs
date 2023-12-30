using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TurnBaseGame
{
    public class CharacterHPBar : MonoBehaviour
    {
        [SerializeField] SpriteRenderer fillRenderer;
        [SerializeField] Transform scaleRoot;
        [SerializeField] Gradient colorGradient;

        [SerializeField] float animateDuration;

        public void SetHealthBarTo(float percentage)
        {
            scaleRoot.DOScaleX(percentage, animateDuration * Gameplay.gameplaySpeed);

            Color desiredColor = colorGradient.Evaluate(percentage);

            fillRenderer.DOColor(desiredColor, animateDuration * Gameplay.gameplaySpeed);
        }

        public float GetAnimateDuration()
        {
            return animateDuration * Gameplay.gameplaySpeed;
        }
    }
}
