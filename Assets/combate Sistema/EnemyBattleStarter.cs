using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBattleStarter : MonoBehaviour
{
    [Header("Detección y combate")]
    public float detectionDistance = 6f;
    public float engageDistance = 1.5f;
    public float maxChaseDistance = 10f;
    public string battleSceneName = "BattleScene";

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Exclamación")]
    public GameObject alertIconPrefab;
    public float alertDuration = 0.6f;

    private EnemyVision vision;
    private NavMeshAgent agent;
    private Transform player;
    private PlayerMovement playerMovement;

    private bool chasing = false;
    private bool alertShown = false;

    private Vector3 originalPosition;

    private void Start()
    {
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        agent.speed = patrolSpeed;
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Si NO está persiguiendo todavía
        if (!chasing)
        {
            if (vision.CanSeePlayer(player))
            {
                chasing = true;
                agent.speed = chaseSpeed;

                if (!alertShown)
                {
                    alertShown = true;
                    StartCoroutine(ShowAlertIcon());
                }
            }
            else
                return;
        }

        // Perseguir al jugador
        agent.SetDestination(player.position);

        // Si el jugador se aleja demasiado → dejar de perseguir
        float chaseDistance = Vector3.Distance(transform.position, player.position);
        if (chaseDistance > maxChaseDistance)
        {
            StopChasing();
            return;
        }

        // Si está lo suficientemente cerca → combate
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= engageDistance && chasing)
        {
            StartBattle();
        }
    }

    private void StopChasing()
    {
        chasing = false;
        alertShown = false; // ← Permite que la exclamación vuelva a aparecer
        agent.speed = patrolSpeed;

        // Volver al punto original
        agent.SetDestination(originalPosition);
    }

    private void StartBattle()
    {
        agent.isStopped = true;

        // Guardar enemigo
        PlayerPrefs.SetString("LastEnemy", gameObject.name);

        // Bloquear movimiento del jugador
        playerMovement.enabled = false;

        // Iniciar transición
        FindObjectOfType<SceneFader>().FadeToScene(battleSceneName);
    }



private IEnumerator ShowAlertIcon()
    {
        // Instanciar el icono encima del enemigo
        GameObject icon = Instantiate(alertIconPrefab, transform);
        icon.transform.localPosition = new Vector3(0, 2f, 0);

        yield return new WaitForSeconds(alertDuration);

        Destroy(icon);
    }
}




