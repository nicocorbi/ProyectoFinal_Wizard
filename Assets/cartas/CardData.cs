using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Combat/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public int damage;
    public Sprite artwork;
}

