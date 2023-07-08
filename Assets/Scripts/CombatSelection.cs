using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSelection : MonoBehaviour
{

    public Monster monster;
    public Slider monsterHealthSlider;
    public Slider heroHealthSlider;

    [SerializeField]
    private List<Button> buttons;


    public void SetInteractable(bool state)
    {
        foreach (var button in buttons)
            button.interactable = state;
    }


    public void Attack()
    {
        Debug.Log("Attack Button Pressed");
        monster.SimpleAttack();
    }

    public void Special()
    {
        Debug.Log("Special Button pressed");
    }

    public void Heal()
    {
        Debug.Log("Heal Button Pressed");
    }

    public void Block()
    {
        Debug.Log("Block Button Pressed");
    }


    public void setHealthSlider(int value, Slider slider)
    {
        slider.value = value;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha0)) {
        //    setHealthSlider(0, monsterHealthSlider);
        //}
    }

}
