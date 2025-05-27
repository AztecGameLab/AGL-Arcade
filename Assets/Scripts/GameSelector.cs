using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
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
    
    [Header("Sister Scripts")]
    [Tooltip("Script that controls the buttons")]
    [SerializeField] private GameButtons buttonController;
    
    
    // Start is called before the first frame update
    private void Start() { SelectGame(defaultGame); }

    public void SelectGame(GameData game)
    {

        description.text = game.description;
        
        var gamePath = Path.Combine(directory.text, "Contents/MacOS", game.executableFile);
        image.sprite = File.Exists(gamePath) ? game.unlocked : game.locked;

        buttonController.GameLink = game.url;
    }
}
