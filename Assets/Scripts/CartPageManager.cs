using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class CartItemClass
{
    public string phone;
    public string iname;
    public string showname;
    public string description;
    public float price;
    public int ar;
}

public class CartPageManager : MonoBehaviour
{
    public GameObject content;
    public GameObject ItemPrefab;

    public TextMeshProUGUI totalPriceText;
    private float totalPrice = 0;

    private void Start()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        StartCoroutine(GetItems());
    }

    private CartItemClass ReturnItemJsonDataByJsonUtility(string json)
    {
        CartItemClass item = JsonUtility.FromJson<CartItemClass>(json);
        // Debug.Log(item.iname);
        // Debug.Log(item.showname);
        // Debug.Log(item.description);
        // Debug.Log(item.price);
        // Debug.Log(item.ar);
        return item;
    }

    IEnumerator GetItems()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/cart_items?phone=" + GameManager.instance.phone);
        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);

        if (request.downloadHandler.text == "[]")
        {
            totalPriceText.text = "总价:0元";
            yield break;
        }

        string[] items = request.downloadHandler.text.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < items.Length; i++)
        {
            string item = items[i];
            if (i == 0)
            {
                // 去掉第一个字符"["
                item = item[1..];
            }
            else
            {
                item = "{" + item;
            }

            if (i != items.Length - 1)
            {
                item += "}";
            }
            else
            {
                // 去掉最后一个字符"]"
                item = item[..^1];
            }

            CartItemClass itemClass = ReturnItemJsonDataByJsonUtility(item);
            GameObject itemObject = Instantiate(ItemPrefab, content.transform);
            itemObject.GetComponent<Item>().InitItem(itemClass.iname, itemClass.showname, itemClass.description, itemClass.price, itemClass.ar == 1);
            totalPrice += itemClass.price;
        }

        totalPriceText.text = "总价:" + totalPrice.ToString() + "元";
    }

    public void OnBuyButtonClicked()
    {
        StartCoroutine(SubmitOrder());
    }

    IEnumerator SubmitOrder()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/address?phone=" + GameManager.instance.phone);
        yield return request.SendWebRequest();

        // Debug.Log("Address Response: " + request.downloadHandler.text);
        // Address Response: [{"address":"上海静安区"}]
        string address = request.downloadHandler.text.Split(new string[] { "address\":\"", "\"}" }, StringSplitOptions.RemoveEmptyEntries)[1];
        // Debug.Log("Address: " + address);
        if (StringProcess.Trim(address) == "")
        {
            GlobalNotification.instance.Notify("请先设置收获地址!");
            yield break;
        }

        Item[] items = content.GetComponentsInChildren<Item>();
        string list = "";
        foreach (Item item in items)
        {
            list += item.showname + ";";
        }
        request = UnityWebRequest.Get("http://47.94.221.211:3000/submit_order?phone=" + GameManager.instance.phone + "&list=" + list + "&price=" + totalPrice.ToString() + "&address=" + address);
        yield return request.SendWebRequest();

        Debug.Log("Submit Order Response: " + request.downloadHandler.text);

        if (request.downloadHandler.text == "success")
        {
            Debug.Log("Submit Order Success");
            GlobalNotification.instance.Notify("提交成功!");

            foreach (Item item in items)
            {
                item.OnRemoveButtonClicked();
                Destroy(item.gameObject);
            }

            GameManager.instance.LoadScene("Main");
        }
        else
        {
            Debug.Log("Submit Order Failed");
            GlobalNotification.instance.Notify("提交失败!");
        }
    }

    public void DecreaseTotalPrice(float price)
    {
        totalPrice -= price;
        totalPriceText.text = "总价:" + totalPrice.ToString() + "元";
    }
}
