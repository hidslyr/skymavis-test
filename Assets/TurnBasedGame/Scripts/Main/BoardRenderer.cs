using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] int boardDefaultSize;
    [SerializeField] Material boardMaterial;

    public void RescaleBoard(int boardSize)
    {
        float multiplyToBase = (float)boardSize / boardDefaultSize;
        this.transform.localScale = new Vector3(multiplyToBase, 1, multiplyToBase);
        boardMaterial.mainTextureScale = Vector2.one * multiplyToBase;
    }
}
