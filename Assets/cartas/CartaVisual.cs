using UnityEngine;
using UnityEngine.UI;

public class CartaVisual : MonoBehaviour
{
    public CartasAbstractClass cartaLogic;   // Lógica de la carta (Ataque, Curación, etc.)
                  // Imagen completa de la carta

    public void Configurar(CardData data)
    {
        // Asignar los datos del ScriptableObject a la lógica
        cartaLogic.data = data;

       
    }
}


