using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject inventoryItem;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform obj in ItemContent)
        {
            Destroy(obj.gameObject);
        }
        foreach (Item item in Items)
        {
            GameObject obj = Instantiate(inventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCount = obj.transform.Find("Quantidade").GetComponent<TMPro.TextMeshProUGUI>();

            Debug.Log(item.itemName);
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            if (item.itemName.Equals("Sal"))
            {
                itemCount.text = item.value.ToString();
            }
            else
            {
                itemCount.text = "";
            }
        }
    }

    public void ListItem(Item item)
    {
        if (item.itemName.Equals("Sal"))
        {
            foreach(Transform obj in ItemContent)
            {
                var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
                int pos = 0;
                if (itemName.Equals(item.itemName))
                {
                    var itemCount = obj.transform.Find("Quantidade").GetComponent<TMPro.TextMeshProUGUI>();
                    int countText = (Items[pos].value + 1);
                    itemCount.text = countText.ToString();
                    break;
                }
            }
        }
        else
        {
            GameObject obj = Instantiate(inventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCount = obj.transform.Find("Quantidade").GetComponent<TMPro.TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemCount.text = null;
        }
    }
}
