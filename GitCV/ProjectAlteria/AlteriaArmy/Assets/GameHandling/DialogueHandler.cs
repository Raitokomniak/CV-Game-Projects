using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class DialogueHandler : MonoBehaviour {

	public TextAsset dialogueText;
	public ArrayList lines;
	public ArrayList chains;
	public ArrayList lineList;
	public ArrayList choices;

	string speaker;
	string text;
	public bool dialogueSceneRunning;
	int localStoryProgression;
	int lineIndex;
	bool endOfDialogueChain;

	// Use this for initialization
	void Awake () {
		lines = new ArrayList();
		chains = new ArrayList ();
		lineList = new ArrayList ();
		choices = new ArrayList ();

		LoadAllDialogue ();
		localStoryProgression = -1;
		lineIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadAllDialogue(){
		char delimiterChar = '=';

		chains.AddRange(dialogueText.text.Split(delimiterChar));
	}

	public void GetDialogue(int index){

		lineList.Clear ();
		lineList.InsertRange(0, chains[localStoryProgression].ToString().Split("\n" [0]));
		FilterEmptyLines ();

		ArrayList parsedLines = new ArrayList ();
		parsedLines.AddRange (lineList [lineIndex].ToString ().Split ("\t" [0]));

		bool isChoice = CheckLineForChoice (parsedLines);

		if (!isChoice) {
			speaker = parsedLines [0].ToString ();
			text = parsedLines [1].ToString ();
		}

		GameControl.gameControl.ui.UpdateDialogue (speaker, text, isChoice);

		lineIndex++;

		if (lineIndex + 2 > lineList.Count) {
			endOfDialogueChain = true;
			lineIndex = 0;
		}

	}

	bool CheckLineForChoice(ArrayList lines)
	{
		choices.Clear ();

		if (lines [0].ToString () == "Choice>") {
			choices.Add (lines [1]);
			choices.Add (lines [3]);
			return true;
		} else {
			return false;
		}
	}

	public void DialogueChoose(int index)
	{
		if (index == 0) {
			lineIndex += 0;
			CheckForDialogue ();
			lineIndex += 1;
		}
		if (index == 1) {
			lineIndex += 1;
			CheckForDialogue ();
		}

		GameControl.gameControl.ui.ForceOffDialogueChoices ();
	}


	public void FilterEmptyLines(){
		int removeEmpty = 0;
		foreach(string line in lineList)
		{
			if (line == " " && line == "") removeEmpty = lineList.IndexOf(line);
		}

		lineList.RemoveAt(removeEmpty);
	}

	public void CheckForDialogue(){
		if (!endOfDialogueChain) {
			localStoryProgression = GameControl.gameControl.storyProgression;
			GetDialogue (localStoryProgression);
			GameControl.gameControl.ui.ToggleDialogue (true);
			dialogueSceneRunning = true;
		} else {
			GameControl.gameControl.ui.ToggleDialogue (false);
			dialogueSceneRunning = false;
			endOfDialogueChain = false;
		}
	}
}
