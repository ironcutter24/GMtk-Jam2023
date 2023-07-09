using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealthSlider : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI healthLabel;

    [SerializeField]
    Slider healthSlider;

    private void Update()
    {
        healthSlider.value = GameManager.Instance.Hero.NormalizedHealth;
        healthLabel.SetText("HP: " + GameManager.Instance.Hero.Health.ToString());
    }
}
