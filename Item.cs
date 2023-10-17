using UnityEngine;

// Class to represent a generic inventory item
[CreateAssetMenu(fileName = "New Item",menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;  // The name of the item
    public Sprite icon;  // The icon of the item
    public int value;
    public bool isEquippable = false;  // Whether the item can be equipped
}
