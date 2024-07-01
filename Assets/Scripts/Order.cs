using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Order : MonoBehaviour
{
    public string oid;
    public string phone;
    public string list;
    public string address;
    public float price;

    public TextMeshProUGUI oidText;
    public TextMeshProUGUI listText;
    public TextMeshProUGUI addressText;
    public TextMeshProUGUI priceText;

    public void InitOrder(string oid, string phone, string list, string address, float price)
    {
        this.oid = oid;
        this.phone = phone;
        this.list = list;
        this.address = address;
        this.price = price;

        oidText.text = "订单号:" + oid;
        listText.text = "商品清单:" + list;
        addressText.text = "地址:" + address;
        priceText.text = "总价:" + price.ToString() + "元";
    }

    public void InitOrder(OrderClass order)
    {
        this.oid = order.oid;
        this.phone = order.phone;
        this.list = order.list;
        this.address = order.address;
        this.price = order.price;

        oidText.text = "订单号:" + oid;
        listText.text = "商品清单:" + list;
        addressText.text = "地址:" + address;
        priceText.text = "总价:" + price.ToString() + "元";
    }
}
