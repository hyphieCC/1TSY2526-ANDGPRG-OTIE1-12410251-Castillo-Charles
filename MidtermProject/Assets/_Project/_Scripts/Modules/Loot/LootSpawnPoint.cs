using UnityEngine;

public class LootSpawnPoint : MonoBehaviour
{
    [SerializeField] private LootType lootType;
    public LootType Type => lootType;
}
