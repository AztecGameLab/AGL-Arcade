using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;

[HideMonoScript]
[CreateAssetMenu(menuName = "GameData")]
public class GameData : ScriptableObject
{
    [Header("Images")]
    
    [Tooltip("The image that is shown when the game is not installed.")]
    public Sprite locked;
    [Tooltip("The image that is shown when the game is installed.")]
    public Sprite unlocked;

    [Header("Other")] 
    
    [Tooltip("Description for the game.")]
    public string description;
    [Tooltip("URL to the game's Itch.io download page.")]
    public string url;
    [Tooltip("The name of the file that is executed to run the game.")]
    public string executableFile;

}
