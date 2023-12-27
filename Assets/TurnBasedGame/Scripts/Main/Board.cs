using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Assume board is always a square
    [SerializeField] int boardSize;

    [SerializeField] BoardRenderer boardRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSize()
    {
        return boardSize;
    }

#if UNITY_EDITOR
    public void RegenerateBoard()
    {
        boardRenderer.RescaleBoard(boardSize);
    }
#endif
}
