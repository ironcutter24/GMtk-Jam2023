using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupLocation : MonoBehaviour
{
    public GameObject panel;

    public abstract void UpdateHeroStatus();

    protected abstract void OnDrawGizmos();

    private void Start()
    {
        panel.SetActive(false);
    }

    public void DisplayPanel() {
        
        StartCoroutine(_DisplayingPanel());

        IEnumerator _DisplayingPanel() {
            panel.SetActive(true);
            yield return new WaitForSeconds(2f);
            panel.SetActive(false);
        }

    }

}
