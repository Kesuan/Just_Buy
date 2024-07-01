using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class OrderClass
{
    public string oid;
    public string phone;
    public string list;
    public float price;
    public string address;
    public string time;
}

public class OrderPageManager : MonoBehaviour
{
    public GameObject content;
    public GameObject OrderPrefab;

    private void Start()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        StartCoroutine(GetOrders());
    }

    private OrderClass ReturnItemJsonDataByJsonUtility(string json)
    {
        OrderClass order = JsonUtility.FromJson<OrderClass>(json);
        return order;
    }

    IEnumerator GetOrders()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/orders?phone=" + GameManager.instance.phone);
        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);

        if (request.downloadHandler.text == "[]")
        {
            GlobalNotification.instance.Notify("暂无订单!");
            yield break;
        }

        string[] orders = request.downloadHandler.text.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < orders.Length; i++)
        {
            string order = orders[i];
            if (i == 0)
            {
                // 去掉第一个字符"["
                order = order[1..];
            }
            else
            {
                order = "{" + order;
            }

            if (i != orders.Length - 1)
            {
                order += "}";
            }
            else
            {
                // 去掉最后一个字符"]"
                order = order[..^1];
            }

            OrderClass orderClass = ReturnItemJsonDataByJsonUtility(order);
            GameObject orderObject = Instantiate(OrderPrefab, content.transform);
            orderObject.GetComponent<Order>().InitOrder(orderClass);
        }
    }
}
