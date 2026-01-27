using UnityEngine;
using UnityEngine.AI;

public class BattleTrigger : MonoBehaviour
{
    public float detectionRadius = 3f;
    public float separationDistance = 2f; // distancia entre player y enemigo en combate

    private bool battleStarted = false;

    private void Update()
    {
        if (battleStarted) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                StartBattle(hit.transform);
                break;
            }
        }
    }

    private void StartBattle(Transform player)
    {
        Enemy enemy = GetComponent<Enemy>();
        enemy.LookAtPlayer(player);

        battleStarted = true;

        // Desactivar movimiento del enemigo
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // Calcular dirección Player → Enemigo
        Vector3 direction = (transform.position - player.position).normalized;

        // Calcular posiciones automáticas
        Vector3 enemyCombatPos = player.position + direction * separationDistance;
        Vector3 playerCombatPos = enemyCombatPos - direction * separationDistance;

        // Colocar posiciones
        player.position = playerCombatPos;
        transform.position = enemyCombatPos;

        // Orientar cara a cara
        player.LookAt(transform);
        transform.LookAt(player);

        // Desactivar movimiento del jugador
        player.GetComponent<PlayerMovement>().enabled = false;

        // Activar combate
        CombatController combat = FindObjectOfType<CombatController>();
        combat.IniciarCombate(this.gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


