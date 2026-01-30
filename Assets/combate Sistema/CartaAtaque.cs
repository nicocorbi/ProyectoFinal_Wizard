using UnityEngine;

public class CartaAtaque : CartasAbstractClass
{
    public override void EjecutarCarta(CombatController combate, bool usadaPorJugador)
    {
        if (usadaPorJugador)
        {
            combate.enemigoHealth.TakeDamage(Damage);
            Debug.Log($"{Name} hizo {Damage} de daño al ENEMIGO");
        }
        else
        {
            combate.jugadorHealth.TakeDamage(Damage);
            Debug.Log($"{Name} hizo {Damage} de daño al JUGADOR");
        }
    }
}



