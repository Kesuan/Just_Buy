using System.Collections;
using TMPro;
using UnityEngine;

public class GlobalNotification : MonoBehaviour
{
    private TextMeshProUGUI notifyText;

    public static GlobalNotification instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        notifyText = GetComponent<TextMeshProUGUI>();

        notifyText.enabled = false;
    }

    public void Notify(string text)
    {
        notifyText.enabled = true;
        notifyText.text = text;
        notifyText.alpha = 1;
        StartCoroutine(HideNotify());
    }

    IEnumerator HideNotify()
    {
        // 逐渐透明
        for (float i = 1; i >= 0; i -= 0.01f)
        {
            notifyText.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        notifyText.enabled = false;
    }
}
