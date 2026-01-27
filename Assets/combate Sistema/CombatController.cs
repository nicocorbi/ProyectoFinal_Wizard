using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Stats")]
    public HealthComponent jugadorHealth;
    public HealthComponent enemigoHealth;
    public int JugadorMana = 5;

    [Header("Sistema de mazo")]
    public DeckManager deck;

    [Header("Visual")]
    public Transform cartasSpawnPoint;
    public float escalaBaseCarta = 0.1f;
    public float separacion = 2f;
    public float levantamientoY = 0.5f;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private int currentIndex = 0;
    private bool esperandoSeleccion = false;

    private void Start()
    {
        MostrarMano();
        esperandoSeleccion = true;

        // Eventos de muerte
        enemigoHealth.OnDeath += EnemigoMuerto;
        jugadorHealth.OnDeath += JugadorMuerto;
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
    public void IniciarCombate(GameObject enemigo)
    {
        enemigoHealth = enemigo.GetComponent<HealthComponent>();

        // Aquí puedes desactivar movimiento del jugador si quieres
        // playerMovement.enabled = false;

        Debug.Log("Combate iniciado contra " + enemigo.name);

        MostrarMano();
        esperandoSeleccion = true;
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

            cartaGO.transform.localPosition = new Vector3(i * separacion, 0, 0);
            cartaGO.transform.localRotation = Quaternion.identity;
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

        // Ejecuta la carta (daño, curación, etc.)
        carta.EjecutarCarta(this);

        // Descarta y roba una nueva
        deck.UsarCarta(index);

        // Actualiza la mano visual
        MostrarMano();

        esperandoSeleccion = false;
        ComprobarEstado();
    }

    private void ComprobarEstado()
    {
        if (enemigoHealth.currentHealth <= 0)
        {
            Debug.Log("¡Has ganado!");
            return;
        }

        TurnoEnemigo();
    }

    private void TurnoEnemigo()
    {
        //enemigoHealth.TakeDamage(0); // por si quieres activar eventos

        // Daño al jugador
        //jugadorHealth.TakeDamage(10);

        TurnoJugador();
    }

    private void TurnoJugador()
    {
        esperandoSeleccion = true;
        MostrarCartaActual();
    }

    private void EnemigoMuerto()
    {
        jugadorHealth.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log("El enemigo ha muerto");
    }

    private void JugadorMuerto()
    {
        jugadorHealth.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log("Has muerto");
    }
}

