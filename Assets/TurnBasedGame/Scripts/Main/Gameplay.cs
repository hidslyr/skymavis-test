using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] Spawner spawner;
    [SerializeField] TextAsset defaultBoard;

    private List<Character> characters;

    // Start is called before the first frame update
    void Start()
    {
        LoadAxiesFromText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadAxiesFromText()
    {
        int[][] boardNodes = Utils.GetSquareArrayFromText(defaultBoard.text);

        if (boardNodes.Length > board.GetSize())
        {
            Debug.LogError("Board size is smaller than loaded formation, " +
                "please make board bigger");

            return;
        }

        // To make sure the loaded formation always in center of the board
        // no matter the board size is
        int offset = (board.GetSize() - boardNodes.Length) / 2;

        for (int i = 0; i < boardNodes.Length; i++)
        {
            for (int j = 0; j < boardNodes[i].Length; j++)
            {
                BoardNodeType nodeType = (BoardNodeType)boardNodes[i][j];

                if (nodeType != BoardNodeType.Empty)
                {
                    SpawnCharacterOnBoard((BoardNodeType)boardNodes[i][j], 
                        new Vector2(i + offset, j + offset));
                }
            }
        }
    }

    private void SpawnCharacterOnBoard(BoardNodeType boardNodeType, Vector2 indexes)
    {
        Vector3 position = board.GetPositionAtIndexes(indexes);

        Character character = spawner.SpawnCharacter(boardNodeType, position);

        characters.Add(character);
    }
}
