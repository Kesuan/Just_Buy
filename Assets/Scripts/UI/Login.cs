using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public TextMeshProUGUI phone;
    public TextMeshProUGUI password;
    public TextMeshProUGUI statusText;

    public void OnLoginButtonClicked()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        if (phone.text == "" || password.text == "")
        {
            statusText.text = "Please fill in all fields!";
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/login?phone=" + phone.text + "&password=" + password.text);
        yield return request.SendWebRequest();

        if (request.downloadHandler.text == "success")
        {
            statusText.text = "Login Success!";
        }
        else
        {
            statusText.text = "Login Failed!";
        }
    }
}
