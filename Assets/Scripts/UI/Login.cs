using System.Collections;
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
        string phoneText = StringProcess.Trim(phone.text);
        string passwordText = StringProcess.Trim(password.text);
        if (phoneText == "" || passwordText == "")
        {
            statusText.text = "请输入手机号和密码！";
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/login?phone=" + phoneText + "&password=" + passwordText);
        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);
        if (request.downloadHandler.text == "success")
        {
            statusText.text = "登录成功！";
            statusText.color = Color.green;

            yield return new WaitForSeconds(1);
            GameManager.instance.phone = phoneText;
            GameManager.instance.password = passwordText;
            GameManager.instance.LoadScene("Main");
        }
        else
        {
            statusText.text = "登录失败！";
            statusText.color = Color.red;
        }
    }
}
