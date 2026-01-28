using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float visionDistance = 6f;
    public float visionAngle = 45f;
    public LayerMask playerMask;

    public bool CanSeePlayer(Transform player)
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;

        // 1. Ángulo de visión basado en la dirección del sprite
        float angle = Vector3.Angle(GetForwardDirection(), dirToPlayer);
        if (angle > visionAngle)
            return false;

        // 2. Distancia
        if (Vector3.Distance(transform.position, player.position) > visionDistance)
            return false;

        // 3. Raycast para evitar paredes
        if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, visionDistance))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // Dirección real según el sprite del enemigo
    private Vector3 GetForwardDirection()
    {
        Enemy e = GetComponent<Enemy>();

        switch (e.lastDirection)
        {
            case 0: return Vector3.back;   // abajo
            case 1: return Vector3.forward; // arriba
            case 2: return Vector3.right;   // derecha
            case 3: return Vector3.left;    // izquierda
        }

        return Vector3.forward;
    }
}


