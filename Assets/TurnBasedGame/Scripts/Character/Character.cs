using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public abstract class Character : MonoBehaviour
{
    [SerializeField] AxieFigure axieFigure;

    public abstract bool CanMove();
}
