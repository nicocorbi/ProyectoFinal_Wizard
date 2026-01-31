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
            // Multiplicador elemental según carta vs enemigo
            float mult = TypeChart.GetMultiplier(data.tipo, combate.enemigo.tipoEnemigo);
            int dañoFinal = Mathf.RoundToInt(data.damage * mult);

            combate.enemigoHealth.TakeDamage(dañoFinal);

            Debug.Log($"{data.cardName} ({data.tipo}) hizo {dañoFinal} de daño al ENEMIGO (x{mult})");
        }
        else
        {
            int dmg = data.damage;

            // Defensa del jugador
            if (combate.defensaActiva)
            {
                dmg = Mathf.RoundToInt(dmg * (1f - combate.defensaPorcentaje));
                combate.defensaActiva = false;
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







