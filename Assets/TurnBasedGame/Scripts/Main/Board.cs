using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BoardNodeType
{
    Empty = 0,
    Attacker = 1,
    Defender = 2
}

public struct Location
{
    public int x;
    public int y;

    public Location(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}

public class Board : MonoBehaviour
{
    // Assume board is always a square
    [SerializeField] int boardSize;

    [SerializeField] BoardRenderer boardRenderer;
    [SerializeField] BoardLoader boardLoader;

    private float tileSize = 1;
    private List<Character> characters = new List<Character>();
    private List<List<int>> boardNodes;

    public int GetSize()
    {
        return boardSize;
    }

    public Vector3 GetPositionAtBoardLocation(Location indexes)
    {
        float xPos = (indexes.x - (float)boardSize / 2) * tileSize - tileSize / 2;
        float zPos = (indexes.y - (float)boardSize / 2) * tileSize - tileSize / 2;

        return new Vector3(xPos, 0, zPos);
    }

    public void LoadDefaultBoard()
    {
        characters =  boardLoader.LoadAxiesFromText();

        foreach (Character character in characters)
        {
            character.SetOnDeadListener(OnCharacterDead);
        }

        ConstructBoardNodes();
    }

    private void ConstructBoardNodes()
    {
        boardNodes = new List<List<int>>();

        for (int i = 0; i < boardSize; i++)
        {
            List<int> row = new List<int>();

            for (int j = 0; j < boardSize; j++)
            {
                row.Add(0);
            }

            boardNodes.Add(row);
        }

        FillNodesWithCharacterLocation(characters);
    }

    private void MoveCharacter(Character character, Location targetLocation)
    {
        Location currentLocation = character.GetCurrentLocation();
        Vector3 targetPosition = GetPositionAtBoardLocation(targetLocation);
        character.Move(targetPosition, targetLocation);

        // Swap the node as character moved
        int temp = boardNodes[currentLocation.x][currentLocation.y];
        boardNodes[currentLocation.x][currentLocation.y] = boardNodes[targetLocation.x][targetLocation.y];
        boardNodes[targetLocation.x][targetLocation.y] = temp;
    }

    private void FillNodesWithCharacterLocation(List<Character> characters)
    {
        foreach(Character character in characters)
        {
            Location location = character.GetCurrentLocation();
            boardNodes[location.x][location.y] = (int)character.GetBoardNodeType();
        }
    }

    private void OnCharacterDead(Character character)
    {
        characters.Remove(character);
    }


#if UNITY_EDITOR
    public void RegenerateBoard()
    {
        boardRenderer.RescaleBoard(boardSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoveAllAttackerOneToTheLeft();
        }
    }

    // For testing animation & function
    private void MoveAllAttackerOneToTheLeft()
    {
        foreach (Character character in characters.Where(x => x is Attacker))
        {
            Location nextLocation = character.GetCurrentLocation();
            nextLocation.x -= 1;

            MoveCharacter(character, nextLocation);
        }
    }
#endif
}
