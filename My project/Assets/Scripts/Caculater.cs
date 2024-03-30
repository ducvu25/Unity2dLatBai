using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Caculater 
{
    public static int[,] scoreValue = { { 0, 5, 10, 20, 30, 50, 100},
                                        { 0, 10, 20, 30, 50, 100, 200}}; 
    public static int[] InitValue(int level, bool isLeft = true)
    {
        int[] result = new int[5];
        if (level == 4)
        {
            int[] C = { -1, -1, -2, -2, -3 };
            return ShuffleArray(C);
        }
        int[,,] type = {{{ 0, 1, 2, 2, 3},
                        {0, 2, 2, 3, 3},
                        {0, 0, 3, 3, 4},
                        {0, 0, 4, 5, 6} },
                        {{ 0, 1, 1, 2, 2},
                        {0, 2, 2, 3, 3},
                        {0, 0, 3, 3, 4},
                        {0, 0, 4, 5, 6} },
                        };
        

        return ShuffleArray(type, isLeft ? 1 : 0, level);
    }

    static int[] ShuffleArray(int[,,] array, int type, int level)
    {
        List<int> check = new List<int>();
        int[] result = { 0, 0, 0, 0, 0 };
        for (int i = 0; i < 5; i++)
        {
            while (true)
            {
                int k = (int)Random.Range(0, 5);
                if (!check.Contains(k))
                {
                    result[i] = array[type, level, k];
                    check.Add(k);
                    break;
                }
            }
        }
        return result;
    }
    static int[] ShuffleArray(int[] array)
    {
        List<int> check = new List<int>();
        int[] result = { 0, 0, 0, 0, 0 };
        for (int i = 0; i < 5; i++)
        {
            while (true)
            {
                int k = (int)Random.Range(0, 5);
                if (!check.Contains(k))
                {
                    result[i] = array[k];
                    check.Add(k);
                    break;
                }
            }
        }
        return result;
    }
}
