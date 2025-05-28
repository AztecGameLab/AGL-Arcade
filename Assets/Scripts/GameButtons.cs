using System.Diagnostics;
using System.IO;
using SFB;
using TMPro;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Input = UnityEngine.Input;

[HideMonoScript]
public class GameButtons : MonoBehaviour
{
    /// <summary> Saved Data for AGL-Arcade </summary>
    private ArcadeData Data;
    
    [Header("Game Components")]
    
    [Tooltip("The script that manages the game selection menu")]
    [SerializeField] private GameSelector selector;
    [Tooltip("The data for each game in the arcade")]
    [SerializeField] private GameData[] allData;
    
    [Header("Page Data")]
    
    [Tooltip("The directory of the currently selected game")]
    [SerializeField] private TMP_InputField directory;
    /// <summary> The link to the download page of the currently selected game </summary>
    public string GameLink { get; set; }
    
    [Tooltip("All of the game-selection buttons")]
    [SerializeField] private Button[] buttons;
    /// <summary> The index of the the currently selected game </summary>
    private int buttonIndex;
    

    public void Start()
    {
        // Check if saved data for AGL-Arcade already exists
        var filePath = Application.persistentDataPath + "/ArcadeData.json";
        // If it exists
        if (File.Exists(filePath)) {
            // Set the game's current data to the saved local data
            var data = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<ArcadeData>(data);
        }
        // Otherwise, create a new JSON file for saved data
        else{Data = new ArcadeData();}
    }

    /// <summary> Functionality for the "Play" Button </summary>
    public void StartGame()
    {
        // If the game is installed, open the application
        if (Directory.Exists(directory.text)) 
        { Process.Start(directory.text); }
    }
    
    /// <summary> Functionality for the "Link to Download Page" Button </summary>
    public void DownloadLink() { Application.OpenURL(GameLink); }

    /// <summary> Functionality for the "Locate" Button </summary>
    public void LocateFile()
    {
        // The extensions that are allowed to be selected
        var extensions = new [] { new ExtensionFilter("Application", "app" ), };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];
        directory.text = path ?? "";
        Data.gameDirectories[buttonIndex] = path ?? "";
        SaveIntoJson();
        selector.UpdatePage();
    }

    public void LocateAllFiles()
    {
        var path = StandaloneFileBrowser.OpenFolderPanel("Open Folder", "", false)[0];
        if (path == "") return;
        var list = Directory.GetDirectories(path);
        foreach(var item in list) {
            var gameFile = Directory.GetFiles(item + "/Contents/MacOS")[0];
            for (var i = 0; i < allData.Length; i++) {
                if (!Path.GetFileName(gameFile).Equals(allData[i].executableFile)) continue;
                Data.gameDirectories[i] = item; }
        }
        SaveIntoJson();
        selector.UpdatePage();
    }

    public void DeleteFile()
    {
        var extensions = new [] {
            new ExtensionFilter("Application", "app" ),
        };
        var parent = Directory.GetParent(directory.text).FullName;
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", parent, extensions, false)[0];
        if (path == "") return;
        var trashPath = Path.Combine(Application.persistentDataPath, "Trash");
        Directory.Move(path, trashPath);
        Directory.Delete(trashPath, true);
        selector.UpdatePage();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            NextButton();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            PreviousButton();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void NextButton()
    {
        if (buttonIndex == buttons.Length - 1) return;
        buttonIndex++; 
        buttons[buttonIndex].Select();
        buttons[buttonIndex].onClick.Invoke();
    }

    private void PreviousButton()
    {
        if (buttonIndex == 0) return;
        buttonIndex--; 
        buttons[buttonIndex].Select();
        buttons[buttonIndex].onClick.Invoke();
    }

    public void SetIndex(int index)
    {
        if (index > buttons.Length || index < 0) return;
        buttonIndex = index;
    }

    public void UpdateDirectory()
    {
        if (Data.IsUnityNull()) return;
        directory.text = Data.gameDirectories[buttonIndex];
    }
    
    private void SaveIntoJson(){
        var data = JsonUtility.ToJson(Data);
        File.WriteAllText(Application.persistentDataPath + "/ArcadeData.json", data);
    }

}
