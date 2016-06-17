using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUISetup : MonoBehaviour {

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

	public void GivePlayerTexts( NetworkPlayerController playerController )
	{
		//Give player bar controllers
		playerController.healthBarController = healthBarController;
		playerController.staminaBarController = staminaBarController;
		playerController.magicBarController = magicBarController;

		//Give player all text components
		playerController.wigsText = wigsText;
		playerController.karmaText = wigsText;
		playerController.hpText = hpText;
		playerController.staminaText = staminaText;
		playerController.magicText = magicText;
	}
}
