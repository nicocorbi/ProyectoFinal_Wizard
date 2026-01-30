using UnityEngine;
using System.Collections;

public class BattleSceneLoader : MonoBehaviour
{
    public Transform playerBattlePos;
    public Transform enemyBattlePos;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private IEnumerator Start()
    {
        // Instanciar jugador
        GameObject player = Instantiate(playerPrefab, playerBattlePos.position, playerBattlePos.rotation);

        // Esperar a que Awake() del jugador se ejecute
        yield return null;

        // Asegurar que el jugador tiene HealthComponent inicializado
        HealthComponent hp = player.GetComponent<HealthComponent>();
        while (hp == null || hp.currentHealth <= 0)
        {
            yield return null;
            hp = player.GetComponent<HealthComponent>();
        }

        // Instanciar enemigo
        GameObject enemy = Instantiate(enemyPrefab, enemyBattlePos.position, enemyBattlePos.rotation);

        // Iniciar combate AHORA que el jugador ya está vivo
        CombatController combat = FindObjectOfType<CombatController>();
        combat.IniciarCombate(enemy);
    }
}


