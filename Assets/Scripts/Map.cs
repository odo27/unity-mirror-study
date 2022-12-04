using System.Collections.Generic;

public class Map
{
    private static int MAP_SIZE = 15;

    public static int gameid;
    public static int mainturn;
    public static int subturn;
    private static int playerCount;
    private static List<List<int>> map;
    private static List<Dictionary<string, int>> playerStat;

    public static void UpdateTurn()
    {
        subturn++;
        if (subturn > playerCount)
        {
            mainturn++;
            subturn = 1;
        }
    }

    public static string PrintPlayerStat()
    {
        string result = "";
        for (int i = 1; i < playerStat.Count; i++)
        {
            result += "player" + i + "      ";
            result += playerStat[i]["health"] + "/" + playerStat[i]["maxHealth"] + "           ";
            result += playerStat[i]["strikingPower"] + "\n";
        }
        return result;
    }

    public static bool Attack(int subjectIdentity, int enemyIdentity)
    {
        playerStat[enemyIdentity]["health"] -= playerStat[subjectIdentity]["strikingPower"];
        if (playerStat[enemyIdentity]["health"] <= 0)
        {
            playerStat[subjectIdentity]["maxHealth"] += 20;
            playerStat[subjectIdentity]["health"] += 20;
            playerStat[subjectIdentity]["strikingPower"] += 1;
            return true;
        }
        return false;
    }

    public static void ClearAll()
    {
        mainturn = 1;
        subturn = 1;
        playerCount = 0;
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

        playerStat = new();
        Dictionary<string, int> emptySpace = new();
        playerStat.Add(emptySpace);
    }

    public static int AddNewUser()
    {
        Dictionary<string, int> initialStat = new()
        {
            {"maxHealth", 100 },
            {"health", 100},
            {"strikingPower", 5}
        };
        playerStat.Add(initialStat);
        return ++playerCount;
    }

    public static void Visit(int x, int y, int identity)
    {
        map[x][y] = identity;
    }

    public static void Clear(int x, int y)
    {
        map[x][y] = 0;
    }

    public static bool IsPlayer(int x, int y)
    {
        return map[x][y] != 0;
    }

    public static int GetPlayerIdentity(int x, int y)
    {
        return map[x][y];
    }

    public static bool IsInRange(int x, int y)
    {
        return (x >= 0 && x < MAP_SIZE && y >= 0 && y < MAP_SIZE);
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
