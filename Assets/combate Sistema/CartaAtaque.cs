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
            float mult = TypeChart.GetMultiplier(data.tipo, combate.enemigo.tipoEnemigo);
            int dañoFinal = Mathf.RoundToInt(data.damage * mult);

            combate.enemigoHealth.TakeDamage(dañoFinal);

            Debug.Log($"{data.cardName} ({data.tipo}) hizo {dañoFinal} de daño al ENEMIGO (x{mult})");
        }
        else
        {
            // Multiplicador elemental enemigo → jugador
            float mult = TypeChart.GetMultiplier(data.tipo, combate.tipoJugador);
            int dmg = Mathf.RoundToInt(data.damage * mult);

            // 🔥 Defensa de 3 turnos
            if (combate.defensaActiva && combate.defensaTurnosRestantes > 0)
            {
                dmg = Mathf.RoundToInt(dmg * (1f - combate.defensaPorcentaje));
                combate.defensaTurnosRestantes--;

                Debug.Log($"DEFENSA: quedan {combate.defensaTurnosRestantes} turnos de reducción");

                if (combate.defensaTurnosRestantes <= 0)
                    combate.defensaActiva = false;
            }

            combate.jugadorHealth.TakeDamage(dmg);
            Debug.Log($"{data.cardName} ({data.tipo}) hizo {dmg} de daño al JUGADOR (x{mult})");
        }
    }

    void EjecutarDefensa(CombatController combate)
    {
        combate.defensaActiva = true;
        combate.defensaPorcentaje = data.defensePercent;
        combate.defensaTurnosRestantes = 3; // ← Dura 3 turnos enemigos

        Debug.Log($"DEFENSA ACTIVADA: durante 3 turnos el enemigo hará un {data.defensePercent * 100}% menos de daño");
    }

    void EjecutarCuracion(CombatController combate)
    {
        combate.jugadorHealth.Heal(data.healAmount);
        Debug.Log($"{data.cardName} curó {data.healAmount} de vida al jugador");
    }
}








