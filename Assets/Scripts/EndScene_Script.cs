using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScene_Script : MonoBehaviour
{
    public Text m_CurrentScore;
    public Text m_BestScore;

    private PersistentInfo_Script m_PInfo;
    private LeaderboardData m_LocalData;
    private UserStatLine m_LocalUser;
    private int m_SessionScore;
    
    // Start is called before the first frame update
    void Start()
    {
        m_PInfo = GameObject.Find("PersistentStorage").GetComponent<PersistentInfo_Script>();
        m_LocalUser = m_PInfo.m_UserData;
        m_SessionScore = m_LocalUser.m_Score;

        m_LocalData = SaveGameSystem.LoadGame("LeaderboardData") as LeaderboardData;


        UpdateLeaderboard();
        DisplayScore(m_LocalUser);
    }

    public void DisplayScore(UserStatLine user)
    {
        m_CurrentScore.text = "Your Latest Score is: " + m_SessionScore;
        m_BestScore.text = user.m_UserName + " Highscore: " + user.m_Score + " Rank:" + user.m_Rank;
    }

    public void UpdateLeaderboard()
    {

        //Check if leaderboard exists.
        if(m_LocalData != null)
        {
            //Check if player has existing score.
            int pIndex = m_LocalData.FindUser(m_LocalUser);
            if (pIndex > -1)
            {
                //Check if Player has new highscore.
                if(m_LocalData.GetUser(pIndex).m_Score < m_LocalUser.m_Score)
                {
                    m_LocalData.GetUser(pIndex).m_Score = m_PInfo.m_UserData.m_Score;
                    //Sort Leaderboard to account for changes.
                    m_LocalData.SortLeaderboard();
                }
                //Update local stats.
                m_LocalUser = m_LocalData.GetUser(m_LocalUser.m_UserName);

            }
            else
            {
                //User doesn't exist, create new user.
                m_LocalData.AddUser(m_LocalUser);
                m_LocalData.SortLeaderboard();
                m_LocalUser = m_LocalData.GetUser(m_LocalUser.m_UserName);
            }

            //Save.
            //SaveGameSystem.DeleteSaveGame("LeaderboardData");
            SaveGameSystem.SaveGame(m_LocalData, "LeaderboardData");
            
        }
        else
        {
            //create new save data;
            m_LocalUser.m_Rank = 1;

            //Add player data to the leaderboard.
            LeaderboardData newLeaderboard = new LeaderboardData();
            newLeaderboard.AddUser(m_LocalUser);

            //Save Leaderboard
            SaveGameSystem.SaveGame(newLeaderboard, "LeaderboardData");
        }
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
