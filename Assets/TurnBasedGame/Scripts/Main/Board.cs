using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardNodeType
{
    Empty = 0,
    Attacker = 1,
    Defender = 2
}

public class Board : MonoBehaviour
{
    // Assume board is always a square
    [SerializeField] int boardSize;

    [SerializeField] BoardRenderer boardRenderer;

    private float tileSize = 1;

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

    public Vector3 GetPositionAtIndexes(Vector2 indexes)
    {
        float xPos = (indexes.x - (float)boardSize / 2) * tileSize - tileSize / 2;
        float zPos = (indexes.y - (float)boardSize / 2) * tileSize - tileSize / 2;

        return new Vector3(xPos, 0, zPos);
    }

#if UNITY_EDITOR
    public void RegenerateBoard()
    {
        boardRenderer.RescaleBoard(boardSize);
    }
#endif
}
