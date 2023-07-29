using UnityEngine;
public class SpawnCharacter : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterData characterData;
    private GameObject mainCam;
    private GameObject spotLight2D;
    public GameObject prefabToSpawn;
    private GameObject spawnedObject;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedObject = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);
        spawnedObject.transform.position = gameObject.transform.position;
        mainCam = GameObject.FindWithTag("MainCamera");
        spotLight2D = GameObject.Find("Spot Light 2D");
    }
    private void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        characterData.InvokeCharacterData(100, 100, 10, 0, 10, 20);

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        //CamFollowPlayer();
        SpotLightFollowPlayer();
    }
    private void CamFollowPlayer()
    {
        mainCam.transform.position = spawnedObject.transform.position + new Vector3(0, 8.5f, -40);
    }
    private void SpotLightFollowPlayer()
    {
        spotLight2D.transform.position = spawnedObject.transform.position;
    }
}
