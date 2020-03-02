using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn_Controller : MonoBehaviour
{
    public List<GameObject> m_SpawnList;
    public bool m_SpawnerOn;

    public float m_SpawnInterval;
    public float m_SpawnVariance;
    public float m_ObstacleSpeed;

    private float m_SpawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        Player_Controller.OnGameOver += OnGameOver;
        GameManger_Script.OnPause += OnPause;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_SpawnTimer <= 0.0f && m_SpawnerOn)
        {
            int randomSpawn = Random.Range(0, m_SpawnList.Count);
            SpawnObstacle(m_SpawnList[randomSpawn]);
            m_SpawnTimer = m_SpawnInterval + Random.Range(-m_SpawnVariance, m_SpawnVariance);
        }
        else
        {
            m_SpawnTimer -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        Player_Controller.OnGameOver -= OnGameOver;
        GameManger_Script.OnPause -= OnPause;
    }

    public void OnPause(bool pause)
    {
        if (pause)
        {
            m_SpawnerOn = false;
        }
        else
        {
            m_SpawnerOn = true;
        }
    }

    public void OnGameOver()
    {
        m_SpawnerOn = false;
    }

    private void SpawnObstacle(GameObject obstacle)
    {
        GameObject spawned;
        Obstacle_Controller controller;
        Transform spawnPoint = GetComponent<Transform>();
        spawned = Instantiate(obstacle, spawnPoint.position, spawnPoint.rotation);
        controller = spawned.GetComponent<Obstacle_Controller>();
        controller.m_speed = m_ObstacleSpeed;
    }
}
