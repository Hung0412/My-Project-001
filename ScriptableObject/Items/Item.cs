using UnityEngine;
[CreateAssetMenu]
public class Item : ScriptableObject
{
    private bool stackable;
    public bool Stackable
    {
        get { return stackable; }
        set { stackable = value; }
    }
    public enum ItemType
    {
        COIN,
        HEALTH
    }
    private ItemType itemType;
    public ItemType _ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }

}
