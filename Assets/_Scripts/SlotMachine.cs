using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Linq;

using System.Collections.Generic;

public class SlotMachine : MonoBehaviour {


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
    

    // Logic to run before the start of the game
    void Start () {
       
    }
	
    // adding and attaching the GUI components and class properties
    void OnGUI()
    {
        // Creating the three picture box structures holding the images
        _PictureBoxes();
        // Setting the value of Total credits = playerMoney
        // also attaching it to this GUI componenent making it reactive, i.e., any changes to the property value will automatically be updated on the view
        GameObject.Find("TotalCredits").GetComponent<Text>().text = "$" + playerMoney;

        // Displaying the bet amount set
        GameObject.Find("BetAmount").GetComponent<Text>().text = "$" + playerBet;
    }
    
    // Creating the three picture holding boxes showing the spin result
    void _PictureBoxes()
    {
        // First box
        GUI.Box(new Rect(Screen.width - 740, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[0] == null) ? "banana" : fruitsToDisplay[0]) as Texture2D);

        // Second box
        GUI.Box(new Rect(Screen.width - 575, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[1] == null) ? "cherry" : fruitsToDisplay[1]) as Texture2D);

        // Third box ( Right most)
        GUI.Box(new Rect(Screen.width - 410, Screen.height - 400, 125, 105), Resources.Load((fruitsToDisplay[2] == null) ? "grapes" : fruitsToDisplay[2]) as Texture2D);
    }

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
        EditorUtility.DisplayDialog("Player Stats", "Statistics from last play \n" + stats, "OK");
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
		string[] spinResult = { " ", " ", " " };

		for (var spin = 0; spin < 3; spin++)
		{
			int randomNumber = Random.Range(1,65);

			if (checkRange(randomNumber, 1, 27)) {  // 41.5% probability
				spinResult[spin] = "blank";
			}
			else if (checkRange(randomNumber, 28, 37)){ // 15.4% probability
				spinResult[spin] = "Grapes";
			}
			else if (checkRange(randomNumber, 38, 46)){ // 13.8% probability
				spinResult[spin] = "Banana";
			}
			else if (checkRange(randomNumber, 47, 54)){ // 12.3% probability
				spinResult[spin] = "Orange";
			}
			else if (checkRange(randomNumber, 55, 59)){ //  7.7% probability
				spinResult[spin] = "Cherry";
			}
			else if (checkRange(randomNumber, 60, 62)){ //  4.6% probability
				spinResult[spin] = "Bar";
			}
			else if (checkRange(randomNumber, 63, 64)){ //  3.1% probability
				spinResult[spin] = "Bell";
			}
			else if (checkRange(randomNumber, 65, 65)){ //  1.5% probability
				spinResult[spin] = "Seven";
			}

		}
        this.fruitsToDisplay = spinResult;
		return spinResult;
	}

	/* This function calculates the player's winnings, if any */
	private void _determineResult(string[] spinResult)
	{
        // Dictionary to contain the fruits that came on spin and their frequency in this spin
        Dictionary<string, int> SpinResults = new Dictionary<string, int>();

        // Pass the spin result data from the spinResult to the SpinResults dictionary
        foreach (string item in spinResult)
        {
            // If the current item has already been added to the dictionary
            if (SpinResults.ContainsKey(item)) {
                // increment it's value,i.e., indicating the number the times it appeared in the spinResult
                SpinResults[item]++;
            }
            else
            {
                // Add the fruit into the fruit dictionary with initial value of 1
                SpinResults.Add(item, 1);
            }
        }

        // If the SpinResults contain a blank
        if (SpinResults.ContainsKey("blank"))
        {
            lossNumber++;
            showLossMessage();
        }
        else
        {
            // factor by which the playerBet will be multiplied
            int factor = 0;

            if (SpinResults.ContainsValue(3) && SpinResults.Count == 1)
            {
                // Get the item (key) with value of 3
                string item = SpinResults.FirstOrDefault(x => x.Value == 3).Key;
                switch (item)
                {
                    case "grapes": factor = 10; break;
                    case "banana": factor = 20; break;
                    case "oranges": factor = 30; break;
                    case "cherry": factor = 40; break;
                    case "bars": factor = 50; break;
                    case "bells": factor = 75; break;
                    case "sevens": factor = 100; break;
                }
            }
            // if SpinResults has an item coming two times
            else if (SpinResults.ContainsValue(2))
            {
                // Get the item (key) with value of 2
                string item = SpinResults.FirstOrDefault(x => x.Value == 2).Key;
                switch (item)
                {
                    case "grapes": 
                    case "banana": factor = 1; break;
                    case "oranges": factor = 3; break;
                    case "cherry": factor = 4; break;
                    case "bars": factor = 5; break;
                    case "bells": factor = 10; break;
                    case "sevens": factor = 20; break;
                }
            }
            // Contains all unique items
            else
            {
                // if SpinResults contain at least one sevens then factor will be 5, else 1
                factor = (SpinResults.ContainsKey("sevens")) ?  5 : 1;
            }

            // set the winning amount
            winnings = playerBet * factor;
            // Update the win number
            winNumber++;
            // Display win message
            showWinMessage();
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
            // Call the Reel method to make the spin
			spinResult = Reels();
            // pass the spin result obtained from Reel method to _determineResult Method
            _determineResult(spinResult);
            turn++; // increment the turn number
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
