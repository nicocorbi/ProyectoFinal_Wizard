using UnityEngine;
using UnityEngine.UI;

public class ManaOrbUI : MonoBehaviour
{
    [Header("Referencia al relleno del orbe")]
    public Image fillImage;

    private int maxMana;

    // Llamado desde CombatController al iniciar el combate
    public void Initialize(int maxManaValue)
    {
        maxMana = maxManaValue;
        SetMana(maxManaValue);
    }

    // Actualiza el fill del orbe
    public void SetMana(int currentMana)
    {
        if (fillImage == null)
        {
            Debug.LogError("ManaOrbUI: No se asignó fillImage.");
            return;
        }

        if (maxMana <= 0)
        {
            Debug.LogError("ManaOrbUI: maxMana no está inicializado.");
            return;
        }

        float amount = Mathf.Clamp01((float)currentMana / maxMana);
        fillImage.fillAmount = amount;
    }
}



