using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.Instance.hasBegun)
        {
            gameObject.SetActive(false);
        }
        else {
            Time.timeScale = 0;
        }
    }

    public void BeginGame() {
        GameManager.Instance.hasBegun = true;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
