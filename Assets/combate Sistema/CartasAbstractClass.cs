using UnityEngine;

public abstract class CartasAbstractClass : MonoBehaviour
{
    public CardData data;

    public string Name => data.cardName;
    public int Cost => data.manaCost;
    public int Damage => data.damage;
    public Sprite Image => data.artwork;

    public abstract void EjecutarCarta(CombatController combate, bool usadaPorJugador);
}



