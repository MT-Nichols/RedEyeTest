using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject m_GameOver;
    public GameObject m_ScoreDisplay;
    public GameObject m_GManObj;

    private Text m_ScoreText;
    private Text m_GameOverText;
    private GameManger_Script m_GManager;

    // Start is called before the first frame update
    void Start()
    {
        m_GameOver.SetActive(false);
        m_ScoreText = m_ScoreDisplay.GetComponent<Text>();
        m_GameOverText = m_GameOver.GetComponent<Text>();
        m_GManager = m_GManObj.GetComponent<GameManger_Script>();

        GameManger_Script.OnPause += OnPause;
        Player_Controller.OnGameOver += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GManager.m_GState == GameManger_Script.GameState.running)
        {
            m_ScoreText.text = "Score: " + Mathf.FloorToInt(m_GManager.m_Score);
        }
    }

    private void OnDestroy()
    {
        GameManger_Script.OnPause -= OnPause;
        Player_Controller.OnGameOver -= OnGameOver;
    }

    private void OnPause(bool pause)
    {
        if (pause)
        {
            m_GameOverText.text = "PAUSED";
            m_GameOver.SetActive(true);
        }
        else
        {
            m_GameOver.SetActive(false);
        }
    }

    private void OnGameOver()
    {
        m_GameOverText.text = "GAME OVER";
        m_GameOver.SetActive(true);
    }
}
