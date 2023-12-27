using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterHPBar : MonoBehaviour
{
    [SerializeField] SpriteRenderer fillRenderer;
    [SerializeField] Transform scaleRoot;
    [SerializeField] Gradient colorGradient;

    [SerializeField] float animateDuration;

    public void SetHealthBarTo(float percentage)
    {
        scaleRoot.DOScaleX(percentage, animateDuration);

        Color desiredColor = colorGradient.Evaluate(percentage);

        fillRenderer.DOColor(desiredColor, animateDuration);
    }

    public float GetAnimateDuration()
    {
        return animateDuration;
    }
}
