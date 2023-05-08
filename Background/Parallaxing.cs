using System.Collections.Generic;
using UnityEngine;
public class Parallaxing : MonoBehaviour
{
    //REFERENCES
    private GameObject[] backgrounds;
    public List<GameObject> backgroundsList = new List<GameObject>();
    public List<float> parallaxScaleList = new List<float>();
    private Transform playerTransform;

    //VARIABLES
    private Vector3 playerPrePos;
    public float[] parallaxScales;

    // Start is called before the first frame update
    void Start()
    {
        backgrounds = GameObject.FindGameObjectsWithTag("Background");
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerPrePos = playerTransform.transform.position;
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgroundsList.Add(backgrounds[i]);
        }
        for (int i = 0; i < backgroundsList.Count; i++)
        {
            parallaxScales[i] = backgroundsList[i].transform.position.z;
            parallaxScaleList.Add(parallaxScales[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ParallaxEffect();
    }
    private void ParallaxEffect()
    {
        for (int i = 0; i < backgroundsList.Count; i++)
        {
            float parallax = (playerTransform.transform.position.x - playerPrePos.x) * parallaxScaleList[i] / 100;
            float backgroundNewPosX = backgroundsList[i].transform.position.x + parallax;
            backgroundsList[i].transform.position = new Vector3(backgroundNewPosX, backgroundsList[i].transform.position.y, backgroundsList[i].transform.position.z);
        }
        playerPrePos = playerTransform.transform.position;
    }
}
