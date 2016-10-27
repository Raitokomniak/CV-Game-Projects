using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class Item {
	public string spritePath;

	public string type;
	public bool equippable;

	public abstract void SetIndex(int i);
	public abstract int GetIndex();
}

[Serializable]
public class Weapon : Item {
	public int weaponIndex;
	public int damage;

	public override void SetIndex(int i){
		weaponIndex = i;
	}
	public override int GetIndex(){
		return weaponIndex;
	}
	public void SetDamage(int d){
		damage = d;
	}
	public int GetDamage(){
		return damage;
	}
		

}

[Serializable]
public class Ring : Item {
	public int ringIndex;

	public override void SetIndex(int i){
		ringIndex = i;
	}
	public override int GetIndex(){
		return ringIndex;
	}
}

public class ItemController : MonoBehaviour {

	public ArrayList allItems;

	public Item[] inventory;
	int inventorySize;

	// Use this for initialization
	void Start () {
		allItems = new ArrayList ();

		inventorySize = 3;
		inventory = new Item[inventorySize];
		CreateItems ();
		//AddToInventory ((Item)allItems[0]);
	}
	
	public void CreateItems(){
		

		for (int i = 1; i < 3; i++) {
			Weapon weapon = new Weapon ();
			weapon.SetIndex (i);
			weapon.type = "Weapon";
			weapon.SetDamage (i*3);
			weapon.spritePath = "UI/Icons/Items/Equipment/" + weapon.type + "_" + weapon.GetIndex();
			weapon.equippable = true;
			allItems.Add (weapon);
		}

		Ring ring1 = new Ring ();
		ring1.type = "Ring";
		ring1.SetIndex (1);
		ring1.equippable = true;
		ring1.spritePath = "UI/Icons/Items/Equipment/" + ring1.type + "_" + ring1.GetIndex();


		allItems.Add (ring1);
	}


	public void UpdateInventoryData(){
		GameControl.gameControl.inventory = inventory;
	}

	public void AddToInventory(Item item, bool isUnequip){
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory [i] == null) {
				inventory [i] = item;
				GameControl.gameControl.ui.AddToInventory (item, i);
				break;
			}
		}

		if (isUnequip) {
			CharacterData character = (CharacterData)GameControl.gameControl.playerList [GameControl.gameControl.ui.selectedCharacterInMenu];

			if (character.weapon != null && item == character.weapon) {
				character.weapon = null;
			}
			if (character.ring != null && item == character.ring) {
				character.ring = null;
			}
		}
	}



	public void RemoveFromInventory(int index)
	{
		inventory [index] = null;
		GameControl.gameControl.ui.RemoveFromInventory (index);
	}

	public void Equip(Item item, int characterIndex, int fromslot){
		CharacterData character = GameControl.gameControl.playerList [characterIndex] as CharacterData;
		Item checkedItemType = item;

		if (item.type == "Weapon") {
			checkedItemType = character.weapon;
		} else if (item.type == "Ring") {
			checkedItemType = character.ring;
		}

		if (checkedItemType != null) {
			Unequip (checkedItemType, characterIndex);
		}

		if (item.type == "Weapon") {
			character.weapon = (Weapon)item;
		} else if (item.type == "Ring") {
			character.ring = (Ring)item;
		}
	
		Item newItem = (Item)inventory[fromslot];
		GameControl.gameControl.ui.UpdateSlots(character);
		RemoveFromInventory (fromslot);
		GameControl.gameControl.CalculateStats (character, item);
		GameControl.gameControl.ui.RefreshEquip (character);

	}

	public void Unequip(Item item, int characterIndex)
	{
		CharacterData character = (CharacterData)GameControl.gameControl.playerList [characterIndex];
		AddToInventory (item, true);
		GameControl.gameControl.CalculateStats (character, item);
		GameControl.gameControl.ui.RefreshEquip (character);
	}
}
