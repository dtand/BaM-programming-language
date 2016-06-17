using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Encapsulates weapon stats
[System.Serializable]
public class Weapon
{
	string name;
	int damageRange;
	int durability;
	int damage;
}


//Encapsulates character stats
[System.Serializable]
public class Character
{
	string name;

	public int health;
	public int totalHealth;

	public int stamina;
	public int totalStamina;

	public int magic;
	public int totalMagic;

	public int karma;

	public int wigs;

	public Character()
	{
		health = 100;
		totalHealth = 100;
		magic = 100;
		totalMagic = 100;
		stamina = 100;
		totalStamina = 100;
		karma = 0;
		wigs = 0;
	}


}

public class GameController : MonoBehaviour {

	public GameObject mainCharacter;
	public Movement characterMovement;

	public Character characterStats;

	//Dynamic bars
	public BarController healthBarController;
	public BarController staminaBarController;
	public BarController magicBarController;

	//Text components [ dynamic ]
	public Text wigsText;
	public Text karmaText;
	public Text hpText;
	public Text staminaText;
	public Text magicText;

	void Start()
	{
		characterMovement = mainCharacter.GetComponent< Movement >( );
		characterStats = new Character( );
		AlterKarma( characterStats.karma );
		AlterWigs( characterStats.wigs );
	}


	//== Functions to alter character stats, they always return the
	//== stat after altering it
	public int AlterCharacterHealth( int change )
	{
		string ending = " /" + characterStats.totalHealth.ToString( );
		characterStats.health += change;
		string newString = characterStats.health.ToString( ) + ending;
		hpText.text = newString;
		healthBarController.UpdateBarWidth( (float)characterStats.health / (float)characterStats.totalHealth );
		return characterStats.health;
	}

	public int AlterWigs( int change )
	{
		characterStats.wigs += change;
		wigsText.text = characterStats.wigs.ToString( );
		return characterStats.wigs;
	}

	public int AlterKarma( int change )
	{
		characterStats.karma += change;
		karmaText.text = characterStats.karma.ToString( );
		return characterStats.karma;
	}

	public int AlterStamina( int change )
	{
		string ending = " /" + characterStats.totalStamina.ToString( );
		characterStats.stamina += change;
		string newString = characterStats.stamina.ToString( ) + ending;
		staminaText.text = newString;
		staminaBarController.UpdateBarWidth( (float)characterStats.stamina / (float)characterStats.totalStamina );
		return characterStats.stamina;
	}
	
	public int AlterMagic( int change )
	{
		string ending = " /" + characterStats.totalMagic.ToString( );
		characterStats.magic += change;
		string newString = characterStats.magic.ToString( ) + ending;
		magicText.text = newString;
		magicBarController.UpdateBarWidth( (float)characterStats.magic / (float)characterStats.totalMagic );
		return characterStats.stamina;
	}

	public bool IsFull( string stat )
	{
		if( stat == "health" )
		{
			return characterStats.health == characterStats.totalHealth;
		}
		else if( stat == "stamina" )
		{
			return characterStats.stamina == characterStats.totalStamina;
		}
		else if( stat == "magic" )
		{
			return characterStats.magic == characterStats.totalMagic;
		}

		return false;
	}

	public void Equalize( string stat )
	{
		if( stat == "health" )
		{
			if ( characterStats.health > characterStats.totalHealth )
			{
				characterStats.health = characterStats.totalHealth;
			}
		}
		else if( stat == "magic" )
		{
			if ( characterStats.magic > characterStats.totalMagic )
			{
				characterStats.magic = characterStats.totalMagic;
			}
		}
		else if( stat == "stamina" )
		{
			if ( characterStats.stamina > characterStats.totalStamina)
			{
				characterStats.stamina = characterStats.totalStamina;
			}
		}
	}


	

}
