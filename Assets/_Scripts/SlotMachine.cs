using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

///<summary>
/// Georgian College - Computer Programmer
/// COMP 1004 - Rapid Application Development
/// Instructor: Tom Tsiliopoulos
/// 
/// Assignment 5: A slot machine game
/// 
/// BONUS: Sound effects
/// 
/// Author Name: Pranav Kural
/// Student Number: 200333253
/// 
/// Last modified: April 14, 2017
/// 
/// Brief revision history:
/// Initial commit to add default .gitIgnore and .gitAttribute files.
/// .....
/// Added the sound effects
/// Improved the logic execution and application flow
/// 
/// </summary>

public class SlotMachine : MonoBehaviour {

    // Class variables
    string[] itemsToDisplay = new string[3];    // Items to be diplayed on slot machine after spin
    private int _playerMoney = 1000;            // Total player money; Initial $1000
    private int _winnings = 0;                  // Winning amount
    private int _jackpot = 5000;                // jackpot amount; Initial $5000
    private float _turn = 0.0f;                 // Player turns
    private int _playerBet = 10;                // Player bet amount; Initial $10
    private float _winNumber = 0.0f;            // Number of wins
    private float _lossNumber = 0.0f;           // Number of losses
   

    // Logic to run before the start of the game
    void Start () {}
	
    // adding and attaching the GUI components and class properties
    void OnGUI()
    {
        // Creating the three picture box structures holding the images
        _PictureBoxes();
        // Setting the value of Total credits = playerMoney
        // also attaching it to this GUI componenent making it reactive, i.e., any changes to the property value will automatically be updated on the view
        GameObject.Find("TotalCredits").GetComponent<Text>().text = "$" + _playerMoney;

        // Displaying the bet amount set
        GameObject.Find("BetAmount").GetComponent<Text>().text = "$" + _playerBet;
    }
    
    // Creating the three picture holding boxes showing the spin result
    void _PictureBoxes()
    {
        // First box
        GUI.Box(new Rect(Screen.width - 740, Screen.height - 400, 125, 105), Resources.Load((itemsToDisplay[0] == null) ? "banana" : itemsToDisplay[0]) as Texture2D);

        // Second box
        GUI.Box(new Rect(Screen.width - 575, Screen.height - 400, 125, 105), Resources.Load((itemsToDisplay[1] == null) ? "cherry" : itemsToDisplay[1]) as Texture2D);

        // Third box ( Right most)
        GUI.Box(new Rect(Screen.width - 410, Screen.height - 400, 125, 105), Resources.Load((itemsToDisplay[2] == null) ? "grapes" : itemsToDisplay[2]) as Texture2D);
    }

	/* Utility function to show Player Stats */
	private void showPlayerStats()
	{
         float _winRatio = 0.0f;             // Ratio of winning
         float _lossRatio = 0.0f;            // Ratio of lossing

        // Calculate stats
        _winRatio = _winNumber / _turn;
		_lossRatio = _lossNumber / _turn;
		string stats = "";
		stats += ("Jackpot: " + _jackpot + "\n");
		stats += ("Player Money: " + _playerMoney + "\n");
		stats += ("Turn: " + _turn + "\n");
		stats += ("Wins: " + _winNumber + "\n");
		stats += ("Losses: " + _lossNumber + "\n");
		stats += ("Win Ratio: " + (_winRatio * 100) + "%\n");
		stats += ("Loss Ratio: " + (_lossRatio * 100) + "%\n");

        // Display a message box with stats
        EditorUtility.DisplayDialog("Player Stats", "Statistics from last play \n" + stats, "OK");
	}

	/* Utility function to reset the player stats */
	public void resetAll()
	{
        // reset the properties to initial values
		_playerMoney = 1000;    // initial amount of $1000
		_winnings = 0;
		_jackpot = 5000;        // initial amount of $5000
        _turn = 0;
		_playerBet = 10;        // initial amount of $10
        _winNumber = 0;
		_lossNumber = 0;
        // reset the display of spin result and win amount
		GameObject.Find("SpinResult").GetComponent<Text>().text = "      Spin It!";
        GameObject.Find("WinAmount").GetComponent<Text>().text = "$0";
	}


