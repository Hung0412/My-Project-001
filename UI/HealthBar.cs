using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    //REFERENCES
    private CharacterData characterData;
    private Image healthBackground;
    public Image healingChargeBackground;
    // Start is called before the first frame update
    void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        healthBackground = GameObject.Find("HealthBar_Fill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float lerpSpeed = 5;
        healthBackground.fillAmount = Mathf.Lerp(healthBackground.fillAmount, characterData.CurrentHealthValue / characterData.MaxHealthValue, Time.deltaTime * lerpSpeed);
        healingChargeBackground.fillAmount = Mathf.Lerp(healingChargeBackground.fillAmount, characterData.CurrentHealingChargeValue / characterData.MaxHealingChargeValue, Time.deltaTime * lerpSpeed);
    }
}
