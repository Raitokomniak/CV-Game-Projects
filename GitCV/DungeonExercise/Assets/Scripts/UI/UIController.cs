using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	SaveLoadController data;

	AudioController audio1;
	InventorySystem inventorySystem;
	EquipSystem equipSystem;

	public Text aggroText;
	bool timerActive;
	float timer;

	public Text warningText;
	public bool warningTextActive;
	public float warningTextTimer;

	public Text infoText;
	public bool infoTextActive;
	public float infoTextTimer;

	Text NPCname;
	Text NPCnameDesc;
	GameObject NPC;
	GameObject WorldCanvas;
	GameObject UICanvas;
	GameObject Dialog;

	GameObject CharacterScreen;
	public Text statsStrengthValue;
	public Text statsStaminaValue;

	string NPCname_temp;
	string NPCname_desc_temp;



	// Use this for initialization
	void Awake () {
		data = SaveLoadController.saveLoadControl;

		audio1 = GetComponent<AudioController>();
		inventorySystem = GetComponent<InventorySystem>();
		equipSystem = GetComponent<EquipSystem>();

		NPCname = GameObject.Find("name_NPC").GetComponent<Text>();
		NPCnameDesc = GameObject.Find("name_NPC_desc").GetComponent<Text>();
		NPC = GameObject.FindWithTag("NPC");

		WorldCanvas = GameObject.Find("WorldCanvas");
		UICanvas = GameObject.Find("UICanvas");
		Dialog = GameObject.Find("DialogPanel");

		CharacterScreen = GameObject.Find("CharacterPanel");
		statsStrengthValue = GameObject.Find("Stats_Strength_Value").GetComponent<Text>();
		statsStaminaValue = GameObject.Find("Stats_Stamina_Value").GetComponent<Text>();
		CharacterScreen.SetActive(false);

		warningTextTimer = 0;
		warningText = GameObject.Find("WarningText").GetComponent<Text>();
		warningText.gameObject.SetActive(false);

		infoTextTimer = 0;
		infoText = GameObject.Find("InfoText").GetComponent<Text>();
		infoText.gameObject.SetActive(false);

		Dialog.SetActive(false);
	}

	void Update () {
		//MAKE TEXT POSITIONS FOLLOW THEIR GAME OBJECTS
		updateNPCNamePosition();

		//KEEP TEXTS UPDATED
		NPCname.text = NPCname_temp;
		NPCnameDesc.text = NPCname_desc_temp;

		//AGGRO TEXT
		FlashAggroText();

		if(Dialog.activeSelf) {
			dialogPanelHandler();
		}

		if(Input.GetButtonDown("CharacterScreen")) {
			
			if(!CharacterScreen.activeSelf)
			{
				CharacterScreen.SetActive(true);
				inventorySystem.charScreenActive = true;
				inventorySystem.LoadSprites();
				equipSystem.LoadSprites();
			}

			else{
				CharacterScreen.SetActive(false);
				inventorySystem.charScreenActive = false;
			}
			audio1.PlayUISound("Audio/FX/Inventory_Open_01");
		}


		if(warningTextActive) {
			warningText.gameObject.SetActive(true);
			warningTextTimer += Time.deltaTime;

			if (warningTextTimer >= 2) {
				warningText.gameObject.SetActive(false);
				warningTextTimer = 0;
				warningTextActive = false;
			}
		}

		if(infoTextActive) {
			infoText.gameObject.SetActive(true);
			infoTextTimer += Time.deltaTime;

			if (infoTextTimer >= 2) {
				infoText.gameObject.SetActive(false);
				infoTextTimer = 0;
				infoTextActive = false;
			}
		}

		if(CharacterScreen.activeSelf) {
			Stats stats = GameObject.Find("Player").GetComponent<Stats>();
			statsStrengthValue.text = (stats.initialStr + equipSystem.bonusStrength).ToString();
			statsStaminaValue.text = (stats.initialSta + equipSystem.bonusStamina).ToString();
		}
	}


	public void SetWarningText(string warning) 
	{
		warningTextTimer = 0;
		warningTextActive = true;
		warningText.text = warning;
	}

	public void SetInfoText(string info) 
	{
		infoTextTimer = 0;
		infoTextActive = true;
		infoText.text = info;
	}

	public void FlashAggroText() 
	{
		if (timerActive) {
			timer -= Time.deltaTime;
			Color redText = new Color(255f, 0f, 0f, 50);
			aggroText.color = redText;
			aggroText.text = "Aggroed!";

			if (timer <= 0) {
				timerActive = false;
			}
		}
	}

	public void updateNPCNamePosition()
	{
		NPCname.transform.position = NPC.transform.position + new Vector3(0f, 1.2f, 0f);
		NPCnameDesc.transform.position = NPC.transform.position + new Vector3(0f, 1f, 0f);
		NPCnameDesc.transform.rotation = GameObject.Find("WorldCanvas").transform.rotation;
	}

	public void updateHorseNamePosition()
	{
	}

	public void setName(string name, string desc)
	{
		NPCname_temp = name;
		NPCname_desc_temp = desc;
	}

	public void toggleDialogPanel(bool mode)
	{
		Dialog.SetActive(mode);
	}

	public void forceDialogOff()
	{
		Debug.Log("success");
		toggleDialogPanel(false);
	}

	void dialogPanelHandler()
	{
		Text Dialog_NPCName = GameObject.Find("Dialog_NPCName").GetComponent<Text>();
		Text Dialog_Content = GameObject.Find("Dialog_ContentText").GetComponent<Text>();
		TextAsset contentText = Resources.Load<TextAsset>("NPCDialog_Content");

		Dialog_NPCName.text = NPCname_temp;
		Dialog_Content.text = contentText.ToString();

	}



	public void setCharacterScreenActive(bool mode)
	{
		CharacterScreen.SetActive(mode);

	}
		
}
