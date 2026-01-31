using UnityEngine;

public class CartaVisual : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CartaAtaque cartaLogic;

    public void Configurar(CardData data)
    {
        cartaLogic.data = data;
        spriteRenderer.sprite = data.artwork;

        AjustarEscalaCarta(spriteRenderer, 1.5f); // tamaño uniforme
    }

    private void AjustarEscalaCarta(SpriteRenderer sr, float tamañoDeseado)
    {
        if (sr.sprite == null) return;

        // Tamaño real del sprite en unidades
        var bounds = sr.sprite.bounds;
        float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y);

        // Factor de escala para que todas midan lo mismo
        float factor = tamañoDeseado / maxDimension;

        sr.transform.localScale = Vector3.one * factor;
    }
}




