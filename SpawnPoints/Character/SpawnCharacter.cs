using UnityEngine;
using Cinemachine;
public class SpawnCharacter : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterData characterData;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject spotLight2D;
    public GameObject prefabToSpawn;
    private GameObject spawnedObject;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedObject = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);
        spawnedObject.transform.position = gameObject.transform.position;
        virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        spotLight2D = GameObject.Find("Spot Light 2D");
    }
    private void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        characterData.InvokeCharacterData(100, 100, 10, 0, 10, 20);

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        CamFollowPlayer();
    }
    // Update is called once per frame
    void Update()
    {
        SpotLightFollowPlayer();
    }
    private void CamFollowPlayer()
    {
        virtualCamera.Follow = spawnedObject.transform;
        //virtualCamera.LookAt = spawnedObject.transform;
    }
    private void SpotLightFollowPlayer()
    {
        spotLight2D.transform.position = spawnedObject.transform.position;
    }
}
