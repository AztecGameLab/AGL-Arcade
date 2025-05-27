using System.IO;
using TMPro;
using TriInspector;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using File = UnityEngine.Windows.File;
using Image = UnityEngine.UI.Image;

[HideMonoScript]
public class GameSelector : MonoBehaviour
{

    [Header("Defaults")]
    [Tooltip("The game that will be displayed by default.")]
    [SerializeField] private GameData defaultGame;
    
    [Header("Info Page Assets")]
    [Tooltip("The image of the game.")]
    [SerializeField] private Image image;
    [Tooltip("The description for the game.")]
    [SerializeField] private TextMeshProUGUI description;
    [Tooltip("Location of the game in the player's system.")]
    [SerializeField] private TMP_InputField directory;

    [Tooltip("Data for the current game")] 
    private GameData data;
    
    [Header("Buttons")]
    [Tooltip("The Play Button")]
    [SerializeField] private Button playButton;
    [Tooltip("The Delete Button")] 
    [SerializeField] private Button deleteButton;
    [Tooltip("Script that controls the buttons")]
    [SerializeField] private GameButtons buttonController;
    
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        SelectGame(defaultGame);
    }

    public void SelectGame(GameData game)
    {

        description.text = game.description;
        buttonController.GameLink = game.url;

        data = game;
        buttonController.UpdateURL();
        UpdatePage();
    }

    public void UpdatePage()
    {
        var gamePath = Path.Combine(directory.text, "Contents/MacOS", data.executableFile);
        var fileExists = File.Exists(gamePath);
        image.sprite = fileExists ? data.unlocked : data.locked;
        playButton.interactable = fileExists;
        deleteButton.interactable = fileExists;
        //playButton.color = fileExists ? new Color(85, 245, 115) : Color.gray;
        //deleteButton.color = fileExists ? new Color(245, 90, 90) : Color.gray;
    }
}
