using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class UIController : MonoBehaviour {

	//Controllers
	CharacterStats stats;

	//Dialogue UI
	[SerializeField] public GameObject dialogueCanvas;
	[SerializeField] public Text dialogueSpeakerText;
	[SerializeField] public Text dialogueText;
	[SerializeField] public Image dialogueSpeakerImage;
	[SerializeField] public Button dialogueContinueButton;
	[SerializeField] public Button[] dialogueChoiceButtons;

	///////////////////////////////////
	//Pre-battle UI
	[SerializeField] public GameObject preBattleCanvas;
	[SerializeField] public Button startBattleButton;
	[SerializeField] public Button[] partyButtons;
	[SerializeField] public Button partyNextCharButton;
	[SerializeField] public Button partyPrevCharButton;
	[SerializeField] public Text battleSceneTitle;


	///////////////////////////////////
	//Battle UI
	[SerializeField] public GameObject battleWorldCanvas;
	[SerializeField] public GameObject battleUICanvas;

	[SerializeField] public GameObject victoryScreen;

	[SerializeField] public GameObject characterActionPanel;
	[SerializeField] public Button endTurnButton;
	[SerializeField] public Button moveButton;
	[SerializeField] public Button attackButton;
	[SerializeField] public Button skillButton;

	[SerializeField] public GameObject skillPanel;
	[SerializeField] public Button[] skillButtons;
	[SerializeField] public Button aoeDButton;
	[SerializeField] public Button aoePButton;
	[SerializeField] public Button singleDButton;
	[SerializeField] public Button singlePButton;
	[SerializeField] public Button charSpecButton;

	[SerializeField] public GameObject turnPanel;
	[SerializeField] public Text turnText;

	[SerializeField] public GameObject HP_MP_Bars;
	[SerializeField] public GameObject HPSlider;
	[SerializeField] public GameObject MPSlider;
	ColorBlock cb;

	///////////////////////////////////
	//Overworld UI
	[SerializeField] public GameObject overWorldWorldCanvas;
	[SerializeField] public Button locationButton;


	///////////////////////////////////
	//Menu UI
	[SerializeField] public GameObject menuScreenPanel;
	[SerializeField] public Button characterMenuButton;
	[SerializeField] public Button inventoryMenuButton;


	///////////////////////////////////
	//Character Menu UI
	[SerializeField] public Canvas characterMenuCanvas;
	[SerializeField] public GameObject selectableCharacterPanel;
	[SerializeField] public Button nextCharacterButton;
	[SerializeField] public Button previousCharacterButton;

	[SerializeField] public Image characterImage;
	[SerializeField] public GameObject statsPanel;
	[SerializeField] public Text characterNameText;
	[SerializeField] public Text HPText;
	[SerializeField] public Text levelText;
	[SerializeField] public Text ATKText;

	[SerializeField] public Button weaponButton;
	[SerializeField] public Button ringButton;

	[SerializeField] public GameObject equipmentPanel;
	[SerializeField] public GameObject denyEquipPanel;

	[SerializeField] public Button[] skillSlots;
	[SerializeField] public Button aoeDSlot;
	[SerializeField] public Button aoePSlot;
	[SerializeField] public Button singleDSlot;
	[SerializeField] public Button singlePSlot;
	[SerializeField] public Button charSpecSlot;

	///////////////////////////////////
	//Skill trees
	[SerializeField] public Button fireblastButton;

	///////////////////////////////////
	//Inventory Menu UI
	[SerializeField] public Canvas inventoryMenuCanvas;
	[SerializeField] public GameObject inventoryPanel;
	[SerializeField] public Button inventorySlot;
	[SerializeField] public Sprite weaponSprite;
	[SerializeField] public ArrayList slots;

	///////////////////////////////////
	//Other
	[SerializeField] public GameObject loadingScreen;

	public bool menuOpen;
	public bool characterMenuOpen;
	public bool preBattleMenuOpen;

	public int selectedCharacterInMenu;




	void Start(){
	}


	public void OnGUI(){
		if (GameControl.gameControl.scene.sceneLoaded == false) {
			if (GameControl.gameControl.scene.scene == 0) {
				if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2, 200, 40), "Start new game")) {
					GameControl.gameControl.scene.LoadScene (2, 0);
				} else if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 200, 200, 40), "Load game")) {
					GameControl.gameControl.Load ();
					GameControl.gameControl.scene.LoadScene (2, 0);
				}
				overWorldWorldCanvas.SetActive (false);
				battleWorldCanvas.SetActive (false);
				battleUICanvas.SetActive (false);
				menuScreenPanel.SetActive (false);
				characterMenuCanvas.gameObject.SetActive (false);
			}
		} else if (GameControl.gameControl.scene.scene == 2) {
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height - 50, 200, 40), "Save")) {
				GameControl.gameControl.Save ();
			}
			else if (GUI.Button (new Rect (Screen.width / 2 - 300, Screen.height- 50, 200, 40), "Load")) {
				GameControl.gameControl.Load ();
			}
		}
	}



	// Update is called once per frame
	void Update () {
		if (GameControl.gameControl.scene.scene != 0) {
			KeyListener ();
		}
	}

	void KeyListener(){
		menuOpen = menuScreenPanel.activeSelf;
		characterMenuOpen = characterMenuCanvas.gameObject.activeSelf;

		if (Input.GetButtonDown ("Esc")) {
			if (characterMenuOpen) {
				characterMenuCanvas.gameObject.SetActive (false);

			}
			if (!characterMenuOpen) {
				menuScreenPanel.SetActive (!menuScreenPanel.activeSelf);
			}
		}

	}

	public void SetupOverWorldWorldCanvas(){

		GameObject firstLocationSphere = Instantiate (Resources.Load<GameObject> ("UI/Overworld/LocationSphere"));

		Button firstLocationButton = Instantiate (Resources.Load<Button> ("UI/Overworld/LocationButton"));

		firstLocationButton.transform.SetParent (overWorldWorldCanvas.transform);
		firstLocationButton.transform.position = firstLocationSphere.transform.position + new Vector3(0, 20f, 0);
		firstLocationButton.transform.rotation = Quaternion.Euler (50, 0, 0);
		firstLocationButton.GetComponent<Button>().onClick.AddListener (() => Prebattle(1));

		if (GameControl.gameControl.storyProgression == 1) {
			GameObject secondLocationSphere = Instantiate (Resources.Load<GameObject> ("UI/Overworld/LocationSphere"));
			secondLocationSphere.transform.position = firstLocationButton.transform.position + new Vector3 (30, 0, 40);
			Button secondLocationButton = Instantiate (Resources.Load<Button> ("UI/Overworld/LocationButton"));

			secondLocationButton.transform.SetParent (overWorldWorldCanvas.transform);
			secondLocationButton.transform.position = secondLocationSphere.transform.position + new Vector3(0, 20f, 0);
			secondLocationButton.transform.rotation = Quaternion.Euler (50, 0, 0);
			secondLocationButton.GetComponent<Button>().onClick.AddListener (() => Prebattle(2));
		}


		if(characterMenuCanvas.gameObject.activeSelf) characterMenuCanvas.gameObject.SetActive (false);
		if(menuScreenPanel.activeSelf) menuScreenPanel.SetActive (false);
		toggleLoadingScreen (false);
	}

	////////////////////////////////////
	/// 
	/// PREBATTLE
	/// 
	////////////////////////////////////

	void Prebattle(int battleScene){

		if (battleScene == 1) {
			battleSceneTitle.text = "First battle";
		} else if (battleScene == 2) {
			battleSceneTitle.text = "Second Battle";
		}

		preBattleMenuOpen = true;
		preBattleCanvas.gameObject.SetActive (true);
		startBattleButton.onClick.AddListener(() => GameControl.gameControl.scene.LoadScene(1, 1));

		GameControl.gameControl.partyList.Clear ();

		//Form party automatically at first
		for (int i = 0; i < GameControl.gameControl.partySize; i++) {
			CharacterData character = (CharacterData)GameControl.gameControl.playerList [i];
			GameControl.gameControl.partyList.Insert (i, character);
		}

		Button[] partyNextButtons = new Button[GameControl.gameControl.partySize];
		Button[] partyPrevButtons = new Button[GameControl.gameControl.partySize];

		for(int i = 0; i < GameControl.gameControl.partySize; i++)
		{
			partyButtons[i].onClick.AddListener (() => changeMenuCanvas (1));

			partyNextButtons[i] =Instantiate (Resources.Load<Button> ("UI/NextCharacterButton"));
			partyNextButtons[i].transform.SetParent (GameObject.Find ("Party_" + (i + 1)).transform);
			partyNextButtons[i].transform.localScale = new Vector3 (1, 1, 1);

			partyPrevButtons[i] =Instantiate (Resources.Load<Button> ("UI/PreviousCharacterButton"));
			partyPrevButtons[i].transform.SetParent (GameObject.Find ("Party_" + (i + 1)).transform);
			partyPrevButtons[i].transform.localScale = new Vector3 (1, 1, 1);

		}

		partyNextButtons[0].onClick.AddListener (() => PartySelect (0, true));
		partyNextButtons[1].onClick.AddListener (() => PartySelect (1, true));
		partyPrevButtons[0].onClick.AddListener (() => PartySelect (0, false));
		partyPrevButtons[1].onClick.AddListener (() => PartySelect (1, false));

		RefreshPartySelection ();

	}

	public void ClosePreBattle(){
		preBattleCanvas.SetActive (false);
	}
		

	public void PartySelect(int partySlot, bool next){
		int nextIndex;
		int previousIndex;

		CharacterData partyCharacter = (CharacterData)GameControl.gameControl.partyList [partySlot];
		int currentIndex = partyCharacter.characterIndex;

		GameControl.gameControl.partyList [partySlot] = null;

		if (currentIndex + 1 >= GameControl.gameControl.playerList.Count) {
			nextIndex = 0;
		} else {
			nextIndex = currentIndex + 1;
		}
		if (currentIndex - 1 < 0) {
			previousIndex = GameControl.gameControl.playerList.Count - 1;
		} else {
			previousIndex = currentIndex - 1;
		}


		if (next) {
			CharacterData nextCharacter = (CharacterData)GameControl.gameControl.playerList [nextIndex];
			//If next character already belongs to party, skip
			if (GameControl.gameControl.partyList.Contains (nextCharacter)) {
				nextIndex++;
				if (nextIndex >= GameControl.gameControl.playerList.Count)
					nextIndex = 0;
			}
			GameControl.gameControl.partyList [partySlot] = GameControl.gameControl.playerList [nextIndex];
		} else {
			CharacterData prevCharacter = (CharacterData)GameControl.gameControl.playerList [previousIndex];
			if (GameControl.gameControl.partyList.Contains (prevCharacter)) {
				previousIndex--;
				if (previousIndex < 0)
					previousIndex = GameControl.gameControl.playerList.Count - 1;
			}
			GameControl.gameControl.partyList [partySlot] = GameControl.gameControl.playerList [previousIndex];
		}


		RefreshPartySelection ();
	}


	public void RefreshPartySelection(){
		for(int i = 0; i < GameControl.gameControl.partySize; i++) {
			CharacterData character = (CharacterData)GameControl.gameControl.partyList[i];
			partyButtons [i].GetComponentInChildren<Text> ().text = character.characterName;
			partyButtons [i].onClick.AddListener (() => CharacterMenuSelectCharacter (character.characterIndex));
		}
	}

	////////////////////////////////////
	/// 
	/// MENU
	/// 
	////////////////////////////////////

	public void SetupMenuCanvas(){
		
		SetupCharacterMenuCanvas ();

	}

	void SetupCharacterMenuCanvas(){

		//denyEquipPanel.SetActive (false);
		//INVENTORY

		//GenerateInventory (inventoryPanel);
		GenerateInventory (equipmentPanel);

		//SKILL TREES
		fireblastButton.onClick.AddListener (() => EquipSkill (0, 0));
	}

	void GenerateInventory(GameObject inventory){
		slots = new ArrayList();

		int inventorySlotsHor = 7;
		int inventorySlotsVer = 4;

		for (int j = 0; j < inventorySlotsVer; j++) {
			for (int i = 0; i < inventorySlotsHor; i++) {
				inventorySlot = Instantiate (inventorySlot);
				inventorySlot.name = "slot_" + j.ToString() + "_" + i.ToString();
				inventorySlot.transform.SetParent (inventory.transform, false);
				inventorySlot.transform.localScale = new Vector3 (0.5f, 0.5f, 0);
				inventorySlot.transform.position = inventory.transform.position - new Vector3 (140 - 45 * i, -70 + 45 * j, 0);
				inventorySlot.GetComponent<Image> ().overrideSprite = null;
				slots.Add (inventorySlot);
			}
		}

		if (GameControl.gameControl.items.inventory != null) {
			for (int i = 0; i < GameControl.gameControl.items.inventory.Length; i++) {
				Item checkitem = GameControl.gameControl.items.inventory [i];
				if (checkitem != null) {
					AddToInventory (checkitem, i);
					break;
				}
			}
		}
	}


	public void changeMenuCanvas(int index)
	{
		switch (index) {
		case 1:
			inventoryMenuCanvas.gameObject.SetActive (false);
			characterMenuCanvas.gameObject.SetActive (true);
			characterMenuCanvas.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			CharacterMenuSelectCharacter (0);
			characterMenuOpen = true;
			menuScreenPanel.SetActive (false);

			break;
		case 2:
			characterMenuCanvas.gameObject.SetActive (false);
			inventoryMenuCanvas.gameObject.SetActive (true);
			inventoryMenuCanvas.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			break;
		}
	}

	public void CharacterMenuSelectCharacter(int index)
	{
		CharacterData stats = GameControl.gameControl.GetStats (index);
		UpdateSlots (stats);

		//Handle next/previous character buttons
		nextCharacterButton.onClick.RemoveAllListeners ();
		previousCharacterButton.onClick.RemoveAllListeners ();

		int nextIndex;
		int previousIndex;

		if (selectedCharacterInMenu + 1 >= GameControl.gameControl.playerList.Count) {
			nextIndex = 0;
		} else {
			nextIndex = selectedCharacterInMenu + 1;
		}
		if (selectedCharacterInMenu - 1 < 0) {
			previousIndex = GameControl.gameControl.playerList.Count - 1;
		} else {
			previousIndex = selectedCharacterInMenu - 1;
		}

		nextCharacterButton.onClick.AddListener (() => CharacterMenuSelectCharacter (nextIndex));
		previousCharacterButton.onClick.AddListener (() => CharacterMenuSelectCharacter (previousIndex));
	}

	/////////////////////
	/// 
	//SKILL TREE HANDLER
	///
	//////////////////////// 

	public void EquipSkill(int skillSetIndex, int skillIndex)
	{
		CharacterData character = (CharacterData)GameControl.gameControl.playerList [selectedCharacterInMenu];
		character.skillSet [skillSetIndex] = (Skill)GameControl.gameControl.skillLib.allSkills [skillIndex];
		skillSlots[skillSetIndex].onClick.AddListener (() => UnEquipSkill (0, 0));
		UpdateSlots (character);
	}

	public void UnEquipSkill(int skillSetIndex, int skillIndex)
	{
		CharacterData character = (CharacterData)GameControl.gameControl.playerList [selectedCharacterInMenu];
		character.skillSet [skillSetIndex] = null;
		UpdateSlots (character);
	}

	/////////////////////
	/// 
	//INVENTORY AND EQUIP HANDLER
	///
	//////////////////////// 

	public void RefreshEquip(CharacterData character){
		CharacterData stats = GameControl.gameControl.GetStats (character.characterIndex);
		selectedCharacterInMenu = stats.characterIndex;
		characterNameText.text = stats.characterName;
		characterImage.sprite = Resources.Load<Sprite>("characterImage_" + stats.characterIndex);

		HPText.text = "HP " + stats.HP + "/" + stats.maxHP;
		ATKText.text = "ATK " + stats.ATK;
		levelText.text = "Level " + stats.level;
	}


	public void UpdateSlots(CharacterData character){

		Button targetButton = weaponButton;
		Item item = character.weapon;

		//if (character.equipment != null) {
			for (int i = 0; i < character.equipment.Length; i++) {
			
				if (i == 0) {
					targetButton = weaponButton;
					item = character.weapon;

				} else if (i == 1) {
					targetButton = ringButton;
					item = character.ring;
				}

				if (item != null) {
					targetButton.GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (item.spritePath);
					targetButton.onClick.RemoveAllListeners ();

				} else {
					targetButton.GetComponent<Image> ().overrideSprite = null;
					targetButton.onClick.RemoveAllListeners ();
				}


				if (item != null && item.type == "Weapon")
					weaponButton.onClick.AddListener (() => Unequip ("Weapon", character));
				if (item != null && item.type == "Ring")
					ringButton.onClick.AddListener (() => Unequip ("Ring", character));
			//}
		}
		RefreshEquip (character);

		for (int i = 0; i < skillSlots.Length; i++) {
			if (character.skillSet [i] != null) {
				skillSlots [i].GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (character.skillSet [i].spritePath);
			} else {
				skillSlots [i].GetComponent<Image> ().overrideSprite = null;
			}
		}
	}


	public void Unequip(string slot, CharacterData character){
		if (slot == "Weapon") {
			weaponButton.GetComponent<Image> ().overrideSprite = null;
			weaponButton.onClick.RemoveAllListeners ();
			GameControl.gameControl.items.Unequip (character.weapon, character.characterIndex);
		}
		else if (slot == "Ring") {
			ringButton.GetComponent<Image> ().overrideSprite = null;
			ringButton.onClick.RemoveAllListeners ();
			GameControl.gameControl.items.Unequip (character.ring, character.characterIndex);
		}
	}

	public void RemoveFromInventory(int index){
		Button slot = (Button)slots [index];
		slot.GetComponent<Image> ().overrideSprite = null;
		slot.onClick.RemoveAllListeners ();
	}

	public void AddToInventory(Item item, int index){
		Button slot = (Button)slots[index];
		slot.GetComponent<Image> ().overrideSprite = Resources.Load<Sprite>(item.spritePath);
		slot.onClick.AddListener (() => GameControl.gameControl.items.Equip (item, selectedCharacterInMenu, index));
	}


	////////////////////////////////////
	/// 
	/// BATTLE SCENE
	/// 
	////////////////////////////////////
	public void SetupBattleScene(){
		menuScreenPanel.SetActive (true);
		//denyEquipPanel.SetActive (true);
		moveButton.onClick.AddListener (() => GameControl.gameControl.phase.SetPhase("Moving Phase"));
		attackButton.onClick.AddListener (() => GameControl.gameControl.phase.SetPhase("Attack Phase"));
		//skillButton.onClick.AddListener (() => GameControl.gameControl.phase.SetPhase ("Special Phase"));
		skillButton.onClick.AddListener (() => ToggleSkillPanel());

		if (victoryScreen.activeSelf) {
			victoryScreen.SetActive (false);
		}

		HPSlider = Resources.Load ("HPSlider") as GameObject;
		MPSlider = Resources.Load ("MPSlider") as GameObject;
		HP_MP_Bars = Resources.Load ("HP_MP_Bars") as GameObject;

		//Setting up battle menu canvas

		menuScreenPanel.SetActive (false);
		characterActionPanel.SetActive (false);
		skillPanel.SetActive (false);
		preBattleCanvas.SetActive (false);

		skillButtons = new Button[5];

		skillButtons [0] = aoeDButton;
		skillButtons [1] = aoePButton;
		skillButtons [2] = singleDButton;
		skillButtons [3] = singlePButton;
		skillButtons [4] = charSpecButton;

	}


	public void ToggleCharacterActionPanel(bool toggled, GameObject targetPlayer){
		if (targetPlayer != null) {
			characterActionPanel.transform.SetAsLastSibling();
			stats = targetPlayer.GetComponent<CharacterStats> ();
			characterActionPanel.transform.position = targetPlayer.transform.position + new Vector3(2f, 4f, 0);

			moveButton.interactable = stats.canStillMove;
			attackButton.interactable = stats.canStillAttack;
			skillButton.interactable = stats.canStillAttack;

			GrayOutDisabledCAPButtons (moveButton, moveButton.interactable);
			GrayOutDisabledCAPButtons (attackButton, attackButton.interactable);
			GrayOutDisabledCAPButtons (skillButton, attackButton.interactable);
		}
		characterActionPanel.SetActive(toggled);
		skillPanel.SetActive (false);

		if (characterActionPanel.activeSelf) {
			UpdateSkillPanel ();
		}
	}

	public void ToggleSkillPanel(){
		skillPanel.SetActive (!skillPanel.activeSelf);
	}

	public void UpdateSkillPanel()
	{
		CharacterData character = (CharacterData)GameControl.gameControl.playerList [GameControl.gameControl.phase.selectedPlayerCharacter.GetComponent<CharacterStats>().playerCharacterIndex];
		for (int i = 0; i < skillButtons.Length; i++) {
			if (character.skillSet [i] != null) {
				skillButtons [i].GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (character.skillSet [i].spritePath);
			} else {
				skillButtons [i].GetComponent<Image> ().overrideSprite = null;
			}
		}
	}

	public void GrayOutDisabledCAPButtons(Button button, bool enabled){
		cb = button.colors;
		Color normal;
		if (enabled) normal = Color.white;
		else  normal = Color.gray;
		cb.normalColor = normal;
	}

	public void ToggleTurnPanel(bool toggled, bool playersTurn){
		if (playersTurn) turnText.text = "Player's Turn";
		else turnText.text = "Enemy's Turn";

		turnPanel.SetActive (toggled);
	}

	public void ToggleEndTurnButton(bool toggled){
		endTurnButton.gameObject.SetActive (toggled);
	}

	public void CreateHPBar(GameObject target){
		HPSlider = Instantiate (HPSlider);
		HPSlider.transform.SetParent (battleWorldCanvas.transform, false);
		HPSlider.GetComponent<HPSliderHandler> ().SetTarget (target);
		HPSlider.name = target.name + " HPSlider";
	}

	public void CreateHP_MPBar(GameObject target){
		HP_MP_Bars = Instantiate (HP_MP_Bars);
		HP_MP_Bars.transform.SetParent (battleWorldCanvas.transform, false);
		HP_MP_Bars.GetComponent<HPMPSliderHandler> ().SetTarget (target);
		HP_MP_Bars.name = target.name + " HP/MP Bars";
	}

	public void ProcessVictoryScreen(){
		victoryScreen.SetActive (true);
	}


	////////////////////////////////////
	/// 
	/// DIALOGUE
	/// 
	////////////////////////////////////

	public void UpdateDialogue(string speaker, string text, bool isChoice)
	{
		dialogueSpeakerText.text = speaker;
		dialogueText.text = text;

		if (isChoice) {
			dialogueContinueButton.gameObject.SetActive (false);

			for(int i = 0; i < dialogueChoiceButtons.Length; i++){
				dialogueChoiceButtons [i].gameObject.SetActive (true);
				dialogueChoiceButtons [i].GetComponentInChildren<Text> ().text = GameControl.gameControl.dialogue.choices[i].ToString();
				dialogueChoiceButtons [i].onClick.AddListener (() => GameControl.gameControl.dialogue.DialogueChoose(i));
			}

		} else {
			dialogueContinueButton.gameObject.SetActive (true);
			dialogueChoiceButtons[0].gameObject.SetActive (false);
			dialogueChoiceButtons[1].gameObject.SetActive (false);
		}
	}

	public void ForceOffDialogueChoices(){
		dialogueChoiceButtons[0].gameObject.SetActive (false);
		dialogueChoiceButtons[1].gameObject.SetActive (false);
	}

	public void ToggleDialogue(bool toggled){
		dialogueCanvas.gameObject.SetActive (toggled);
	}

	////////////////////////////////////
	/// 
	/// OTHER
	/// 
	////////////////////////////////////

	public void toggleLoadingScreen(bool toggled){
		loadingScreen.SetActive (toggled);
	}

}
