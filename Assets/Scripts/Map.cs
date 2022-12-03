using System.Collections.Generic;

public class Map
{
    private static int MAP_SIZE = 15;

    private static List<List<int>> map;


    public static void Clear()
    {
        map = new List<List<int>>();

        for (int i = 0; i < MAP_SIZE; i++)
        {
            List<int> row = new();

            for (int j = 0; j < MAP_SIZE; j++)
            {
                row.Add(0);
            }

            map.Add(row);
        }
    }

    public static void Visit(int x, int y)
    {
        map[x][y] = 1;
    }

    public static string PrintMap()
    {
        string result = "";

        for (int i = MAP_SIZE - 1; i >= 0; i--)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                result += map[j][i];
                result += " ";
            }
            result += "\n";
        }

        return result;
    }
}
