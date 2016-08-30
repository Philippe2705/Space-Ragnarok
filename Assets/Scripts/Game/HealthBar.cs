using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    Image background;
    RectTransform rect;

    void Start()
    {
        background = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }


    public void UpdateHealth(float vie)
    {
        background.color = Color.Lerp(Color.red, Color.blue, vie / 100f);
        rect.sizeDelta = vie * 2 * Vector2.up + rect.sizeDelta.x * Vector2.right;
    }
}
