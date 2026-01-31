using UnityEngine;

public enum CardType
{
    Damage,
    Defense,
    Heal
}

[CreateAssetMenu(fileName = "CardData", menuName = "Combat/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite artwork;
    public int manaCost;

    [Header("Daño")]
    public int damage;

    [Header("Curación")]
    public int healAmount;

    [Header("Defensa")]
    public float defensePercent = 0.4f; // 40% menos daño

    [Header("Tipo de carta")]
    public CardType type;
}


