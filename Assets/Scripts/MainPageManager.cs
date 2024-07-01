using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPageManager : MonoBehaviour
{
    public GameObject[] pages;

    private void Start()
    {
        DisableAllPages();
        pages[0].SetActive(true);
    }

    private void DisableAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }

    public void OnPageButtonClicked(string pageName)
    {
        bool flag = false;

        foreach (GameObject page in pages)
        {
            if (page.name == pageName)
            {
                page.SetActive(true);
                flag = true;
            }
            else
            {
                page.SetActive(false);
            }
        }

        if (!flag)
        {
            pages[0].SetActive(true);
        }
    }
}
