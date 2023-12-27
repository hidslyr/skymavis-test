using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseGame
{
    public class BoardLoader : MonoBehaviour
    {
        [SerializeField] Board board;
        [SerializeField] Spawner spawner;
        [SerializeField] TextAsset defaultBoard;

        public List<Character> LoadAxiesFromText()
        {
            List<Character> loadedCharacters = new List<Character>();

            int[][] boardNodes = Utils.GetSquareArrayFromText(defaultBoard.text);

            if (boardNodes.Length > board.GetSize())
            {
                Debug.LogError("Board size is smaller than loaded formation, " +
                    "please make board bigger");

                return null;
            }

            // To make sure the loaded formation always in center of the board
            // no matter the board size is
            int offset = (board.GetSize() - boardNodes.Length) / 2;

            boardNodes = Utils.TransposeArray(boardNodes);

            for (int i = 0; i < boardNodes.Length; i++)
            {
                for (int j = 0; j < boardNodes[i].Length; j++)
                {
                    BoardNodeType nodeType = (BoardNodeType)boardNodes[i][j];

                    if (nodeType != BoardNodeType.Empty)
                    {
                        //Debug.Log("load " + i + " " + j + " " + nodeType.ToString());
                        Character character = SpawnCharacterOnBoard((BoardNodeType)boardNodes[i][j],
                            new Location(i + offset, j + offset));

                        loadedCharacters.Add(character);
                    }
                }
            }

            return loadedCharacters;
        }

        private Character SpawnCharacterOnBoard(BoardNodeType boardNodeType, Location location)
        {
            Vector3 position = board.GetPositionAtBoardLocation(location);

            Character character = spawner.SpawnCharacter(boardNodeType, position);
            character.Init(location);

            return character;
        }
    }
}
