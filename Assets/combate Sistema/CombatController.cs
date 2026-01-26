using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Stats")]
    public int JugadorMana = 5;
    public int EnemigoHP = 200;

    [Header("Cartas")]
    [SerializeField] private List<CartasAbstractClass> cartasDisponibles;

    [Header("Visual")]
    [SerializeField] private Transform cartasSpawnPoint;
    [SerializeField] private float separacion = 2f;

    // 🔥 Escala base de las cartas (AJUSTADA A 0.1)
    [SerializeField] private float escalaBaseCarta = 0.1f;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private int currentIndex = 0;
    private bool esperandoSeleccion = false;

    private void Start()
    {
        IniciarCombate();
    }

    public void IniciarCombate()
    {
        Debug.Log("Combate iniciado");

        InstanciarCartasVisuales();
        esperandoSeleccion = true;
        MostrarCartaActual();
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SeleccionarCarta(currentIndex);
        }
    }


    private void InstanciarCartasVisuales()
    {
        foreach (var c in cartasInstanciadas)
            Destroy(c);

        cartasInstanciadas.Clear();

        for (int i = 0; i < cartasDisponibles.Count; i++)
        {
            var carta = cartasDisponibles[i];

            // 🔥 Instanciar como HIJO del spawn point
            GameObject cartaGO = Instantiate(carta.gameObject, cartasSpawnPoint);

            // 🔥 Posición relativa al spawn point
            cartaGO.transform.localPosition = new Vector3(i * separacion, 0, 0);

            // 🔥 Escala base
            cartaGO.transform.localScale = Vector3.one * escalaBaseCarta;

            // 🔥 Mirar hacia la cámara
            cartaGO.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            cartasInstanciadas.Add(cartaGO);
        }
    }


    private void MostrarCartaActual()
    {
        for (int i = 0; i < cartasInstanciadas.Count; i++)
        {
            float escala = (i == currentIndex)
                ? escalaBaseCarta * 1.2f   // seleccionada
                : escalaBaseCarta;         // normal

            cartasInstanciadas[i].transform.localScale = Vector3.one * escala;
        }

        var carta = cartasDisponibles[currentIndex];
        Debug.Log($"Seleccionada: {carta.Name} | Coste: {carta.Cost}");
    }

    public void SeleccionarCarta(int index)
    {
        if (!esperandoSeleccion) return;

        var carta = cartasDisponibles[index];

        if (JugadorMana < carta.Cost)
        {
            Debug.Log("No tienes suficiente mana");
            return;
        }

        JugadorMana -= carta.Cost;
        carta.EjecutarCarta(this);

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
        Debug.Log("Turno del enemigo...");
        EnemigoHP -= 10;
        Debug.Log("El enemigo te ataca");

        TurnoJugador();
    }

    private void TurnoJugador()
    {
        Debug.Log("Tu turno");
        esperandoSeleccion = true;
        MostrarCartaActual();
    }
}
