using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ItemClass
{
    public string iname;
    public string showname;
    public string description;
    public float price;
    public int ar;
}

public class DefaultPageManager : MonoBehaviour
{
    public GameObject content;
    public GameObject ItemPrefab;

    private void Start()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        StartCoroutine(GetItems());
    }

    private ItemClass ReturnItemJsonDataByJsonUtility(string json)
    {
        ItemClass item = JsonUtility.FromJson<ItemClass>(json);
        return item;
    }

    IEnumerator GetItems()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://47.94.221.211:3000/items");
        yield return request.SendWebRequest();

        // Debug.Log("Response: " + request.downloadHandler.text);

        string[] items = request.downloadHandler.text.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < items.Length; i++)
        {
            string item = items[i];
            if (i == 0)
            {
                // 去掉第一个字符"["
                item = item.Substring(1);
            }
            else
            {
                item = "{" + item;
            }

            if (i != items.Length - 1)
            {
                item = item + "}";
            }
            else
            {
                // 去掉最后一个字符"]"
                item = item.Substring(0, item.Length - 1);
            }

            ItemClass itemClass = ReturnItemJsonDataByJsonUtility(item);
            GameObject itemObject = Instantiate(ItemPrefab, content.transform);
            itemObject.GetComponent<Item>().InitItem(itemClass.iname, itemClass.showname, itemClass.description, itemClass.price, itemClass.ar == 1);
        }
    }
}

