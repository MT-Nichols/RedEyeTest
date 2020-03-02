using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardManager_Script : MonoBehaviour
{
    public List<Text> m_BoardList;
    public Text m_TopText;

    private LeaderboardData m_LocalData;
    private UserStatLine m_LocalUser;
    private PersistentInfo_Script m_PInfo;
    // Start is called before the first frame update
    void Start()
    {
        m_PInfo = GameObject.Find("PersistentStorage").GetComponent<PersistentInfo_Script>();
        m_LocalUser = m_PInfo.m_UserData;
        m_LocalData = SaveGameSystem.LoadGame("LeaderboardData") as LeaderboardData;

        if(m_LocalData != null)
        {
            string top;
            if (m_LocalData.FindUser(m_LocalUser) > -1)
            {
                m_LocalUser = m_LocalData.GetUser(m_LocalUser.m_UserName);
                top = m_LocalUser.m_UserName + " " + m_LocalUser.m_Score + " Rank: " + m_LocalUser.m_Rank;
            }
            else
            {
                top = "You have not scored yet.";
            }
            ChangeTopText(top);
            PopulateBoard();
        }
        else
        {
            string top = "No Leaderboard Available";
            ChangeTopText(top);
        }
    }

    private void PopulateBoard()
    {
        //Populates the leaderboard, number of leaderboard entires displayed is limited by the number of objects in m_BoardList.
        int pCount = Mathf.Min(m_LocalData.m_LeaderList.Count, m_BoardList.Count);

        for(int i = 0; i < pCount; i++)
        {
            ChangeBoardText(m_BoardList[i],m_LocalData.m_LeaderList[i]);
        }
    }

    private void ChangeBoardText(Text textObj, UserStatLine user)
    {
        textObj.text = user.m_Rank + " " + user.m_UserName + " " + user.m_Score;
    }

    public void ChangeTopText(string str)
    {
        m_TopText.text = str;
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
