using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnector : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetUploadSpecialData());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetUploadData()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/uploadData");
        yield return request.SendWebRequest();

        // Debug.Log("Response: " + request.downloadHandler.text);
        text.text = request.downloadHandler.text;
    }

    IEnumerator GetUploadSpecialData()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/uploadData?uid=10010&pwd=123456");
        yield return request.SendWebRequest();

        // Debug.Log("Response: " + request.downloadHandler.text);
        text.text = request.downloadHandler.text;
    }
}
