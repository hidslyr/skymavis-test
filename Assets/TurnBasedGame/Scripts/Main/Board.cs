using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TurnBaseGame
{
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
        [SerializeField] BoardLoader boardLoader;

        private float tileSize = 1;
        private List<Character> characters = new List<Character>();
        private List<List<int>> boardNodes;
        private List<Character> blockedCharacters = new List<Character>();
        private List<Character> deadCharacters = new List<Character>();

        public int GetSize()
        {
            return boardSize;
        }

        public Vector3 GetPositionAtBoardLocation(Location indexes)
        {
            float xPos = (indexes.x - (float)boardSize / 2) * tileSize + tileSize / 2;
            float zPos = ((float)boardSize / 2 - indexes.y) * tileSize - tileSize / 2;

            return new Vector3(xPos, 0, zPos);
        }

        public void LoadDefaultBoard()
        {
            characters = boardLoader.LoadAxiesFromText();

            foreach (Character character in characters)
            {
                character.SetOnDeadListener(OnCharacterDead);
            }

            ConstructBoardNodes();
        }

        public void BothTeamsAttack()
        {
            foreach (Character character in characters)
            {
                Character target = FindAdjacentEnemy(character);

                if (target == null) // No adjacent enemy
                {
                    continue;
                }

                character.Attack(target);
            }
        }

        public void AttackersMove()
        {
            blockedCharacters.Clear();

            MoveAttackersTowardNearestDefender(GetAllAttackers());
            if (blockedCharacters.Count > 0)
            {
                MoveAttackersTowardNearestDefender(blockedCharacters, false);
            }
        }

        public void ClearDeadCharacters()
        {
            foreach(Character dead in deadCharacters)
            {
                characters.Remove(dead);
            }

            deadCharacters.Clear();
        }

        private void MoveAttackersTowardNearestDefender(IEnumerable<Character> attackers, bool canRetry = true)
        {
            foreach (Character attacker in attackers)
            {
                // Some might be blocked by teammates which have not moved yet
                // Stored and try to move again later
                if (IsBlockedByTeammates(attacker))
                {
                    if (canRetry)
                    {
                        blockedCharacters.Add(attacker);
                    }
                    else
                    {
                        break;
                    }

                    continue;
                }

                IEnumerable<Character> targets = NearestDefenders(attacker);

                foreach(Character target in targets)
                {
                    int distanceToDefender = attacker.Distance(target);

                    // Already in adjacent slot, wait for attack phase
                    if (distanceToDefender == 1)
                    {
                        break;
                    }

                    Pathfinding pf = new Pathfinding(boardNodes);
                    List<(int, int)> path = pf.FindPath((attacker.Location.x, attacker.Location.y),
                        (target.Location.x, target.Location.y));

                    // Nearest target may be blocked, try another one
                    if (path == null || path.Count < 2)
                    {
                        continue;
                    }

                    (int, int) nextMove = path.First();

                    MoveCharacter(attacker, new Location(nextMove.Item1, nextMove.Item2));
                    break;
                }
            }
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
            Location currentLocation = character.Location;
            Vector3 targetPosition = GetPositionAtBoardLocation(targetLocation);
            character.Move(targetPosition, targetLocation);

            // Swap the node as character moved
            int temp = boardNodes[currentLocation.x][currentLocation.y];
            boardNodes[currentLocation.x][currentLocation.y] = boardNodes[targetLocation.x][targetLocation.y];
            boardNodes[targetLocation.x][targetLocation.y] = temp;

            //LogBoardNodes();
        }

        private void FillNodesWithCharacterLocation(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                Location location = character.Location;
                boardNodes[location.x][location.y] = (int)character.GetBoardNodeType();
            }
        }

        private IEnumerable<Character> NearestDefenders(Character attacker)
        {
            return GetAllDefenders().OrderBy(x => x.Distance(attacker));
        }

        private bool IsBlockedByTeammates(Character character)
        {
            foreach(Location location in character.Location.GetNeighbours())
            {
                if (IsOutOfBound(location))
                {
                    continue;
                }

                // Has one way to exit
                if (boardNodes[location.x][location.y] != (int)character.GetBoardNodeType())
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsOutOfBound(Location location)
        {
            if (location.x >= boardSize || location.y >= boardSize)
            {
                return true;
            }

            return false;
        }

        private Character FindAdjacentEnemy(Character character)
        {
            foreach (Character it in characters)
            {
                if (character.Distance(it) == 1 && character.IsOnDifferentTeam(it))
                {
                    return it;
                }
            }

            return null;
        }

        private void OnCharacterDead(Character character)
        {
            deadCharacters.Add(character);
        }

        private IEnumerable<Character> GetAllAttackers()
        {
            return characters.Where(x => x is Attacker);
        }

        private IEnumerable<Character> GetAllDefenders()
        {
            return characters.Where(x => x is Defender);
        }

#if UNITY_EDITOR
        public void RegenerateBoard()
        {
            boardRenderer.RescaleBoard(boardSize);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveAllAttackerLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveAllAttackerRight();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveAllAttackerUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveAllAttackerDown();
            }
        }

        // For testing animation & function
        private void MoveAllAttackerLeft()
        {
            foreach (Character character in GetAllAttackers())
            {
                Location nextLocation = character.Location;
                nextLocation.x -= 1;

                MoveCharacter(character, nextLocation);
            }
        }

        private void MoveAllAttackerRight()
        {
            foreach (Character character in GetAllAttackers())
            {
                Location nextLocation = character.Location;
                nextLocation.x += 1;

                MoveCharacter(character, nextLocation);
            }
        }

        private void MoveAllAttackerUp()
        {
            foreach (Character character in GetAllAttackers())
            {
                Location nextLocation = character.Location;
                nextLocation.y -= 1;

                MoveCharacter(character, nextLocation);
            }
        }

        private void MoveAllAttackerDown()
        {
            foreach (Character character in GetAllAttackers())
            {
                Location nextLocation = character.Location;
                nextLocation.y += 1;

                MoveCharacter(character, nextLocation);
            }
        }

        private void LogBoardNodes()
        {
            List<List<int>> forVisualization = boardNodes.Transpose();
            string log = "";

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    log += $" {forVisualization[i][j]}";
                }

                log += "\n";
            }

            Debug.Log(log);
        }
#endif
    }
}
