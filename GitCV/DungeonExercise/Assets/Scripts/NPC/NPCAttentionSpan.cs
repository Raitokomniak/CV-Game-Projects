using UnityEngine;
using System.Collections;

public class NPCAttentionSpan : MonoBehaviour {

	NPCController NPCC;

	void Awake()
	{
		NPCC = GetComponentInParent<NPCController>();
	}

	void OnTriggerExit()
	{
		//NPCC.toggleInteract(false);
	}
}
