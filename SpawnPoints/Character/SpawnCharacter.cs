using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterData characterData;
    private GameObject mainCam;
    private GameObject spotLight2D;
    public GameObject prefabToSpawn;
    private GameObject spawnedPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedPlayer = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);
        spawnedPlayer.transform.position = gameObject.transform.position;
        mainCam = GameObject.FindWithTag("MainCamera");
        spotLight2D = GameObject.Find("Spot Light 2D");
    }
    private void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        characterData.InvokeCharacterData(100, 100, 10, 0, 10, 20);

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        mainCam.transform.position = new Vector3(0, 7.2f, -40);
    }
    // Update is called once per frame
    void Update()
    {
        CamFollowPlayer();
        SpotLightFollowPlayer();
    }
    private void CamFollowPlayer()
    {
        float smoothSpeed = 0.5f;
        Vector3 targetCamPos = new Vector3(spawnedPlayer.transform.position.x, 7.2f, -40);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetCamPos, smoothSpeed * Time.deltaTime);
    }
    private void SpotLightFollowPlayer()
    {
        spotLight2D.transform.position = spawnedPlayer.transform.position;
    }
}
