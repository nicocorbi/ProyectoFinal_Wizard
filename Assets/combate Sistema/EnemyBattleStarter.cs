using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBattleStarter : MonoBehaviour
{
    public float detectionDistance = 6f;
    public float engageDistance = 1.5f;
    public string battleSceneName = "BattleScene";

    private EnemyVision vision;
    private NavMeshAgent agent;
    private Transform player;
    private bool chasing = false;

    private void Start()
    {
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!chasing)
        {
            if (vision.CanSeePlayer(player))
            {
                chasing = true;
            }
            else
                return;
        }

        // 1. El enemigo va hacia el jugador
        agent.SetDestination(player.position);

        // 2. Cuando llega cerca → combate
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= engageDistance)
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        agent.isStopped = true;

        // Guardar qué enemigo ha iniciado el combate
        PlayerPrefs.SetString("LastEnemy", gameObject.name);

        // Fade + cargar escena
        SceneManager.LoadScene(battleSceneName);
    }
}

