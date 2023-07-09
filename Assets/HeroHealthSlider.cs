using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealthSlider : MonoBehaviour
{

    Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider = gameObject.GetComponent<Slider>();
        healthSlider.value = GameManager.Instance.Hero.Health;
    }


}
