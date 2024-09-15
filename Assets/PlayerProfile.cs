using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile Instance { get; set; }
    public TMP_InputField nameInputField; // For player name

    public Button saveButton;

    // public Button loadButton;

    private string filePath;
    private List<PlayerData> playerDataList = new(); // List to store multiple player data entries

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.csv";
        if (saveButton)
            saveButton.onClick.AddListener(SavePlayerDataToFile);
        // loadButton.onClick.AddListener(LoadPlayerDataFromFile);
    }

    // Save the entered data to a file
    public void SavePlayerDataToFile()
    {
        string playerName = nameInputField.text;
        string playerScore = GameManager.Instance.currentAmountDisplayer.text; // Convert score input to integer
        string salad_name = GameManager.Instance.currentSalad.salad_name;

        if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(salad_name))
        {
            // Create a new PlayerData object
            PlayerData newPlayerData = new PlayerData(playerName, playerScore, salad_name);

            // Append the new player data to the file
            File.AppendAllText(filePath, newPlayerData.ToString() + "\n");

            Debug.Log("Data saved: " + newPlayerData);
            saveButton.interactable  = false;
        }
        else
        {
            Debug.LogWarning("Some fields are empty. Cannot save.");
        }
    }

    // Load the player data from the file into a list
    public List<PlayerData> LoadPlayerDataFromFile()
    {
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Clear the existing list
            playerDataList.Clear();

            // Parse each line and add it to the player data list
            foreach (string line in lines)
            {
                PlayerData playerData = PlayerData.FromString(line);
                playerDataList.Add(playerData);
            }
        }
        return playerDataList;
    }
}
