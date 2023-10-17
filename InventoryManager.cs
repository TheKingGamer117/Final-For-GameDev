using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;
    private void Awake()
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
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }

            InventoryItemController controller = obj.GetComponent<InventoryItemController>();
            controller.AddItem(item);  // Adding this line to make sure the Item object is properly set.
            removeButton.onClick.AddListener(controller.RemoveItem);  // Hook up the RemoveItem() function to the click event of RemoveButton

        }

        SetInventoryItems();
    }

    public void EnableItemRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < InventoryItems.Length; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
        }
    }

    public void SellItems()
    {
        int totalValue = 0;

        List<int> deadFishIDs = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        // Loop through items and sum the value of dead fish and treasure
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            if (deadFishIDs.Contains(Items[i].id) || Items[i].id == 9)
            {
                totalValue += Items[i].value;
                Items.RemoveAt(i);
            }
        }

        // Find the PlayerStats script to add gold to it
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddGold(totalValue);
        }

        ListItems();
        // Update inventory UI or other actions...
    }

}
