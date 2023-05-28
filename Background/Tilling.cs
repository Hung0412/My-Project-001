using UnityEngine;

public class Tilling : MonoBehaviour
{
    // REFERENCES
    private Camera cam;
    private SpriteRenderer backgroundSRenderer;

    // VARIABLES
    public float offsetX = 2f;
    public bool hasALeftBuddy = false;
    public bool hasARightBuddy = false;

    private float backgroundWidth;
    private float camHorizontalExtend;

    private Parallaxing parallaxing;

    void Start()
    {
        cam = Camera.main;
        backgroundSRenderer = GetComponent<SpriteRenderer>();

        backgroundWidth = backgroundSRenderer.bounds.size.x;

        parallaxing = GameObject.Find("Backgrounds").GetComponent<Parallaxing>();
    }

    void Update()
    {
        if (!hasALeftBuddy || !hasARightBuddy)
        {
            camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
            float edgeVisiblePosRight = backgroundWidth * 0.5f - camHorizontalExtend + backgroundSRenderer.transform.position.x - cam.transform.position.x;
            float edgeVisiblePosLeft = -backgroundWidth * 0.5f + camHorizontalExtend + backgroundSRenderer.transform.position.x - cam.transform.position.x;

            if (edgeVisiblePosRight <= offsetX && !hasARightBuddy)
            {
                MakeNewBackGround(true);
                hasARightBuddy = true;
            }
            else if (edgeVisiblePosLeft >= -offsetX && !hasALeftBuddy)
            {
                MakeNewBackGround(false);
                hasALeftBuddy = true;
            }
        }
    }

    void MakeNewBackGround(bool isRightBuddy)
    {
        float offset = isRightBuddy ? backgroundWidth : -backgroundWidth;
        Vector3 newBackgroundPos = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        GameObject newBackground = Instantiate(gameObject, newBackgroundPos, Quaternion.Euler(0, 180, 0));
        Tilling newTiling = newBackground.GetComponent<Tilling>();

        if (isRightBuddy)
        {
            newTiling.hasALeftBuddy = true;
        }
        else
        {
            newTiling.hasARightBuddy = true;
        }

        newBackground.transform.parent = transform.parent;
        SetUpNewBackgroundWithParallaxing(newBackground);
    }

    void SetUpNewBackgroundWithParallaxing(GameObject newBackground)
    {
        float newBackgroundPosZ = newBackground.transform.position.z;
        newBackgroundPosZ = transform.position.z;
        parallaxing.backgroundsList.Add(newBackground);
        parallaxing.parallaxScaleList.Add(newBackgroundPosZ);
    }
}