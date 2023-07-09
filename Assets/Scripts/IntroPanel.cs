using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(!GameManager.Instance.hasBegun);
    }

    bool flag = false;
    public void BeginGame()
    {
        flag = true;
    }

    public IEnumerator _ShowIntro()
    {
        gameObject.SetActive(true);

        yield return new WaitUntil(() => flag);

        GameManager.Instance.hasBegun = true;
        MusicManager.Instance.StartMusic();
        gameObject.SetActive(false);
    }
}
