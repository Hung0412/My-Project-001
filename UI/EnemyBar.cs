using UnityEngine;
using UnityEngine.UI;
public class EnemyBar : MonoBehaviour
{
    private EnemyData enemyData;
    public Image enemyHealthBarImage;
    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustEnemyHealthBar();
    }
    private void AdjustEnemyHealthBar()
    {
        enemyHealthBarImage.fillAmount = enemyData.CurrentHealthValue / enemyData.MaxHealthValue;
        enemyHealthBarImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
