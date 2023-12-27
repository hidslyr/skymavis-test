using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

namespace TurnBaseGame
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] List<string> characterPoolNames;

        public Character SpawnCharacter(BoardNodeType nodeType, Vector3 position)
        {
            GameObject go = EasyObjectPool.instance.GetObjectFromPool(
                characterPoolNames[(int)nodeType], position, Quaternion.identity);

            return go.GetComponent<Character>();
        }
    }
}
