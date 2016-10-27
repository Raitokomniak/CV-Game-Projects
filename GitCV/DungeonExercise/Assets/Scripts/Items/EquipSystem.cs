using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipSystem : MonoBehaviour {
	SaveLoadController data;
	AudioController audio;
	Stats playerStats;
	InventorySystem inventorySystem;
	UIController ui;

	int equipSlotQuantity = 2;
	public int[] equippedItemID;
	public GameObject[] equipSlots;
	public bool[] slotEmpty;
	public int equipEmptyValue;

	Text statsStrengthValue;
	Text statsStaminaValue;

	public int bonusStrength;
	public int bonusStamina;

	public bool initialized;


	// Use this for initialization
	void Awake () {
		data = GameObject.Find("GameControl").GetComponent<SaveLoadController>();
		audio = GetComponent<AudioController>();
		ui = GetComponent<UIController>();
		playerStats = GameObject.Find("Player").GetComponent<Stats>();
		inventorySystem = GetComponent<InventorySystem>();

		equipSlots = new GameObject[equipSlotQuantity];
		equipEmptyValue = 90;
		equippedItemID = new int[2];

		InitializeEquipment();
	}
	
	// Update is called once per frame
	void Update () {

		playerStats.strength = playerStats.initialStr + bonusStrength;
		playerStats.stamina = playerStats.initialSta + bonusStamina;

	}



	public void Equip(string type, int itemid)
	{
		audio = GetComponent<AudioController>();

			for(int i = 0; i < equipSlots.Length; i++)
			{
			if(equippedItemID[i] == equipEmptyValue)
				{
				ui.setCharacterScreenActive(true);
				if(inventorySystem.charScreenActive == true)
				{
					equipSlots[i] = GameObject.Find("equipSlot_" + i);
					equipSlots[i].GetComponent<Image>().sprite = inventorySystem.items[itemid].GetComponent<Image>().sprite;
				}

					UpdateStats(itemid, 1);
					equippedItemID[i] = itemid;
					UpdateToData(i, itemid, "Equip");
					return;
				}
			}
	}



	public void UnEquip(int id)
	{
		if(equippedItemID[id] != equipEmptyValue)
		{
			if(inventorySystem.IsAnySlotEmpty())
			{
			audio.PlayUISound("Audio/FX/Inventory_Open_00");
			inventorySystem.UnEquip(equippedItemID[id], id);
			UpdateStats(equippedItemID[id], 0);
			equippedItemID[id] = equipEmptyValue;
			UpdateToData(id, equippedItemID[id], "Remove");
			}
			else
			Debug.Log("inventory full");
		}
	}



	public void InitializeEquipment()
	{
		bonusStrength = 0;
		bonusStamina = 0;
		for(int i=0; i<equipSlotQuantity; i++)
		{
			equippedItemID[i] = equipEmptyValue;
		}
		LoadEquips();
		initialized = true;
	}




	public void LoadEquips()
	{


		data.LoadItemResources();

		if(data.equip0 != equipEmptyValue) Equip(data.items[data.equip0].GetComponent<Item>().type, data.equip0);
		if(data.equip1 != equipEmptyValue) Equip(data.items[data.equip1].GetComponent<Item>().type, data.equip1);
	}

	public void LoadSprites()
	{
		for(int i = 0; i < equipSlotQuantity; i++)
		{
			if(equippedItemID[i] != equipEmptyValue)
			{
				equipSlots[i] = GameObject.Find("equipSlot_" + i);
				equipSlots[i].GetComponent<Image>().sprite = inventorySystem.items[equippedItemID[i]].GetComponent<Image>().sprite;
			}
		}
	}

	void UpdateToData(int slotID, int itemID, string EquipOrRemove)
	{
		int id;

		if(EquipOrRemove == "Equip") id = itemID;
		else id = equipEmptyValue;

		switch(slotID)
		{
		case 0:
			data.equip0 = id;
			break;
		case 1:
			data.equip1 = id;
			break;
		}
	}



	void UpdateStats(int id, int direction)
	{
		switch(direction)
		{
		case(1):
		bonusStrength += data.items[id].GetComponent<Item>().strength;
		bonusStamina += data.items[id].GetComponent<Item>().stamina;
		
		break;
		case(0):
		bonusStrength -= data.items[id].GetComponent<Item>().strength;
		bonusStamina -= data.items[id].GetComponent<Item>().stamina;
		if(bonusStrength <= 0) bonusStrength = 0;
		if(bonusStamina <= 0) bonusStamina = 0;
		break;
		}
	}
}
