using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Stats")]
    public HealthComponent jugadorHealth;
    public HealthComponent enemigoHealth;
    public int JugadorMana = 5;

    [Header("Mazo del jugador")]
    public DeckManager deck;

    [Header("Mazo del enemigo")]
    public EnemyDeckManager enemyDeck;
    public int EnemyMana = 5;

    [Header("Visual jugador")]
    public Transform cartasSpawnPoint;
    public float escalaBaseCarta = 0.1f;
    public float separacion = 2f;
    public float levantamientoY = 0.5f;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private int currentIndex = 0;
    private bool esperandoSeleccion = false;

    // ---------------------------------------------------------
    // AWAKE: asegura que siempre usamos el jugador correcto
    // ---------------------------------------------------------
    private void Awake()
    {

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugadorHealth = playerObj.GetComponent<HealthComponent>();
        }
        else
        {
            Debug.LogError("No se encontró un objeto con tag Player en la escena de combate.");
        }
    }

    private void Start()
    {
        Debug.Log("JugadorHealth encontrado: " + jugadorHealth.name);
        Debug.Log("Vida inicial del jugador: " + jugadorHealth.currentHealth);

        MostrarMano();
        esperandoSeleccion = true;

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

        Debug.Log("Combate iniciado contra " + enemigo.name);

        MostrarMano();
        esperandoSeleccion = true;
    }

    // ---------------------------------------------------------
    // MANO DEL JUGADOR (VISUAL)
    // ---------------------------------------------------------
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

        var manoJugador = deck.ObtenerMano();
        if (manoJugador.Count == 0 || index < 0 || index >= manoJugador.Count)
        {
            Debug.LogWarning("No hay carta válida seleccionada");
            return;
        }

        var carta = manoJugador[index];

        if (JugadorMana < carta.Cost)
        {
            Debug.Log("No tienes suficiente mana");
            return;
        }

        JugadorMana -= carta.Cost;

        carta.EjecutarCarta(this, true); // jugador usa carta

        deck.UsarCarta(index);

        MostrarMano();

        esperandoSeleccion = false;
        ComprobarEstado();
    }

    // ---------------------------------------------------------
    // CAMBIO DE TURNOS
    // ---------------------------------------------------------
    private void ComprobarEstado()
    {
        if (enemigoHealth.currentHealth <= 0)
        {
            Debug.Log("¡Has ganado!");
            return;
        }

        if (jugadorHealth.currentHealth <= 0)
        {
            Debug.Log("Has muerto");
            return;
        }

        TurnoEnemigo();
    }

    // ---------------------------------------------------------
    // TURNO DEL ENEMIGO (IA)
    // ---------------------------------------------------------
    private void TurnoEnemigo()
    {
        Debug.Log("Turno del enemigo");

        var manoEnemigo = enemyDeck.ObtenerMano();

        if (manoEnemigo == null || manoEnemigo.Count == 0)
        {
            Debug.Log("El enemigo no tiene cartas");
            TurnoJugador();
            return;
        }

        int index = Random.Range(0, manoEnemigo.Count);
        var carta = manoEnemigo[index];

        if (EnemyMana < carta.Cost)
        {
            Debug.Log("El enemigo no tiene mana suficiente, pasa turno");
            TurnoJugador();
            return;
        }

        EnemyMana -= carta.Cost;

        carta.EjecutarCarta(this, false); // enemigo usa carta

        enemyDeck.UsarCarta(index);

        Debug.Log("El enemigo usó la carta: " + carta.name);

        if (jugadorHealth.currentHealth <= 0)
        {
            Debug.Log("Has muerto");
            return;
        }

        TurnoJugador();
    }

    // ---------------------------------------------------------
    // TURNO DEL JUGADOR
    // ---------------------------------------------------------
    private void TurnoJugador()
    {
        esperandoSeleccion = true;
        MostrarCartaActual();
    }

    // ---------------------------------------------------------
    // EVENTOS DE MUERTE
    // ---------------------------------------------------------
    private void EnemigoMuerto()
    {
        var pm = jugadorHealth.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = true;

        Debug.Log("El enemigo ha muerto");
    }

    private void JugadorMuerto()
    {
        var pm = jugadorHealth.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = true;

        Debug.Log("Has muerto");
    }
}


