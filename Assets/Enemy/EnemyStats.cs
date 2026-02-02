using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Combat/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int maxMana;
    public Sprite enemySprite;

    [Header("Tipo elemental del enemigo")]
    public ElementType tipoEnemigo;   // ← AHORA EL TIPO ESTÁ AQUÍ
}


