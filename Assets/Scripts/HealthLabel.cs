using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthLabel : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Start()
    {
        healthText = gameObject.GetComponent<TextMeshProUGUI>();
        healthText.SetText("HP: " + GameManager.Instance.Hero.Health.ToString());
    }
}
