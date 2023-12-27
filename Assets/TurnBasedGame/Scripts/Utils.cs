using System.Collections;
using System.Collections.Generic;
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
}
