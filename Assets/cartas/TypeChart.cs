using System.Collections.Generic;

public static class TypeChart
{
    private static readonly Dictionary<ElementType, Dictionary<ElementType, float>> chart =
        new Dictionary<ElementType, Dictionary<ElementType, float>>
    {
        {
            ElementType.Fuego, new Dictionary<ElementType, float>
            {
                { ElementType.Hielo, 2f },
                { ElementType.Agua, 0.5f },
                { ElementType.Fuego, 1f },
                { ElementType.Rayo, 1f },
                { ElementType.Vida, 1f },
                { ElementType.Muerte, 1f }
            }
        },

        {
            ElementType.Hielo, new Dictionary<ElementType, float>
            {
                { ElementType.Agua, 2f },
                { ElementType.Fuego, 0.5f },
                { ElementType.Hielo, 1f },
                { ElementType.Rayo, 1f },
                { ElementType.Vida, 1f },
                { ElementType.Muerte, 1f }
            }
        },

        {
            ElementType.Rayo, new Dictionary<ElementType, float>
            {
                { ElementType.Agua, 2f },
                { ElementType.Hielo, 0.5f },
                { ElementType.Fuego, 1f },
                { ElementType.Rayo, 1f },
                { ElementType.Vida, 1f },
                { ElementType.Muerte, 1f }
            }
        },

        {
            ElementType.Agua, new Dictionary<ElementType, float>
            {
                { ElementType.Fuego, 2f },
                { ElementType.Hielo, 0.5f },
                { ElementType.Rayo, 0.5f },
                { ElementType.Agua, 1f },
                { ElementType.Vida, 1f },
                { ElementType.Muerte, 1f }
            }
        }
    };

    public static float GetMultiplier(ElementType attacker, ElementType defender)
    {
        if (chart.ContainsKey(attacker) && chart[attacker].ContainsKey(defender))
            return chart[attacker][defender];

        return 1f;
    }
}


