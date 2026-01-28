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

    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] rightSprites;

    public int lastDirection = 0;

    private float animationTimer = 0f;
    public float animationInterval = 0.2f;
    private int animationFrame = 0;

    private bool inBattle = false; // 🔥 IMPORTANTE

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originPos = transform.position;
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        if (inBattle)
            return; // 🔥 NO IA NI ANIMACIÓN DURANTE COMBATE

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

        if (vel.magnitude < 0.1f)
        {
            animationFrame = 0;
            animationTimer = 0f;
        }
        else
        {
            float x = vel.x;
            float z = vel.z;

            if (Mathf.Abs(x) > Mathf.Abs(z))
                lastDirection = x > 0 ? 2 : 3;
            else
                lastDirection = z > 0 ? 1 : 0;

            animationTimer += Time.deltaTime;
            if (animationTimer >= animationInterval)
            {
                animationFrame = (animationFrame + 1) % 4;
                animationTimer = 0f;
            }
        }

        switch (lastDirection)
        {
            case 0:
                spriteRenderer.sprite = downSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;
            case 1:
                spriteRenderer.sprite = upSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;
            case 2:
                spriteRenderer.sprite = rightSprites[animationFrame];
                spriteRenderer.flipX = false;
                break;
            case 3:
                spriteRenderer.sprite = rightSprites[animationFrame];
                spriteRenderer.flipX = true;
                break;
        }
    }

    public void LookAtPlayer(Transform player)
    {
        Vector3 dir = player.position - transform.position;

        float x = dir.x;
        float z = dir.z;

        if (Mathf.Abs(x) > Mathf.Abs(z))
            lastDirection = x > 0 ? 2 : 3;
        else
            lastDirection = z > 0 ? 1 : 0;

        animationFrame = 0;

        switch (lastDirection)
        {
            case 0:
                spriteRenderer.sprite = downSprites[0];
                spriteRenderer.flipX = false;
                break;
            case 1:
                spriteRenderer.sprite = upSprites[0];
                spriteRenderer.flipX = false;
                break;
            case 2:
                spriteRenderer.sprite = rightSprites[0];
                spriteRenderer.flipX = false;
                break;
            case 3:
                spriteRenderer.sprite = rightSprites[0];
                spriteRenderer.flipX = true;
                break;
        }
    }

    public void EnterBattle(Transform player)
    {
        inBattle = true;

        if (agent != null)
            agent.enabled = false;

        LookAtPlayer(player);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}


