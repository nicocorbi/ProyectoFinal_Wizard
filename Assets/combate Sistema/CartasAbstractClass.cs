using UnityEngine;

public abstract class CartasAbstractClass : MonoBehaviour
{
    [SerializeField] protected string CartaName;
    [SerializeField] protected int ManaCost;
    [SerializeField] protected Sprite CartaImage;

    public string Name => CartaName;
    public int Cost => ManaCost;
    public Sprite Image => CartaImage;

    // IMPORTANTE: ahora sabe si la usa el jugador o el enemigo
    public abstract void EjecutarCarta(CombatController combate, bool usadaPorJugador);
}


