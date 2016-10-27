using UnityEngine;
using System.Collections;

public class PhaseController : MonoBehaviour {

	AccessibleArea AAD;

	public GameObject grid;
	public GameObject selectedPlayerCharacter;

	public int specialPhase;
	//PHASES
	//
	// Moving Phase
	// Attack Phase
	// Aggro Phase
	// Special Phase 0-4

	string phase;


	public void Setup(){
		grid = GameObject.Find ("Grid");
		AAD = grid.GetComponent<AccessibleArea> ();

		phase = "Default";
	}

	public void SetSelectedPlayerCharacter(GameObject givenCharacter)
	{
		selectedPlayerCharacter = givenCharacter;
	}

	public void SetEnemyPhase(string givenPhase, GameObject target)
	{
		phase = givenPhase;
		AAD.DetermineArea (target);
	}

	public void SetPhase(string givenPhase)
	{
		ResetPhase ();

		phase = givenPhase;
		AAD.DetermineArea (selectedPlayerCharacter);
	}

	public void SetSpecialPhase(int index)
	{
		ResetPhase ();
		phase = "Special Phase";
		specialPhase = index;
		AAD.DetermineArea (selectedPlayerCharacter);
	}

	public void SetSpecialAADPreview(int skillSetIndex)
	{
		if (selectedPlayerCharacter.GetComponent<CharacterStats> ().skillSet [skillSetIndex] != null) {
			phase = "Special Phase";
			specialPhase = skillSetIndex;

			AAD.DetermineArea (selectedPlayerCharacter);

			GameControl.gameControl.ui.skillButtons [skillSetIndex].onClick.RemoveAllListeners ();
			GameControl.gameControl.ui.skillButtons [skillSetIndex].onClick.AddListener (() => selectedPlayerCharacter.GetComponent<CharacterSkill> ().SpecialAttack (skillSetIndex));
		}
	}
		

	public string GetPhase()
	{
		return phase;
	}

	public void ResetPhase()
	{
		grid = GameObject.Find ("Grid");
		AAD = grid.GetComponent<AccessibleArea> ();
		AAD.TurnOff ();
		GameControl.gameControl.ui.ToggleCharacterActionPanel (false, selectedPlayerCharacter);
		phase = "Default";
	}

	public void SoftResetPhase(){
		if (specialPhase != 2) {
			AAD.TurnOff ();
			phase = "Default";
		}
	}

}
