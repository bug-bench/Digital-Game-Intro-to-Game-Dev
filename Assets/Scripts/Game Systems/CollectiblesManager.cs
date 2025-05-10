using UnityEngine;
using UnityEngine.UI;

public class CollectiblesManager : MonoBehaviour
{
    public int commonCollectibleCount;
    public Text commonCollectibleText;

    public int limitedCollectibleCount;
    public Text limitedCollectibleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        commonCollectibleText.text = " x " + commonCollectibleCount.ToString();
        limitedCollectibleText.text = " x " + limitedCollectibleCount.ToString() + "/5";
    }
}