	/* Utility function to show a win message and increase player money */
	private void showWinMessage()
	{
        // Add the win amount to player's money
		_playerMoney += _winnings;

        /* Check to see if the player won the jackpot */

        /* compare two random values */
        var jackPotTry = Random.Range(1, 51);
        var jackPotWin = Random.Range(1, 51);
        if (jackPotTry == jackPotWin)
        {
            // Display the jackpot winning message
            EditorUtility.DisplayDialog("Jackpot!", "You Won the $" + _jackpot + " Jackpot!!", "OK");
            _playerMoney += _jackpot;
            _jackpot = 1000; // jackpot amount after one jackpot has been won
            // Display the win amount + jackpot
            GameObject.Find("WinAmount").GetComponent<Text>().text = "$" + _winnings + _jackpot;
            GameObject.Find("SpinResult").GetComponent<Text>().text = "You Won: $" + _winnings + _jackpot;
        } else
        {
            // Dsiplay the win amount
            GameObject.Find("WinAmount").GetComponent<Text>().text = "$" + _winnings;
            GameObject.Find("SpinResult").GetComponent<Text>().text = "You Won: $" + _winnings;
        }
        
        // Play the winning sound effect
        GameObject.Find("WinAudio").GetComponent<AudioSource>().Play();
}

	/* Utility function to show a loss message and reduce player money */
	private void showLossMessage()
	{
        // reduce the player money by the bet amount
		_playerMoney -= _playerBet;
        // Display the loss message
        GameObject.Find("SpinResult").GetComponent<Text>().text = "      You Lost!";
        // Clear the win amount
        GameObject.Find("WinAmount").GetComponent<Text>().text = "$0";

        // Play the loosing sound effect
        GameObject.Find("LossAudio").GetComponent<AudioSource>().Play();
	}

	// Method to determine the betLine results
    // e.g. Bar - Orange - Banana */
	private string[] Reels()
	{
        // array to store the spin results
		string[] spinResult = { " ", " ", " " };

        // Spin 3 times
		for (var spin = 0; spin < 3; spin++)
		{
			int randomNumber = Random.Range(1,65);

			if (randomNumber <= 27) {  // 41.5% probability
				spinResult[spin] = "blank";
			}
			else if (randomNumber >= 28 && randomNumber <= 37) { // 15.4% probability
				spinResult[spin] = "Grapes";
			}
			else if (randomNumber >= 38 && randomNumber <= 46){ // 13.8% probability
				spinResult[spin] = "Banana";
			}
			else if (randomNumber >= 47 && randomNumber <= 54){ // 12.3% probability
				spinResult[spin] = "Orange";
			}
			else if (randomNumber >= 55 && randomNumber <= 59){ //  7.7% probability
				spinResult[spin] = "Cherry";
			}
			else if (randomNumber >= 60 && randomNumber <= 62){ //  4.6% probability
				spinResult[spin] = "Bar";
			}
			else if (randomNumber >= 63 && randomNumber <= 64){ //  3.1% probability
				spinResult[spin] = "Bell";
			}
			else if (randomNumber == 65) { //  1.5% probability
				spinResult[spin] = "Seven";
			}
		}
        // Set the spin results to the fruitsToDisplay, which will automatically update the view
        this.itemsToDisplay = spinResult;
        // return the spin Results
		return spinResult;
	}

	// Determine if the user Won or Lost and calculate winnings if any
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
            // increment the number of losses
            _lossNumber++;
            // display the loss message
            showLossMessage();
        }
        else
        {
            // factor by which the playerBet will be multiplied
            int factor = 1;

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
            _winnings = _playerBet * factor;
            // Update the win number
            _winNumber++;
            // Display win message
            showWinMessage();
        }
	}

    // Spin Button click event handler
	public void OnSpinButtonClick()
	{
        // if player has no money left
		if (_playerMoney == 0)
		{
            // Display a message to inform the player he ran out of money
            if (EditorUtility.DisplayDialog("Out of Money", "You ran out of Money! Do you want to play again?", "Yes", "No"))
            {
                // Reset the game if user chose yes
                resetAll(); // resets the playerMoney too

                // Display the player the stats of last play
                showPlayerStats();
            }
            
        }
		else if (_playerBet > _playerMoney)
		{
            // Display a message for insufficient balance to play the bet
            EditorUtility.DisplayDialog("Not Enough Money","You don't have enough Money to place that bet.", "OK");
		}
		else if (_playerBet <= _playerMoney)
		{
            // Call the Reel method to make the spin
			string[] _spinResult = Reels();
            // pass the spin result obtained from Reel method to _determineResult Method
            _determineResult(_spinResult);
            _turn++; // increment the turn number
		}
	}

    // Set the player bet amount
    public void Bet(int betAmount)
    {
        // if the bet amount is valid and greater than zero
        if (betAmount > 0)
        {
            // set the new player amount
            this._playerBet = betAmount;
            // This automatically updates the amount shown on view
            // don't have to be set again to the GUI component
        }
    }

    // Quit button click event handler
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
