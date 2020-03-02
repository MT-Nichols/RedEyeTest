using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    private Transform m_PlayerTransform;
    private State m_State;
    private State m_PrevState;
    private float m_AirTime;
    private Sprite m_Sprite;
    private float m_GroundLevel = 0.0f;

    public float m_MaxAirTime;
    public float m_JumpPower;
    public float m_Gravity;
    public GameObject m_NormalImage;
    public GameObject m_FailImage;

    public enum State
    {
        onGround,
        airBorne,
        gameOver,
        pause
    }

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerTransform = GetComponent<Transform>();
        m_State = State.onGround;
        m_AirTime = m_MaxAirTime;
        m_NormalImage.SetActive(true);
        m_FailImage.SetActive(false);

        GameManger_Script.OnPause += OnPause;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case State.onGround:
                if (Input.GetButton("Jump"))
                {
                    Vector3 jump = new Vector3(0, m_JumpPower);
                    m_PlayerTransform.position += jump;
                    m_AirTime -= Time.deltaTime;
                    m_State = State.airBorne;
                }
                
                break;

            case State.airBorne:
                //stop player from jumping again if they release the button in mid air.
                if (Input.GetButtonUp("Jump"))
                {
                    m_AirTime = 0.0f;
                }

                if (Input.GetButton("Jump") && m_AirTime > 0.0f)
                {
                    Vector3 jump = new Vector3(0, m_JumpPower);
                    m_PlayerTransform.position += jump;
                    m_AirTime -= Time.deltaTime;
                }
                else
                {
                    //Check if player has reached ground level. Then re-enable all jump parameters.
                    if(m_PlayerTransform.position.y <= m_GroundLevel)
                    {
                        m_AirTime = m_MaxAirTime;
                        m_State = State.onGround;
                    }
                    else
                    {
                        //Falldown
                        Vector3 jump = new Vector3(0, m_Gravity);
                        m_PlayerTransform.position -= jump;
                    }
                }
                break;

            case State.pause:

                break;

            case State.gameOver:

                break;

            default:

                break;
        }
    }

    private void OnDestroy()
    {
        GameManger_Script.OnPause -= OnPause;
    }

    //Only thing it will collide with is an obstacle, so for now, any collision will end the game.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        m_State = State.gameOver;
        OnGameOver();
        m_FailImage.SetActive(true);
        m_NormalImage.SetActive(false);
    }

    public void OnPause(bool pause)
    {
        if (pause)
        {
            m_PrevState = m_State;
            m_State = State.pause;
        }
        else
        {
            m_State = m_PrevState;
        }
    }
}
