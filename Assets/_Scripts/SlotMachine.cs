using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

using System.Collections.Generic;

public class SlotMachine : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
    }
	
    // adding components on formation of GUI
    void OnGUI()
    {
        _PictureBoxes();

        GameObject.Find("TotalCredits").GetComponent<Text>().text = "$" + playerMoney;
        GameObject.Find("BetAmount").GetComponent<Text>().text = "$" + playerBet;

        
    }

 

    void _PictureBoxes()
    {
        // First box
        GUI.Box(new Rect(Screen.width - 740, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[0] == null) ? "banana" : fruitsToDisplay[0]) as Texture2D);

        // Second box
        GUI.Box(new Rect(Screen.width - 575, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[1] == null) ? "cherry" : fruitsToDisplay[1]) as Texture2D);

        // Third box ( Right most)
        GUI.Box(new Rect(Screen.width - 410, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[2] == null) ? "grapes" : fruitsToDisplay[2]) as Texture2D);
    }

    string[] fruitsToDisplay = new string[3];
    private int playerMoney = 1000;
	private int winnings = 0;
	private int jackpot = 5000;
	private float turn = 0.0f;
	private int playerBet = 10;
	private float winNumber = 0.0f;
	private float lossNumber = 0.0f;
	private string[] spinResult;
	private string fruits = "";
	private float winRatio = 0.0f;
	private float lossRatio = 0.0f;
	private int grapes = 0;
	private int bananas = 0;
	private int oranges = 0;
	private int cherries = 0;
	private int bars = 0;
	private int bells = 0;
	private int sevens = 0;
	private int blanks = 0;
    

    private Dictionary<string, int> _ScoreList = new Dictionary<string, int>();

	/* Utility function to show Player Stats */
	private void showPlayerStats()
	{
		winRatio = winNumber / turn;
		lossRatio = lossNumber / turn;
		string stats = "";
		stats += ("Jackpot: " + jackpot + "\n");
		stats += ("Player Money: " + playerMoney + "\n");
		stats += ("Turn: " + turn + "\n");
		stats += ("Wins: " + winNumber + "\n");
		stats += ("Losses: " + lossNumber + "\n");
		stats += ("Win Ratio: " + (winRatio * 100) + "%\n");
		stats += ("Loss Ratio: " + (lossRatio * 100) + "%\n");
		Debug.Log(stats);
	}

	/* Utility function to reset all fruit tallies*/
	private void resetFruitTally()
	{
		grapes = 0;
		bananas = 0;
		oranges = 0;
		cherries = 0;
		bars = 0;
		bells = 0;
		sevens = 0;
		blanks = 0;
	}

	/* Utility function to reset the player stats */
	public void resetAll()
	{
		playerMoney = 1000;
		winnings = 0;
		jackpot = 5000;
		turn = 0;
		playerBet = 10; // default amount
		winNumber = 0;
		lossNumber = 0;
		winRatio = 0.0f;
        resetFruitTally();
		GameObject.Find("SpinResult").GetComponent<Text>().text = "      Spin It!";
        GameObject.Find("WinAmount").GetComponent<Text>().text = "";
        Debug.Log("Resetted all values");
	}


	/* Utility function to show a win message and increase player money */
	private void showWinMessage()
	{
		playerMoney += winnings;
        GameObject.Find("SpinResult").GetComponent<Text>().text = "You Won: $" + winnings;
        Debug.Log("You Won: $" + winnings);
        GameObject.Find("WinAmount").GetComponent<Text>().text = "$" + winnings;
        resetFruitTally();

        /* Check to see if the player won the jackpot */

        /* compare two random values */
        var jackPotTry = Random.Range(1, 51);
        var jackPotWin = Random.Range(1, 51);
        if (jackPotTry == jackPotWin)
        {
            EditorUtility.DisplayDialog("Jackpot!", "You Won the $" + jackpot + " Jackpot!!", "OK");
            Debug.Log("You Won the $" + jackpot + " Jackpot!!");
            playerMoney += jackpot;
            jackpot = 1000;
            GameObject.Find("WinAmount").GetComponent<Text>().text = "$" + winnings + jackpot;
        } else
        {
            GameObject.Find("WinAmount").GetComponent<Text>().text = "$" + winnings;
        }
        
        // Play the winning sound effect
        GameObject.Find("WinAudio").GetComponent<AudioSource>().Play();
}

	/* Utility function to show a loss message and reduce player money */
	private void showLossMessage()
	{
		playerMoney -= playerBet;
        GameObject.Find("SpinResult").GetComponent<Text>().text = "      You Lost!";
        Debug.Log("You Lost!");
        GameObject.Find("WinAmount").GetComponent<Text>().text = "$0";
        resetFruitTally();

        // Play the loosing sound effect
        GameObject.Find("LossAudio").GetComponent<AudioSource>().Play();
	}

	/* Utility function to check if a value falls within a range of bounds */
	private bool checkRange(int value, int lowerBounds, int upperBounds)
	{
		return (value >= lowerBounds && value <= upperBounds) ? true : false;

	}

	/* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
	private string[] Reels()
	{
		string[] betLine = { " ", " ", " " };

		for (var spin = 0; spin < 3; spin++)
		{
			int randomNumber = Random.Range(1,65);

			if (checkRange(randomNumber, 1, 27)) {  // 41.5% probability
				betLine[spin] = "blank";
				blanks++;
			}
			else if (checkRange(randomNumber, 28, 37)){ // 15.4% probability
				betLine[spin] = "Grapes";
				grapes++;
			}
			else if (checkRange(randomNumber, 38, 46)){ // 13.8% probability
				betLine[spin] = "Banana";
				bananas++;
			}
			else if (checkRange(randomNumber, 47, 54)){ // 12.3% probability
				betLine[spin] = "Orange";
				oranges++;
			}
			else if (checkRange(randomNumber, 55, 59)){ //  7.7% probability
				betLine[spin] = "Cherry";
				cherries++;
			}
			else if (checkRange(randomNumber, 60, 62)){ //  4.6% probability
				betLine[spin] = "Bar";
				bars++;
			}
			else if (checkRange(randomNumber, 63, 64)){ //  3.1% probability
				betLine[spin] = "Bell";
				bells++;
			}
			else if (checkRange(randomNumber, 65, 65)){ //  1.5% probability
				betLine[spin] = "Seven";
				sevens++;
			}

		}
        this.fruitsToDisplay = betLine;
		return betLine;
	}

	/* This function calculates the player's winnings, if any */
	private void determineWinnings()
	{
		if (blanks == 0)
		{
			if (grapes == 3)
			{
				winnings = playerBet * 10;
			}
			else if (bananas == 3)
			{
				winnings = playerBet * 20;
			}
			else if (oranges == 3)
			{
				winnings = playerBet * 30;
			}
			else if (cherries == 3)
			{
				winnings = playerBet * 40;
			}
			else if (bars == 3)
			{
				winnings = playerBet * 50;
			}
			else if (bells == 3)
			{
				winnings = playerBet * 75;
			}
			else if (sevens == 3)
			{
				winnings = playerBet * 100;
			}
			else if (grapes == 2)
			{
				winnings = playerBet * 2;
			}
			else if (bananas == 2)
			{
				winnings = playerBet * 2;
			}
			else if (oranges == 2)
			{
				winnings = playerBet * 3;
			}
			else if (cherries == 2)
			{
				winnings = playerBet * 4;
			}
			else if (bars == 2)
			{
				winnings = playerBet * 5;
			}
			else if (bells == 2)
			{
				winnings = playerBet * 10;
			}
			else if (sevens == 2)
			{
				winnings = playerBet * 20;
			}
			else if (sevens == 1)
			{
				winnings = playerBet * 5;
			}
			else
			{
				winnings = playerBet * 1;
			}
			winNumber++;
			showWinMessage();
		}
		else
		{
			lossNumber++;
			showLossMessage();
		}

	}

	public void OnSpinButtonClick()
	{

		if (playerMoney == 0)
		{
            if (EditorUtility.DisplayDialog("Out of Money", "You ran out of Money! Do you want to play again?", "Yes", "No"))
            {
                resetAll();
                showPlayerStats();
            }
            
        }
		else if (playerBet > playerMoney)
		{
            EditorUtility.DisplayDialog("Not Enough Money","You don't have enough Money to place that bet.", "OK");
		}
		else if (playerBet < 0)
		{
            EditorUtility.DisplayDialog("Invalid Bet Amount","All bets must be a positive $ amount.", "OK");
		}
		else if (playerBet <= playerMoney)
		{
			spinResult = Reels();
			fruits = spinResult[0] + " - " + spinResult[1] + " - " + spinResult[2];
			Debug.Log(fruits);
			determineWinnings();
			turn++;
			showPlayerStats();
		}
		else
		{
            EditorUtility.DisplayDialog("Invalid Bet Amount","Please enter a valid bet amount", "OK");
		}
	}

    public void Bet(int betAmount)
    {
        // bet amount logic
        if (betAmount > 0)
        {
            this.playerBet = betAmount;
        }
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
