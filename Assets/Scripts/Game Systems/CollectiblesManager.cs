using UnityEngine;
using UnityEngine.UI;

public class CollectiblesManager : MonoBehaviour
{
    public int commonCollectibleCount;
    public Text commonCollectibleText;
    public Image commonCollectibleImg;

    public int limitedCollectibleCount;
    public Text limitedCollectibleText;
    public Image limitedCollectibleImg;


    private float timeToAppear = 3f;
    private float timeWhenDisappear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        commonCollectibleText.text = " x " + commonCollectibleCount.ToString();
        limitedCollectibleText.text = " x " + limitedCollectibleCount.ToString() + "/5";

        // Check every frame if the timer has expired and the text should disappear
        if (commonCollectibleText.enabled && (Time.time >= timeWhenDisappear))
        {
            commonCollectibleText.enabled = false;
            commonCollectibleImg.enabled = false;
        }
        // Check every frame if the timer has expired and the text should disappear
        if (limitedCollectibleText.enabled && (Time.time >= timeWhenDisappear))
        {
            limitedCollectibleText.enabled = false;
            limitedCollectibleImg.enabled = false;
        }

    }

    //Call to enable the text, which also sets the timer
    public void EnableUI()
    {
        commonCollectibleText.enabled = true;
        commonCollectibleImg.enabled = true;

        limitedCollectibleText.enabled = true;
        limitedCollectibleImg.enabled = true;

        timeWhenDisappear = Time.time + timeToAppear;
    }
}
