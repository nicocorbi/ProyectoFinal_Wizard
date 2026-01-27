using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Stats")]
    public int JugadorMana = 5;
    public int EnemigoHP = 200;

    [Header("Sistema de mazo")]
    public DeckManager deck;

    [Header("Visual")]
    public Transform cartasSpawnPoint;
    public float escalaBaseCarta = 0.1f;
    public float separacion = 2f;   // 🔥 distancia entre cartas en fila
    public float levantamientoY = 0.5f;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private int currentIndex = 0;
    private bool esperandoSeleccion = false;

    private void Start()
    {
        MostrarMano();
        esperandoSeleccion = true;
    }

    private void Update()
    {
        if (!esperandoSeleccion) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % cartasInstanciadas.Count;
            MostrarCartaActual();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = cartasInstanciadas.Count - 1;
            MostrarCartaActual();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SeleccionarCarta(currentIndex);
        }

    }

    private void MostrarMano()
    {
        foreach (var c in cartasInstanciadas)
            Destroy(c);

        cartasInstanciadas.Clear();

        var mano = deck.ObtenerMano();

        for (int i = 0; i < mano.Count; i++)
        {
            var carta = mano[i];
            GameObject cartaGO = Instantiate(carta.gameObject, cartasSpawnPoint);

            // 🔥 POSICIÓN EN FILA (sin abanico)
            cartaGO.transform.localPosition = new Vector3(i * separacion, 0, 0);

            // 🔥 SIN ROTACIÓN
            cartaGO.transform.localRotation = Quaternion.identity;

            // 🔥 ESCALA BASE
            cartaGO.transform.localScale = Vector3.one * escalaBaseCarta;

            cartasInstanciadas.Add(cartaGO);
        }

        MostrarCartaActual();
    }

    private void MostrarCartaActual()
    {
        for (int i = 0; i < cartasInstanciadas.Count; i++)
        {
            var cartaGO = cartasInstanciadas[i];

            if (i == currentIndex)
            {
                // 🔥 Levantar ligeramente la carta seleccionada
                cartaGO.transform.localPosition =
                    new Vector3(i * separacion, levantamientoY, 0);

                cartaGO.transform.localScale =
                    Vector3.one * escalaBaseCarta * 1.2f;
            }
            else
            {
                cartaGO.transform.localPosition =
                    new Vector3(i * separacion, 0, 0);

                cartaGO.transform.localScale =
                    Vector3.one * escalaBaseCarta;
            }
        }
    }

    private void SeleccionarCarta(int index)
    {
        if (!esperandoSeleccion) return;

        var carta = deck.ObtenerMano()[index];

        if (JugadorMana < carta.Cost)
        {
            Debug.Log("No tienes suficiente mana");
            return;
        }

        JugadorMana -= carta.Cost;
        carta.EjecutarCarta(this);

        deck.UsarCarta(index); // 🔥 descarta y roba una nueva

        MostrarMano(); // 🔥 actualiza la fila

        esperandoSeleccion = false;
        ComprobarEstado();
    }

    private void ComprobarEstado()
    {
        if (EnemigoHP <= 0)
        {
            Debug.Log("¡Has ganado!");
            return;
        }

        TurnoEnemigo();
    }

    private void TurnoEnemigo()
    {
        EnemigoHP -= 10;
        TurnoJugador();
    }

    private void TurnoJugador()
    {
        esperandoSeleccion = true;
        MostrarCartaActual();
    }
}
