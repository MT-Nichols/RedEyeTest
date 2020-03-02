using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI_Manager : MonoBehaviour
{
    public InputField m_UserInput;
    public Text m_WelcomeText;

    private PersistentInfo_Script m_PInfo;
    private UserStatLine m_LocalUser;

    private void Start()
    {
        m_WelcomeText.text = "Welcome Guest";
        m_PInfo = GameObject.Find("PersistentStorage").GetComponent<PersistentInfo_Script>();
        m_LocalUser = m_PInfo.m_UserData;
        m_WelcomeText.text = "Welcome " + m_LocalUser.m_UserName;
    }

    public void LoadLevel(string gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }

    public void UpdateName()
    {
        m_LocalUser.m_UserName = m_UserInput.text;
        m_WelcomeText.text = "Welcome " + m_LocalUser.m_UserName;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
