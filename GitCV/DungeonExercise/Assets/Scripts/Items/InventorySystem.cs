using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySystem : MonoBehaviour {

	SaveLoadController data;
	AudioController audio;
	GameObject characterScreen;
	EquipSystem equipSystem;
	UIController ui;

	public Sprite defaultImage;		// Default empty slot image
	int slotQuantity;

	public GameObject[] inventory;	//Inventory for gameobjects
	GameObject[] inventorySlots;	//Inventoryslot ui elements
	public GameObject[] items;		//Library for all items
	int inventoryEmptyValue;
	int equipOrder;

	bool initialized;
	public bool charScreenActive;


	void Awake () {
		equipSystem = GetComponent<EquipSystem>();
		ui = GetComponent<UIController>();
		characterScreen = GameObject.Find("CharacterScreen");
		audio = GetComponent<AudioController>();

		//Find save data
		data = GameObject.Find("GameControl").GetComponent<SaveLoadController>();
		inventoryEmptyValue = 90;							//Find item id of "empty"
		items = data.items;

		equipOrder = 0;
		slotQuantity = 3;									//Define how many inventory slots there are
		inventorySlots = new GameObject[slotQuantity]; 		//Create UI slots array

		defaultImage = Resources.Load<Sprite>("emptySlot"); //Find default slot image

		data.LoadItemResources(); 								// Load resources of all items
		InitializeInventory();								// Initialize


		charScreenActive = false;
	}
		

	void Update () {
		//Check for initialization of equip system on first frame
		if(equipSystem.initialized && !initialized) {
			ui.setCharacterScreenActive(false);
			initialized = true;
		}

		inventory = data.inventory; 			//Create inventory array for storing game objects

	}

	public void LoadSprites()
	{
		for(int i = 0; i < slotQuantity; i++)
		{
			if(inventory[i] != null)
			{
				inventorySlots[i] = GameObject.Find("inventorySlot_" + i);
				inventorySlots[i].GetComponent<Image>().sprite = inventory[i].GetComponent<Image>().sprite;
			}
		}
	}


	//CHECKS IF ANY SLOT IS EMPTY, RESULTS IN INVENTORY FULL WARNING
	public bool IsAnySlotEmpty()
	{
		for(int i = 0; i < slotQuantity; i++)
		{
			if(inventory[i] == null)
			{
				return true;
			}
		}
		return false;
	}

	public void ForceLoot()
	{
		//inventory = new GameObject[3];
		items = data.items;
		inventory[0] = items[0];
		inventorySlots[0] = GameObject.Find("inventorySlot_" + 0);
		inventorySlots[0].GetComponent<Image>().sprite = items[0].GetComponent<Image>().sprite;
	}

	public void Loot(int itemID)
	{
		
		for(int i = 0; i < inventorySlots.Length; i++) 	//Loop through inventory slots
		{
			if(inventory[i] == null)					//Check for empty inventory slot
			{											//and add item to that slot
				Debug.Log(i + "is null");
				inventory[i] = items[itemID];

				if(charScreenActive == true) {
					inventorySlots[i] = GameObject.Find("inventorySlot_" + i);
					inventorySlots[i].GetComponent<Image>().sprite = items[itemID].GetComponent<Image>().sprite;
				}

				UpdateToData(i, itemID, "Loot");
				return;
			}
		}
		Debug.Log("inventory full");

		//LoadItemResources();

		/*
		if(IsAnySlotEmpty())
		{
			for(int i = 0; i < inventorySlots.Length; i++) 	//Loop through inventory slots
			{
				if(inventory[i] == null)					//Check for empty inventory slot
				{											//and add item to that slot
					inventory[i] = items[itemID];

					if(charScreenActive == true) {
					inventorySlots[i] = GameObject.Find("inventorySlot_" + i);
					inventorySlots[i].GetComponent<Image>().sprite = items[itemID].GetComponent<Image>().sprite;
					}

					UpdateToData(i, itemID, "Loot");
					return;
				}
			}
		}
		else { ui.SetWarningText("Inventory is full"); }
		*/
	}



	//RETURNS UNEQUIPPED ITEMS BACK TO FIRST EMPTY INVENTORY SLOT
	public void UnEquip(int itemID, int equipSlotID)
	{
		for(int i = 0; i < inventorySlots.Length; i++)	  //Loop through inventory slots
		{
			if(data.inventory[i] == null)					  //Check for empty inventory slot
			{
				inventory[i] = data.items[itemID];
				inventorySlots[i] = GameObject.Find("inventorySlot_" + i);
				inventorySlots[i].GetComponent<Image>().sprite = equipSystem.equipSlots[equipSlotID].GetComponent<Image>().sprite;
				equipSystem.equipSlots[equipSlotID].GetComponent<Image>().sprite = defaultImage;
				UpdateToData(i, itemID, "Loot");
				return;
			}
		}
	}



	//UPDATES INVENTORY CHANGES TO DATA 
	void UpdateToData(int slotID, int itemID, string LootOrRemove)
	{
		int id;
		if(LootOrRemove == "Loot") id = itemID;		//Change id based on
		else id = inventoryEmptyValue;				//whether adding or removing

		switch(slotID) {
		case 0:
			data.inventory0 = id;
			break;
		case 1:
			data.inventory1 = id;
			break;
		case 2:
			data.inventory2 = id;
			break;
		}
	}


	//CLICKING ON EQUIPPABLE OBJECT EQUIPS IT BY TYPE AND ID
	public void SelectSlot(int slotID)
	{
		if(inventory[slotID] != null)
		{
			for(int i = 0; i < equipSystem.equipSlots.Length; i++)
			{
				if(equipSystem.equippedItemID[i] != equipSystem.equipEmptyValue && equipSystem.equippedItemID[i+1] != equipSystem.equipEmptyValue)
				{
					if(IsAnySlotEmpty())
					{
						equipSystem.UnEquip(equipOrder);
					}
					else
					{
					GameObject repository = inventory[slotID];
					data.inventory[slotID] = null;
					equipSystem.UnEquip(equipOrder);
					equipSystem.Equip(repository.GetComponent<Item>().type, repository.GetComponent<Item>().id);

					if(equipOrder == 0) equipOrder = 1;
					else equipOrder = 0;
	
					return;
					}
				}
				Debug.Log("equipping");
				inventorySlots[slotID].GetComponent<Image>().sprite = defaultImage; 
				equipSystem.Equip(inventory[slotID].GetComponent<Item>().type, inventory[slotID].GetComponent<Item>().id);
				UpdateToData(slotID, inventory[slotID].GetComponent<Item>().id, "Remove");
				inventory[slotID] = null;
				return;
			}
		}
		else{
		}
	}


	//INITIALIZATION
	void InitializeInventory() {	
		//inventory = new GameObject[slotQuantity];

		for(int i=0; i<slotQuantity; i++) {
			inventorySlots[i] = GameObject.Find("inventorySlot_" + i);
			//inventory[i] = null;
		}
		equipSystem.InitializeEquipment();
		LoadInventory();
	}


	//LOAD INVENTORY FROM DATA FILE
	void LoadInventory(){

		if(data.inventory0 != inventoryEmptyValue) data.inventory[0] = items[data.inventory0];
		if(data.inventory1 != inventoryEmptyValue) data.inventory[1] = items[data.inventory1];
		if(data.inventory2 != inventoryEmptyValue) data.inventory[2] = items[data.inventory2];

		equipSystem.LoadEquips();
	}
}
