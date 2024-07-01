using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserClass
{
    public string phone;
    public string password;
    public string nickname;
    public string address;
}

public class UserPageManager : MonoBehaviour
{
    private UserClass user;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI nickname;
    public TMP_InputField phoneInput;
    public TMP_InputField nicknameInput;
    public TMP_InputField addressInput;

    private void Start()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        StartCoroutine(GetUser());
    }

    private UserClass ReturnUserJsonDataByJsonUtility(string userJson)
    {
        UserClass user = JsonUtility.FromJson<UserClass>(userJson);
        return user;
    }

    public void OnResetButtonClicked()
    {
        StartCoroutine(SetUser());
    }

    IEnumerator GetUser()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/user?phone=" + GameManager.instance.phone);
        yield return request.SendWebRequest();

        // Debug.Log("Response: " + request.downloadHandler.text);
        string userJson = request.downloadHandler.text;
        userJson = userJson.Substring(1, userJson.Length - 2);

        UserClass user = ReturnUserJsonDataByJsonUtility(userJson);

        this.user = user;
        if (user.nickname != null && StringProcess.Trim(user.nickname) != "")
        {
            nickname.text = user.nickname;
            Debug.Log(user.nickname);
        }
        else
        {
            nickname.text = user.phone;
            Debug.Log(user.phone);
        }
        phoneInput.text = user.phone;
        nicknameInput.text = user.nickname;
        addressInput.text = user.address;
    }

    IEnumerator SetUser()
    {
        string nickname = StringProcess.Trim(nicknameInput.text);
        string address = StringProcess.Trim(addressInput.text);

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/setuser?phone=" + GameManager.instance.phone + "&nickname=" + nickname + "&address=" + address);
        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);
        if (request.downloadHandler.text == "success")
        {
            GameManager.instance.phone = phoneInput.text;
            statusText.color = Color.green;
            statusText.text = "修改成功！";

            yield return new WaitForSeconds(1);
            GameManager.instance.LoadScene("Main");
        }
        else
        {
            statusText.color = Color.red;
            statusText.text = "修改失败！";
        }
    }
}
