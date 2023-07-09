using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthLabel : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        healthText = gameObject.GetComponent<TextMeshProUGUI>();
        healthText.SetText("HP: " + GameManager.Instance.Hero.Health.ToString());
    }

    private void Awake()
    {
        
    }



}
