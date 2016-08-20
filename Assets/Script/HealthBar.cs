using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    Image background;
    Slider slider;

    void Start()
    {
        background = GetComponentInChildren<Image>();
        slider = GetComponent<Slider>();
    }


	public void UpdateVie(float vie)
    {
        slider.value = (vie / 100f);
        Color couleur;
        couleur.r = (1f - (vie / 100f));
        couleur.g = (vie / 100f);
        couleur.b = 0f;
        couleur.a = 1f;
        background.color = couleur;
    }
}
