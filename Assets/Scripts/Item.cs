using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string iname = "Lian";
    public string showname;
    public string description;
    public float price;
    public bool ar;

    public Image image;
    public GameObject arButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;

    public void InitItem(string _iname, string _showname, string _description, float _price, bool _ar)
    {
        iname = _iname;
        showname = _showname;
        description = _description;
        price = _price;
        ar = _ar;

        nameText.text = showname;
        descriptionText.text = description;
        priceText.text = price.ToString();
        arButton.SetActive(ar);

        StartCoroutine(DownloadImage());
    }

    public void OnARButtonClicked()
    {
        GameObject arPrefab = Resources.Load<GameObject>("Prefabs/" + iname);
        if (arPrefab == null)
        {
            Debug.LogError("AR Prefab not found");
            return;
        }
        GameManager.instance.arPrefab = arPrefab;
        GameManager.instance.LoadScene("AR");
    }

    public void OnBuyButtonClicked()
    {
    }

    public IEnumerator DownloadImage()
    {
        string url = "http://47.94.221.211:3000/images/" + iname + ".png";
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        byte[] body = request.downloadHandler.data;

        string path = Application.persistentDataPath + "/" + iname + ".png";
        System.IO.File.WriteAllBytes(path, body);

        Texture2D texture = new(2, 2);
        texture.LoadImage(body);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        image.sprite = sprite;
    }
}
