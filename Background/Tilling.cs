using UnityEngine;

public class Tilling : MonoBehaviour
{
    //REFERENCES
    private Camera cam;
    private SpriteRenderer backgroundSRenderer;
    //VARIABLES
    public float offsetX = 2f;
    public bool hasALeftBuddy = false;
    public bool hasARightBuddy = false;

    private float backgroundWidth;
    private float camHorizontalExtend;

    private Parallaxing parallaxing;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        backgroundSRenderer = GetComponent<SpriteRenderer>();

        backgroundWidth = backgroundSRenderer.bounds.size.x;

        parallaxing = GameObject.Find("Backgrounds").GetComponent<Parallaxing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
            float edgeVisiblePosRight = backgroundWidth / 2 - (cam.transform.position.x - backgroundSRenderer.transform.position.x) - camHorizontalExtend;
            float edgeVisiblePosLeft = -backgroundWidth / 2 - (cam.transform.position.x - backgroundSRenderer.transform.position.x) + camHorizontalExtend;
            if (edgeVisiblePosRight <= offsetX && hasARightBuddy == false)
            {
                MakeNewBackGround();
                hasARightBuddy = true;
            }
            else if (edgeVisiblePosLeft >= -offsetX && hasALeftBuddy == false)
            {
                MakeNewBackGround();
                hasALeftBuddy = true;
            }
        }
    }
    void MakeNewBackGround()
    {
        if (gameObject.transform.position.x >= 0)
        {
            Vector2 newBackgroundPos = new Vector2(gameObject.transform.position.x + backgroundWidth, gameObject.transform.position.y);
            GameObject newBackground = Instantiate(gameObject, newBackgroundPos, Quaternion.Euler(0, 180, 0));
            newBackground.GetComponent<Tilling>().hasALeftBuddy = true;
            newBackground.transform.parent = gameObject.transform.parent;
            SetUpNewBackgrWithParallaxing(newBackground);
        }
        else if (gameObject.transform.position.x < 0)
        {
            Vector2 newBackgroundPos = new Vector2(gameObject.transform.position.x - backgroundWidth, gameObject.transform.position.y);
            GameObject newBackground = Instantiate(gameObject, newBackgroundPos, Quaternion.Euler(0, 180, 0));
            newBackground.GetComponent<Tilling>().hasARightBuddy = true;
            newBackground.transform.parent = gameObject.transform.parent;
            SetUpNewBackgrWithParallaxing(newBackground);
        }
    }
    private void SetUpNewBackgrWithParallaxing(GameObject newBackground)
    {
        float newBackgroundPosZ = newBackground.transform.position.z;
        newBackgroundPosZ = gameObject.transform.position.z;
        parallaxing.backgroundsList.Add(newBackground);
        parallaxing.parallaxScaleList.Add(newBackgroundPosZ);
    }
}
