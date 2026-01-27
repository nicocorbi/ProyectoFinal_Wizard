using UnityEngine;

public class CartaAtaque : CartasAbstractClass
{
    [SerializeField] private int damage = 50;

    public override void EjecutarCarta(CombatController combate)
    {
        combate.enemigoHealth.TakeDamage(damage);
        Debug.Log($"{Name} hizo {damage} de daño");
    }
}
