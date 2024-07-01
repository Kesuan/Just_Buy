using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string iname;
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
        priceText.text = "单价:" + price.ToString() + "元";
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
        StartCoroutine(AddItemToCart(this));
    }

    IEnumerator AddItemToCart(Item item)
    {
        string phone = GameManager.instance.phone;
        int arValue = item.ar ? 1 : 0;

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/add_cart?phone=" + phone + "&iname=" + item.iname + "&showname=" + item.showname + "&description=" + item.description + "&price=" + item.price + "&ar=" + arValue);
        yield return request.SendWebRequest();

        Debug.Log("Add Item to Cart Response: " + request.downloadHandler.text);

        if (request.downloadHandler.text == "success")
        {
            Debug.Log("Add Item to Cart Success");
            GlobalNotification.instance.Notify("添加成功!");
        }
        else
        {
            Debug.Log("Add Item to Cart Failed");
            GlobalNotification.instance.Notify("添加失败!");
        }

        GameManager.instance.LoadScene("Main");
    }

    public void OnRemoveButtonClicked()
    {
        StartCoroutine(RemoveItemFromCart(this));
    }

    IEnumerator RemoveItemFromCart(Item item)
    {
        string phone = GameManager.instance.phone;

        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/remove_cart?phone=" + phone + "&iname=" + item.iname);
        yield return request.SendWebRequest();

        Debug.Log("Remove Item from Cart Response: " + request.downloadHandler.text);

        if (request.downloadHandler.text == "success")
        {
            Debug.Log("Remove Item from Cart Success");
            // GlobalNotification.instance.Notify("移除成功!");
        }
        else
        {
            Debug.Log("Remove Item from Cart Failed");
            GlobalNotification.instance.Notify("移除失败!");
        }

        CartPageManager cartPageManager = FindObjectOfType<CartPageManager>();
        cartPageManager.DecreaseTotalPrice(price);
        Destroy(gameObject);
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
