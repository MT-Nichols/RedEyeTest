using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


//This class carries the user data between scenes.
//Important classes needed for the save data system are also stored in this script.
public class PersistentInfo_Script : MonoBehaviour
{
    public UserStatLine m_UserData = new UserStatLine();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

/*Full Disclosure: The SaveGameSystem and SaveGame classes were copied straight from the following url: https://nielson.dev/2015/09/saving-games-in-unity
 */
public static class SaveGameSystem
{
    public static bool SaveGame(SaveGame saveGame, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(GetSavePath(name), FileMode.Create))
        {
            try
            {
                formatter.Serialize(stream, saveGame);
            }
            catch (Exception)
            {
                return false;
            }
        }

        return true;
    }

    public static SaveGame LoadGame(string name)
    {
        if (!DoesSaveGameExist(name))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(GetSavePath(name), FileMode.Open))
        {
            try
            {
                return formatter.Deserialize(stream) as SaveGame;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public static bool DeleteSaveGame(string name)
    {
        try
        {
            File.Delete(GetSavePath(name));
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool DoesSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }

    private static string GetSavePath(string name)
    {
        //Default path is: C:\Users\[username]\AppData\LocalLow\[companyname]\[projectname]
        Console.WriteLine(Path.Combine(Application.persistentDataPath, name + ".sav"));
        return Path.Combine(Application.persistentDataPath, name + ".sav");

        //For in engine testing only, comment out below prior to building.
        //return Path.Combine(@"C:\Users\Nichols\Documents\RedEye\RedEyeTest", name + ".sav");
    }
}

[Serializable]
public abstract class SaveGame
{
}

[Serializable]
public class LeaderboardData : SaveGame
{
    public List<UserStatLine> m_LeaderList;

    public LeaderboardData()
    {
        m_LeaderList = new List<UserStatLine>();
    }

    public int FindUser(UserStatLine user)
    {
        return m_LeaderList.FindIndex(x => x.m_UserName == user.m_UserName);
    }

    public void SortLeaderboard()
    {
        //Sort List based on score.
        m_LeaderList.Sort(delegate (UserStatLine x, UserStatLine y) {
            if (x.m_Score >= y.m_Score) { return -1; }
            else { return 1; }
        });

        //Update rank variables.
        for(int i =0; i< m_LeaderList.Count; i++)
        {
            m_LeaderList[i].m_Rank = i + 1;
        }
    }

    public void AddUser(UserStatLine user)
    {
        m_LeaderList.Add(user);
    }

    public UserStatLine GetUser(int index)
    {
        return m_LeaderList[index];
    }

    public UserStatLine GetUser(string name)
    {
        return m_LeaderList.Find(x => x.m_UserName == name);
    }
}

[Serializable]
public class UserStatLine
{
    public int m_Rank;
    public int m_Score;
    public string m_UserName;

    public UserStatLine()
    {
        m_Rank = -1;
        m_Score = 0;
        m_UserName = "Guest";
    }
}
