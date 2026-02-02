using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Stats")]
    public HealthComponent jugadorHealth;
    public HealthComponent enemigoHealth;
    public Enemy enemigo;
    public int JugadorMana = 5;
    public int EnemyMana = 5;

    [Header("Tipo elemental del jugador")]
    public ElementType tipoJugador = ElementType.Fuego;

    [Header("Defensa")]
    public bool defensaActiva = false;
    public float defensaPorcentaje = 0f;
    public int defensaTurnosRestantes = 0;

    [Header("Mazo del jugador")]
    public DeckManager deck;

    [Header("Mazo del enemigo")]
    public EnemyDeckManager enemyDeck;

    [Header("Visual jugador")]
    public Transform cartasSpawnPoint;
    public GameObject cartaVisualPrefab;
    public float separacion = 2f;
    public float levantamientoY = 0.5f;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private int currentIndex = 0;
    private bool esperandoSeleccion = false;

    [Header("UI")]
    public ManaOrbUI manaOrbUI;

    private void Start()
    {
        StartCoroutine(EsperarPlayer());

        // Inicializar el orbe con el mana REAL del jugador
        if (manaOrbUI != null)
            manaOrbUI.Initialize(JugadorMana);
    }

    private IEnumerator EsperarPlayer()
    {
        GameObject playerObj = null;

        while (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        jugadorHealth = playerObj.GetComponent<HealthComponent>();
        jugadorHealth.OnDeath += JugadorMuerto;

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

    public void IniciarCombate(GameObject enemigoGO)
    {
        enemigo = enemigoGO.GetComponent<Enemy>();
        enemigoHealth = enemigoGO.GetComponent<HealthComponent>();
        enemigoHealth.OnDeath += EnemigoMuerto;

        MostrarMano();
        esperandoSeleccion = true;

        // Inicializar el orbe al entrar en combate
        if (manaOrbUI != null)
            manaOrbUI.Initialize(JugadorMana);
    }

    private void MostrarMano()
    {
        foreach (var c in cartasInstanciadas)
            Destroy(c);

        cartasInstanciadas.Clear();

        var mano = deck.ObtenerMano();

        for (int i = 0; i < mano.Count; i++)
        {
            CardData data = mano[i];

            GameObject cartaGO = Instantiate(cartaVisualPrefab, cartasSpawnPoint);
            cartaGO.transform.localPosition = new Vector3(i * separacion, 0, 0);

            CartaVisual visual = cartaGO.GetComponent<CartaVisual>();
            visual.Configurar(data);

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
            }
            else
            {
                cartaGO.transform.localPosition =
                    new Vector3(i * separacion, 0, 0);
            }
        }
    }

    private void SeleccionarCarta(int index)
    {
        if (!esperandoSeleccion) return;

        var manoJugador = deck.ObtenerMano();
        if (manoJugador.Count == 0 || index < 0 || index >= manoJugador.Count)
            return;

        CardData carta = manoJugador[index];

        if (JugadorMana < carta.manaCost)
        {
            Debug.Log("No tienes suficiente mana");
            return;
        }

        // Gastar maná
        JugadorMana -= carta.manaCost;

        // Actualizar orbe
        if (manaOrbUI != null)
            manaOrbUI.SetMana(JugadorMana);

        CartaVisual visual = cartasInstanciadas[index].GetComponent<CartaVisual>();
        visual.cartaLogic.EjecutarCarta(this, true);

        tipoJugador = carta.tipo;
        Debug.Log("El jugador ahora es tipo: " + tipoJugador);

        deck.UsarCarta(index);

        MostrarMano();

        esperandoSeleccion = false;
        ComprobarEstado();
    }

    private void ComprobarEstado()
    {
        if (enemigoHealth != null && enemigoHealth.currentHealth <= 0)
        {
            Debug.Log("¡Has ganado!");
            return;
        }

        if (jugadorHealth != null && jugadorHealth.currentHealth <= 0)
        {
            Debug.Log("Has muerto");
            return;
        }

        TurnoEnemigo();
    }

    private void TurnoEnemigo()
    {
        var manoEnemigo = enemyDeck.ObtenerMano();

        if (manoEnemigo == null || manoEnemigo.Count == 0)
        {
            TurnoJugador();
            return;
        }

        int index = Random.Range(0, manoEnemigo.Count);
        CardData carta = manoEnemigo[index];

        if (EnemyMana < carta.manaCost)
        {
            TurnoJugador();
            return;
        }

        EnemyMana -= carta.manaCost;

        CartaAtaque temp = new GameObject("TempCard").AddComponent<CartaAtaque>();
        temp.data = carta;
        temp.EjecutarCarta(this, false);
        Destroy(temp.gameObject);

        enemyDeck.UsarCarta(index);

        if (jugadorHealth.currentHealth <= 0)
            return;

        TurnoJugador();
    }

    private void TurnoJugador()
    {
        esperandoSeleccion = true;
        MostrarCartaActual();
    }

    private void EnemigoMuerto()
    {
        Debug.Log("El enemigo ha muerto");
    }

    private void JugadorMuerto()
    {
        Debug.Log("Has muerto");
    }
}


