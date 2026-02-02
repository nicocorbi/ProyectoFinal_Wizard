using UnityEngine;

[CreateAssetMenu(fileName = "ElementTypeData", menuName = "Combat/Element Type Data")]
public class ElementTypeData : ScriptableObject
{
    public ElementType tipo;
    public Sprite icono;
    public Color colorUI = Color.white;
}

