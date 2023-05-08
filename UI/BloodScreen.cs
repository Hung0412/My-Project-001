using UnityEngine;
public class BloodScreen : MonoBehaviour
{
    //REFERENCES
    private CharacterData characterData;
    public GameObject bloodScreen;
    // Start is called before the first frame update
    void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
    }

    // Update is called once per frame
    void Update()
    {
        EnableBloodScreen();
    }
    private void EnableBloodScreen()
    {
        if (characterData.CurrentHealthValue <= 10f)
        {
            bloodScreen.SetActive(true);
        }
        else bloodScreen.SetActive(false);
    }
}
