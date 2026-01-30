using UnityEngine;
using System.Collections.Generic;

public class EnemyDeckManager : MonoBehaviour
{
    [Header("Cartas disponibles en el mazo del enemigo")]
    public List<CartasAbstractClass> cartasEnMazo = new List<CartasAbstractClass>();

    [Header("Tamaño de la mano del enemigo")]
    public int manoSize = 3;

    private List<CartasAbstractClass> manoActual = new List<CartasAbstractClass>();

    private void Start()
    {
        RobarManoInicial();
    }

    private void RobarManoInicial()
    {
        manoActual.Clear();

        for (int i = 0; i < manoSize; i++)
        {
            RobarCarta();
        }
    }

    private void RobarCarta()
    {
        if (cartasEnMazo.Count == 0)
        {
            Debug.LogWarning("El mazo del enemigo está vacío");
            return;
        }

        int index = Random.Range(0, cartasEnMazo.Count);
        manoActual.Add(cartasEnMazo[index]);
    }

    public List<CartasAbstractClass> ObtenerMano()
    {
        return manoActual;
    }

    public void UsarCarta(int index)
    {
        manoActual.RemoveAt(index);
        RobarCarta();
    }
}

