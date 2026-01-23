using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent onRestart = new UnityEvent();
    public static UnityEvent onPlayerDeath = new UnityEvent();
    public static UnityEvent onEnemyDeath = new UnityEvent();
}

