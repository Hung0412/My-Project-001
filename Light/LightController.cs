using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    //REFERENCE 
    private Light2D spotLight2D;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        spotLight2D = GameObject.Find("Spot Light 2D").GetComponent<Light2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPlayerSpotLight2D();
    }
    private void AdjustPlayerSpotLight2D()
    {
        if (playerController.isRunning)
        {
            spotLight2D.pointLightInnerRadius = 5;
            spotLight2D.pointLightOuterRadius = 15;
        }
        else if (!playerController.isRunning)
        {
            spotLight2D.pointLightInnerRadius = 3;
            spotLight2D.pointLightOuterRadius = 7;
        }
    }
}
