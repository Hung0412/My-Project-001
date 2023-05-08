using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCombatController playerCombatController;

    private Image playerEachRunBarImage;
    private Image playerEachRunBarMask;
    private TextMeshProUGUI playerEachRunBarText;
    private Image playerNextRunBarImage;
    private Image playerNextRunBarMask;
    private Image playerPunchBarImage;
    private float reverseRunningTimer;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCombatController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatController>();

        playerEachRunBarImage = GameObject.Find("EachRunBar_Image").GetComponent<Image>();
        playerEachRunBarMask = GameObject.Find("EachRunBar_Mask").GetComponent<Image>();
        playerEachRunBarText = GameObject.Find("EachRunBar_Text (TMP)").GetComponent<TextMeshProUGUI>();
        playerNextRunBarImage = GameObject.Find("NextRunBar_Image").GetComponent<Image>();
        playerNextRunBarMask = GameObject.Find("NextRunBar_Mask").GetComponent<Image>();
        playerPunchBarImage = GameObject.Find("PunchBar_Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPlayerRunBar();
    }
    private void AdjustPlayerRunBar()
    {
        SetActivePlayerBar();
        RotationWithPlayer();
        PlayerBarFillAmount();
        PlayerBarText();
    }
    private void SetActivePlayerBar()
    {
        if (playerController.isRunning == true)
        {
            playerEachRunBarImage.gameObject.SetActive(true);
            playerEachRunBarMask.gameObject.SetActive(true);
            playerEachRunBarText.gameObject.SetActive(true);
            playerNextRunBarImage.gameObject.SetActive(false);
            playerNextRunBarMask.gameObject.SetActive(false);
        }
        else if (playerController.isRunning == false)
        {
            playerEachRunBarImage.gameObject.SetActive(false);
            playerEachRunBarMask.gameObject.SetActive(false);
            playerEachRunBarText.gameObject.SetActive(false);
            playerNextRunBarImage.gameObject.SetActive(true);
            playerNextRunBarMask.gameObject.SetActive(true);
        }
        if (playerNextRunBarMask.fillAmount == 1)
        {
            playerNextRunBarImage.gameObject.SetActive(false);
            playerNextRunBarMask.gameObject.SetActive(false);
        }
        if (playerCombatController.hasCharged == true)
        {
            playerPunchBarImage.gameObject.SetActive(true);
        }
        if (playerCombatController.hasCharged == false)
        {
            playerPunchBarImage.gameObject.SetActive(false);
        }
    }
    private void RotationWithPlayer()
    {
        playerEachRunBarImage.transform.rotation = transform.rotation;
        playerEachRunBarMask.transform.rotation = transform.rotation;
        playerEachRunBarText.transform.rotation = transform.rotation;
        playerNextRunBarImage.transform.rotation = transform.rotation;
        playerNextRunBarMask.transform.rotation = transform.rotation;
    }
    private void PlayerBarFillAmount()
    {
        playerEachRunBarMask.fillAmount = playerController.runningTimer / playerController.runningResetTime;
        playerNextRunBarMask.fillAmount = playerController.nextRunningTimer / playerController.nextRunningResetTime;
        playerPunchBarImage.fillAmount = playerCombatController.punchTimer / playerCombatController.punchResetTime;
    }
    private void PlayerBarText()
    {
        if (playerController.runningTimer < (playerController.runningResetTime / 2))
        {
            reverseRunningTimer = (playerController.runningResetTime / 2) + (playerController.runningResetTime / 2 - playerController.runningTimer);
        }
        else if (playerController.runningTimer > (playerController.runningResetTime / 2))
        {
            reverseRunningTimer = (playerController.runningResetTime / 2) - (playerController.runningTimer - playerController.runningResetTime / 2);
        }
        else if (playerController.runningTimer == (playerController.runningResetTime / 2))
        {
            reverseRunningTimer = playerController.runningResetTime / 2;
        }
        playerEachRunBarText.text = ((int)(reverseRunningTimer + 0.5f)).ToString();
    }
}
