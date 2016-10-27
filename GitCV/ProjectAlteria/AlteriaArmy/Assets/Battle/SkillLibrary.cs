using UnityEngine;
using System.Collections;


public abstract class Skill {
	public string type;
	public string spritePath;
	public int index;
	public string name;
	public int manaCost;
	public string effect;
	public bool isSelf;

	public void SetType(string t){
		type = t;
	}

	void Awake(){
		
	}

}

public class AoEDamage : Skill {
	int damage;

	public void SetDamage(int d) {
		damage = d;
	}

	public int GetDamage(){
		return damage;
	}
}

public class SingleDamage : Skill {
	int damage;

	public void SetDamage(int d) {
		damage = d;
	}

	public int GetDamage(){
		return damage;
	}
}

public class AoEPositive : Skill {
	public string affectedStat;
	public int amount;

	public void SetAffectedStat(string stat, int a)
	{
		affectedStat = stat;
		amount = a;
	}

	public void GetAffectedStat(){

	}
}

public class SinglePositive : Skill {
	public string affectedStat;
	public int amount;

	public void SetAffectedStat(string stat, int a)
	{
		affectedStat = stat;
		amount = a;
	}
}

public class CharacterSpecial : Skill {
	public string affectedStat;
	public int amount;

	public void SetAffectedStat(string stat, int a)
	{
		affectedStat = stat;
		amount = a;
	}
}


public class SkillLibrary : MonoBehaviour {
	public ArrayList allSkills;

	public AoEDamage fireBlast = new AoEDamage ();

	public AoEPositive moraleBoost = new AoEPositive();

	public SingleDamage iceStrike = new SingleDamage();

	public SinglePositive attackBoost = new SinglePositive();

	public SinglePositive phoenixFire = new SinglePositive();

	//public CharacterSpecial phoenixFire = new CharacterSpecial();


	public void CreateSkills(){
		allSkills = new ArrayList ();

		fireBlast.SetDamage (10);
		fireBlast.name = "Fireblast";
		fireBlast.type = "AoED";
		fireBlast.index = 1;
		fireBlast.manaCost = 30;
		fireBlast.effect = "Damage";

		iceStrike.SetDamage (20);
		iceStrike.name = "Ice Strike";
		iceStrike.type = "SingleD";
		iceStrike.index = 1;
		iceStrike.manaCost = 20;
		iceStrike.effect = "Damage";

		moraleBoost.name = "Morale Boost";
		moraleBoost.type = "AoEP";
		moraleBoost.index = 1;
		moraleBoost.manaCost = 20;
		moraleBoost.effect = "Buff";
		moraleBoost.SetAffectedStat ("ATK", 5);

		attackBoost.name = "Attack Boost";
		attackBoost.type = "SingleP";
		attackBoost.index = 1;
		attackBoost.manaCost = 10;
		attackBoost.effect = "Buff";

		phoenixFire.name = "PhoenixFire";
		phoenixFire.type = "CharSpec";
		phoenixFire.index = 1;
		phoenixFire.manaCost = 20;
		phoenixFire.effect = "Buff";
		phoenixFire.SetAffectedStat ("HP", 10);
		phoenixFire.isSelf = true;

		allSkills.Add (fireBlast);
		allSkills.Add (moraleBoost);
		allSkills.Add (iceStrike);
		allSkills.Add (attackBoost);
		allSkills.Add (phoenixFire);

		foreach (Skill skill in allSkills) {
			skill.spritePath = "UI/Icons/Skills/Skill_" + skill.type + "_" + skill.index;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
	public virtual int GetDamage(){
		return 0;
	}
}


