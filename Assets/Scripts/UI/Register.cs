using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class Register : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originPos;
    private Vector3 showPos;

    public GameObject registerPanel;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI phone;
    public TextMeshProUGUI password;
    public TextMeshProUGUI address;

    private void Start()
    {
        rectTransform = registerPanel.GetComponent<RectTransform>();
        originPos = rectTransform.localPosition;
        showPos = originPos + new Vector3(-GameManager.instance.resX, 0, 0);
    }

    private void ShowRegisterPanel()
    {

        rectTransform.DOLocalMove(showPos, 0.5f);
    }

    private void HideRegisterPanel()
    {
        rectTransform.DOLocalMove(originPos, 0.5f);
    }

    public void OnRegisterPanelButtonClicked()
    {
        ShowRegisterPanel();
    }

    public void OnRegisterButtonClicked()
    {
        StartCoroutine(RegisterRequest());
    }

    public void OnBackButtonClicked()
    {
        HideRegisterPanel();
    }

    IEnumerator RegisterRequest()
    {
        string nicknameText = StringProcess.Trim(nickname.text);
        string phoneText = StringProcess.Trim(phone.text);
        string passwordText = StringProcess.Trim(password.text);
        string addressText = StringProcess.Trim(address.text);
        if (phoneText == "" || passwordText == "")
        {
            statusText.text = "请填写必填信息！";
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/register?nickname=" + nicknameText + "&phone=" + phoneText + "&password=" + passwordText + "&address=" + addressText);
        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);
        if (request.downloadHandler.text == "success")
        {
            statusText.text = "注册成功！";
            statusText.color = Color.green;

            yield return new WaitForSeconds(1);
            HideRegisterPanel();
        }
        else if (request.downloadHandler.text == "phone exists")
        {
            statusText.text = "注册失败，手机号已存在！";
            statusText.color = Color.red;
        }
        else
        {
            statusText.text = "注册失败，请联系工作人员！";
            statusText.color = Color.red;
        }
    }
}
