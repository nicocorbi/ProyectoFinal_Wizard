using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Combat/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int maxMana;
    public Sprite enemySprite;
}

