using UnityEngine;

public class ResetCharacterData : MonoBehaviour
{
    //REFERENCES
    private CharacterData characterData;

    //VARIABLES

    // Start is called before the first frame update
    void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;

        characterData.CurrentHealthValue = characterData.MaxHealthValue;
        characterData.CurrentHealingChargeValue = 0;
    }
}
