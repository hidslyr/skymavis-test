using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    public static int[][] GetSquareArrayFromText(string str)
    {
        string[] rows = str.Split('\n');
        int rowCount = rows.Length;

        int[][] ret = new int[rowCount][];

        for (int i = 0; i < rowCount; i++)
        {
            string row = rows[i];
            string[] nodes = row.Split(',');

            if (nodes.Length != rowCount)
            {
                Debug.LogError($"Invalid row. Column size # row size at row: \n {row}");
            }


            ret[i] = ConvertStringArrayToIntArray(nodes);
        }

        return ret;
    }

    private static int[] ConvertStringArrayToIntArray(string[] strs)
    {
        int[] ret = new int[strs.Length];

        for(int i = 0; i < ret.Length; i++)
        {
            int nodeValue;

            bool isNodeValueValid = int.TryParse(strs[i], out nodeValue);

            if (!isNodeValueValid)
            {
                Debug.LogError($"Node invalid {strs[i]}");
            }

            ret[i] = nodeValue;
        }

        return ret;
    }

    public static int[][] TransposeArray(int[][] array)
    {
        int size = array.Length;
        int[][] transposed = new int[size][];

        for (int i = 0; i < size; i++)
        {
            transposed[i] = new int[size];
            for (int j = 0; j < size; j++)
            {
                transposed[i][j] = array[j][i];
            }
        }

        return transposed;
    }
}

public static class Extension
{
    public static List<List<int>> Transpose(this List<List<int>> matrix)
    {
        List<List<int>> transposed = new List<List<int>>();

        int rowCount = matrix.Count;
        int colCount = matrix[0].Count;

        for (int i = 0; i < colCount; i++)
        {
            List<int> newRow = new List<int>();
            for (int j = 0; j < rowCount; j++)
            {
                newRow.Add(matrix[j][i]);
            }
            transposed.Add(newRow);
        }

        return transposed;
    }
}
