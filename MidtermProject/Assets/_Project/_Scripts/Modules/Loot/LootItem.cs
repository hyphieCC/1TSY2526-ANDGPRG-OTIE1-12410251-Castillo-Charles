using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LootItem : MonoBehaviour
{
    [SerializeField] private LootType lootType;
    [SerializeField] private string lootName = "Loot";
    [SerializeField] private int value = 10;

    public LootType Type => lootType;
    public string Name => lootName;
    public int Value => value;

    public bool IsStolen { get; private set; }

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Steal()
    {
        if (IsStolen) return;
        IsStolen = true;
        gameObject.SetActive(false);
    }
}
