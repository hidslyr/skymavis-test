using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] TextAsset defaultBoard;

    // Start is called before the first frame update
    void Start()
    {
        LoadAxiesFromText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadAxiesFromText()
    {
        int[][] boardPositions = Utils.GetSquareArrayFromText(defaultBoard.text);
    }
}
