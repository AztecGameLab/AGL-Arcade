using System.IO;
using TMPro;
using TriInspector;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using File = System.IO.File;
using Image = UnityEngine.UI.Image;

[HideMonoScript]
public class GameSelector : MonoBehaviour
{
    
    [Tooltip("Data for the current game")] 
    [SerializeField] private GameData data;
    
    [Header("Info Page Assets")]
    [Tooltip("The image of the selected game.")]
    [SerializeField] private Image image;
    [Tooltip("The description for the selected game.")]
    [SerializeField] private TextMeshProUGUI description;
    [Tooltip("Location of the selected game in the player's system.")]
    [SerializeField] private TMP_InputField directory;
    
    [Header("Buttons")]
    [Tooltip("The Play Button")]
    [SerializeField] private Button playButton;
    [Tooltip("The Delete Button")] 
    [SerializeField] private Button deleteButton;
    [Tooltip("Script that controls the buttons")]
    [SerializeField] private GameButtons buttonController;
    
    // When the game starts, select the default game 
    private void Start() { SelectGame(data); }

    /// <summary> Changes the Arcade Menu page to the selected game </summary>
    /// <param name="game"> The selected game </param>
    public void SelectGame(GameData game)
    {
        // Change the description and the download page URL
        description.text = game.description;
        buttonController.GameLink = game.url;
        
        // Set data (the current game) to the selected game
        data = game;
        
        // Update the directory in the game's saved data 
        buttonController.UpdateDirectory();
    }

    /// <summary> Update the Arcade Menu Page, depending on whether the game is installed </summary>
    public void UpdatePage()
    {
        // Check if the directory given leads to a valid game file
        var gamePath = Path.Combine(directory.text, data.executableFile);
        var fileExists = File.Exists(gamePath);
        
        // If so, show a colored image, and enable the play/delete buttons
        image.sprite = fileExists ? data.unlocked : data.locked;
        playButton.interactable = fileExists;
        deleteButton.interactable = fileExists;
    }

    public GameData GetData()
    {
        return data;
    }
}
