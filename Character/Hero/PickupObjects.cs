using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    private CharacterData characterData;
    // Start is called before the first frame update
    void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<ItemReference>().item;
            if (hitObject != null)
            {
                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        break;
                    case Item.ItemType.HEALTH:
                        characterData.HealthValue = 10;
                        AdjustHealthValue(characterData.HealthValue);
                        Destroy(collision.gameObject);
                        break;
                }
            }
        }
    }
    public void AdjustHealthValue(float amount)
    {
        if (characterData.CurrentHealthValue < characterData.MaxHealthValue)
        {
            characterData.CurrentHealthValue += amount;
        }
    }
}
