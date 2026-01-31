using UnityEngine;

public class CartaAtaque : MonoBehaviour
{
    public CardData data;

    public void EjecutarCarta(CombatController combate, bool esJugador)
    {
        switch (data.type)
        {
            case CardType.Damage:
                EjecutarDaño(combate, esJugador);
                break;

            case CardType.Defense:
                EjecutarDefensa(combate);
                break;

            case CardType.Heal:
                EjecutarCuracion(combate);
                break;
        }
    }

    void EjecutarDaño(CombatController combate, bool esJugador)
    {
        if (esJugador)
        {
            combate.enemigoHealth.TakeDamage(data.damage);
            Debug.Log($"{data.cardName} hizo {data.damage} de daño al ENEMIGO");
        }
        else
        {
            int dmg = data.damage;

            // Si el jugador tiene defensa activa
            if (combate.defensaActiva)
            {
                dmg = Mathf.RoundToInt(dmg * (1f - combate.defensaPorcentaje));
                combate.defensaActiva = false; // Se consume
            }

            combate.jugadorHealth.TakeDamage(dmg);
            Debug.Log($"{data.cardName} hizo {dmg} de daño al JUGADOR");
        }
    }

    void EjecutarDefensa(CombatController combate)
    {
        combate.defensaActiva = true;
        combate.defensaPorcentaje = data.defensePercent;

        Debug.Log($"DEFENSA ACTIVADA: el próximo ataque enemigo hará un {data.defensePercent * 100}% menos de daño");
    }

    void EjecutarCuracion(CombatController combate)
    {
        combate.jugadorHealth.Heal(data.healAmount);
        Debug.Log($"{data.cardName} curó {data.healAmount} de vida al jugador");
    }
}




