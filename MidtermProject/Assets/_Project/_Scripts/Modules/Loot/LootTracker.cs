using UnityEngine;

public class LootTracker : MonoBehaviour
{
    [SerializeField] private int quota = 50;

    public int Total { get; private set; }
    public int Quota => quota;
    public bool QuotaMet => Total >= quota;

    public void AddLoot(int value, string name)
    {
        Total += value;
        Debug.Log($"Stole {name} (+{value}). Total: {Total}/{quota}");
    }
}