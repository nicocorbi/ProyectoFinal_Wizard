using UnityEngine;

public class BattleSceneLoader : MonoBehaviour
{
    public Transform playerBattlePos;
    public Transform enemyBattlePos;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private void Start()
    {
        // Instanciar jugador
        GameObject player = Instantiate(playerPrefab, playerBattlePos.position, playerBattlePos.rotation);

        // Saber qué enemigo te atacó
        string enemyName = PlayerPrefs.GetString("LastEnemy");

        // Instanciar enemigo correcto
        GameObject enemy = Instantiate(enemyPrefab, enemyBattlePos.position, enemyBattlePos.rotation);

        // Iniciar combate
        CombatController combat = FindObjectOfType<CombatController>();
        combat.IniciarCombate(enemy);
    }
}

