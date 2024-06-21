using UnityEngine;


public enum ItemType
{
    Usable,
    Equipable
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "AttackBaldHair/Items")]
public class ItemSO : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public float itemPrice;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}
