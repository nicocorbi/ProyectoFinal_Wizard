using UnityEngine;
using UnityEngine.UI;

public class ManaOrbUI : MonoBehaviour
{
    [Header("Referencia al relleno del orbe")]
    public Image fillImage;

    private void OnEnable()
    {
        CombatController.OnManaChanged += UpdateMana;
    }

    private void OnDisable()
    {
        CombatController.OnManaChanged -= UpdateMana;
    }

    private void UpdateMana(int manaActual, int manaMax)
    {
        if (fillImage == null)
        {
            Debug.LogError("ManaOrbUI: No se asignó fillImage.");
            return;
        }

        float amount = Mathf.Clamp01((float)manaActual / manaMax);
        fillImage.fillAmount = amount;
    }
}



