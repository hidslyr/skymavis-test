using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : BasicStateGameplay
{
    [SerializeField] Board board;

    
    private List<Character> characters = new List<Character>();

    protected override void OnEnterLoading()
    {
        characters = board.LoadDefaultBoard();
    }
}
