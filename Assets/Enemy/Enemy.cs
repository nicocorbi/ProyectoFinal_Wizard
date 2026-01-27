using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("IA")]
    public float wanderRadius = 8f;
    public float wanderInterval = 3f;

    private NavMeshAgent agent;
    private float wanderTimer;
    private Vector3 originPos;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;

    public Sprite[] upSprites;     // 4 sprites
    public Sprite[] downSprites;   // 4 sprites
    public Sprite[] rightSprites;  // 4 sprites (se usa flipX para izquierda)

    private float animationTimer = 0f;
    public float animationInterval = 0.2f;
    private int animationFrame = 0;
    private int lastDirection = 0; // 0=down, 1=up, 2=right, 3=left

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originPos = transform.position;
        wanderTimer = wanderInterval;

    }

    void Update()
    {
        

        // --- IA DE MOVIMIENTO ---
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(originPos, wanderRadius, NavMesh.AllAreas);
            agent.SetDestination(newPos);
            wanderTimer = 0f;
        }

        // --- ANIMACIÓN ---
        Vector3 vel = agent.velocity;

        // Si no se mueve → idle
        if (vel.magnitude < 0.1f)
        {
            animationFrame = 0;
            animationTimer = 0f;
        }
        else
        {
            float x = vel.x;
            float z = vel.z;

            // Determinar dirección dominante
            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                lastDirection = x > 0 ? 2 : 3; // derecha o izquierda
            }
            else
            {
                lastDirection = z > 0 ? 1 : 0; // arriba o abajo
            }

            // Avanzar animación
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationInterval)
            {
                animationFrame = (animationFrame + 1) % 4;
                animationTimer = 0f;
            }
        }

        // Aplicar sprite según dirección
        switch (lastDirection)
        {
            case 0: // abajo
                spriteRenderer.sprite = downSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;

            case 1: // arriba
                spriteRenderer.sprite = upSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;

            case 2: // derecha
                spriteRenderer.sprite = rightSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;

            case 3: // izquierda
                spriteRenderer.sprite = rightSprites[animationFrame];
                spriteRenderer.flipX = true;
                break;
        }
    }

    // Genera una posición aleatoria válida en el NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
