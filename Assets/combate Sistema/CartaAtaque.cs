using UnityEngine;

public class CartaAtaque : CartasAbstractClass
{
    [SerializeField] private int damage = 50;

    public override void EjecutarCarta(CombatController combate, bool usadaPorJugador)
    {
        if (usadaPorJugador)
        {
            combate.enemigoHealth.TakeDamage(damage);
            Debug.Log($"{Name} hizo {damage} de daño al ENEMIGO");
        }
        else
        {
            combate.jugadorHealth.TakeDamage(damage);
            Debug.Log($"{Name} hizo {damage} de daño al JUGADOR");
        }
    }
}


