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
    public GameObject infoLocal;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        if (!Items.Contains(item))
        {
            Items.Add(item);
        }else if(item.itemName.Equals("Sal") || item.itemName.Equals("Bandagens"))
        {
            Items[Items.IndexOf(item)].value += 1;
        }
        ListItem(item);
    }

    /*
    // funcao que funciona antes 
    public void Remove(Item item)
    {
        Items.Remove(item);
    }
    */

    // ** funcao que eu fiz q pode quebrar o codigo**
    public void Remove(Item item)
    {
        if (Items.Contains(item))
        {
            if (item.itemName.Equals("Sal") || item.itemName.Equals("Bandagens"))
            {
                if(Items[Items.IndexOf(item)].value > 0)
                {
                    Items[Items.IndexOf(item)].value -= 1;
                }
            }
            else
            {
                Items.Remove(item);
            }
            Debug.Log(item.itemName + " removido. Quantidade restante: " + Items.Count);
            ListItem(item); // Atualizar o UI após remover item
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (Item item in Items)
        {
            if (item.itemName.Equals(itemName) && item.value > 0)
            {
                return true;
            }
        }
        return false;
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
        if (item.itemName.Equals("Sal") || item.itemName.Equals("Bandagens"))
        {
            foreach(Transform obj in ItemContent)
            {
                var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
                int pos = Items.IndexOf(item);
                if (itemName.text.Equals(item.itemName))
                {
                    var itemCount = obj.transform.Find("Quantidade").GetComponent<TMPro.TextMeshProUGUI>();
                    itemCount.text = Items[pos].value.ToString();
                    Dialogue dialogo = new Dialogue();
                    dialogo.characterImage = item.icon;
                    dialogo.sentences = new List<string>();
                    dialogo.sentences.Add(item.description);
                    DialogueTrigger trigger = obj.transform.GetComponent<DialogueTrigger>();
                    trigger.dialogue = dialogo;
                    trigger.dialogueBox = infoLocal;
                    break;
                }
            }
        }
        else
        {
            foreach (Transform objec in ItemContent)
            {
                var name = objec.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
                if (item.itemName.Equals(name.text))
                {
                    return;
                }
            }
            GameObject obj = Instantiate(inventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCount = obj.transform.Find("Quantidade").GetComponent<TMPro.TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemCount.text = null;

            Dialogue dialogo = new Dialogue();
            dialogo.characterImage = item.imageDetails;
            dialogo.sentences = new List<string>();
            dialogo.sentences.Add(item.description);
            DialogueTrigger trigger = obj.transform.GetComponent<DialogueTrigger>();
            trigger.dialogue = dialogo;
            trigger.dialogueBox = infoLocal;
        }
    }
}
