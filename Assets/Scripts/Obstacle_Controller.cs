using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Controller : MonoBehaviour
{
    private Transform m_ObsTransform;
    private bool m_freeze = false;
    public float m_speed;

    // Start is called before the first frame update
    void Start()
    {
        m_ObsTransform = GetComponent<Transform>();
        GameManger_Script.OnPause += OnPause;
        Player_Controller.OnGameOver += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_freeze)
        {
            Vector3 move = new Vector3(m_speed, 0);
            m_ObsTransform.position -= move;
        }
        
    }

    private void OnDestroy()
    {
        GameManger_Script.OnPause -= OnPause;
        Player_Controller.OnGameOver -= OnGameOver;
    }

    private void OnPause(bool pause)
    {
        m_freeze = pause;
    }

    private void OnGameOver()
    {
        m_freeze = true;
    }
}
