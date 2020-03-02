using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger_Script : MonoBehaviour
{
    public float m_Score = 0;
    //How long the game pauses before moving on to the end screen.
    public float m_EndTimer;
    //How quickly the score increases.
    public float m_ScoreMultiplier;
    public GameObject m_Player;
    public GameObject m_Spawner;
    
    public GameState m_GState;

    private PersistentInfo_Script m_PInfo;
    private UserStatLine m_LocalUser;

    public enum GameState
    {
        running,
        over,
        pause,
        next
    }

    public delegate void GamePause(bool pause);
    public static event GamePause OnPause;

    // Start is called before the first frame update
    void Start()
    {
        m_GState = GameState.running;
        m_PInfo = GameObject.Find("PersistentStorage").GetComponent<PersistentInfo_Script>();
        m_LocalUser = m_PInfo.m_UserData;

        m_LocalUser.m_Score = 0;

        Player_Controller.OnGameOver += OnGameOver;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_GState)
        {
            case GameState.running:
                m_Score += Time.deltaTime * m_ScoreMultiplier;

                if (Input.GetButtonDown("Pause"))
                {
                    OnPause(true);
                    m_GState = GameState.pause;
                }

                break;

            case GameState.over:

                
                m_LocalUser.m_Score = Mathf.FloorToInt(m_Score);
                m_GState = GameState.next;

                break;

            case GameState.next:
                if(m_EndTimer <= 0)
                {
                    SceneManager.LoadScene("EndScene");
                }
                else
                {
                    m_EndTimer -= Time.deltaTime;
                }
                break;

            case GameState.pause:

                if (Input.GetButtonDown("Pause"))
                {
                    OnPause(false);
                    m_GState = GameState.running;
                }

                break;

            default:

                break;
        }
        
    }

    private void OnGameOver()
    {
        m_GState = GameState.over;
    }

    
}
