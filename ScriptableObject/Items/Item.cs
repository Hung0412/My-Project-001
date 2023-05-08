using UnityEngine;
[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public int quantity;
    public bool stackable;
    public enum ItemType
    {
        COIN,
        HEALTH
    }
    public ItemType itemType;

}
