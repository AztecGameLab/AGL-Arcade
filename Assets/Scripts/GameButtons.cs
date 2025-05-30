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
    /// <summary> The index of the currently selected game </summary>
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
            directory.text = Data.gameDirectories[0];
            selector.UpdatePage();
        }
        // Otherwise, create a new JSON file for saved data
        else{Data = new ArcadeData();}
    }

    /* -------------------
     * Game Menu Buttons |
     * ------------------- */
    
    /// <summary> Functionality for the "Play" Button </summary>
    public void StartGame()
    {
        // If the game is installed, open the application
        if (Directory.Exists(directory.text)) 
        { Process.Start(directory.text + "/" + selector.GetData().executableFile); }
    }
    
    /// <summary> Functionality for the "Link to Download Page" Button </summary>
    public void DownloadLink() { Application.OpenURL(GameLink); }

    /// <summary> Functionality for the "Locate" Button </summary>
    public void LocateFile()
    {
        // Open the File Explorer, have the user choose the .app file of the game
        var path = StandaloneFileBrowser.OpenFolderPanel("Open Folder", "", false)[0];
        // Set the "Current Directory" text to the chosen directory
        directory.text = path ?? "";
        // Set the game's directory in the Arcade's Saved Data to the chosen directory
        Data.gameDirectories[buttonIndex] = path ?? "";
        // Save the Data and Update the page with the chosen directory 
        SaveIntoJson();
        selector.UpdatePage();
    }

    /// <summary> Functionality for the "Locate Folder" Button </summary>
    public void LocateAllFiles()
    {
        // Have the user choose a directory / folder 
        var path = StandaloneFileBrowser.OpenFolderPanel("Open Folder", "", false)[0];
        if (path == "") return; // If nothing is chosen, return
        // Get all the folders inside this directory
        var list = Directory.GetDirectories(path);
        // For each folder
        foreach(var item in list) {
            // Check if it contains the valid .exe file
            // If so, set the saved directory for the game in the Arcade Saved Data to this directory
            var gameFiles = Directory.GetFiles(item);
            foreach (var gameFile in gameFiles)
            {
                for (var i = 0; i < allData.Length; i++) {
                    if (!Path.GetFileName(gameFile).Equals(allData[i].executableFile)) continue;
                    Data.gameDirectories[i] = item; }
            }
        }
        // Save the Data and Update the page with the chosen directory 
        SaveIntoJson();
        selector.UpdatePage();
    }

    /// <summary> Functionality for the "Delete" Button </summary>
    public void DeleteFile()
    {
        // Retrieve the name of the directory the "Folder to be Deleted" is in
        var parentDir = Directory.GetParent(directory.text); string parent;
        if (parentDir != null) { parent = parentDir.FullName; }
        else {parent = "";}
        // Open up this directory, and allow the user to select the folder they want to delete (confirmation action)
        var path = StandaloneFileBrowser.OpenFolderPanel("Open Folder", parent, false)[0];
        if (path == "" || path != directory.text) return; // If no file is chosen, or the wrong file is chosen, return
        // Create a trash directory in the persistent data path (Safer than directly deleting the file)
        var trashPath = Path.Combine(Application.persistentDataPath, "Trash");
        // Move the file to the trash directory, and then delete the trash directory
        Directory.Move(path, trashPath);
        Directory.Delete(trashPath, true);
        // Update the Arcade Menu page 
        selector.UpdatePage();
    }

    /* ---------------------
     * Arrow Key Selection |
     * --------------------- */
    
    // Key Input Functionality
    private void Update()
    {
        // Select the next game using the "Down Arrow" or "S" Key
        if(Input.GetKeyDown(KeyCode.DownArrow)) { NextButton(); }
        // Select the next game using the "Up Arrow" or "W" Key
        else if(Input.GetKeyDown(KeyCode.UpArrow)) { PreviousButton(); }
        // Quit AGL-Arcade using the Escape key
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }

    /// <summary> Functionality for selecting the next game </summary>
    private void NextButton() {
        // If the last game is currently selected, loop back to the first game
        if (buttonIndex == buttons.Length - 1) buttonIndex = 0;
        else {buttonIndex++;} // Otherwise, increment the index

        // Select and Click the button at this index
        buttons[buttonIndex].Select(); 
        buttons[buttonIndex].onClick.Invoke();
    }

    /// <summary> Functionality for selecting the previous game </summary>
    private void PreviousButton()
    {
        // If the first game is currently selected, skip to the last game
        if (buttonIndex == 0) buttonIndex = buttons.Length - 1;
        else {buttonIndex--;} // Otherwise, decrement the index

        // Select and Click the button at this index
        buttons[buttonIndex].Select();
        buttons[buttonIndex].onClick.Invoke();
    }

    /// <summary> Functionality for changing the button index when a button is clicked w/ the mouse </summary>
    public void SetIndex(int index)
    {
        // If the index is out of range, return
        if (index > buttons.Length || index < 0) return;
        buttonIndex = index;
    }
    
    /* ---------------------
     * Arcade Saved Data |
     * --------------------- */    
    
    /// <summary> Update the "current directory" using the Saved Data </summary>
    public void UpdateDirectory()
    {
        // If there is no Saved Data, return
        if (Data.IsUnityNull()) return;
        // Set the "Current Directory" to the saved directory of the current game 
        directory.text = Data.gameDirectories[buttonIndex];
    }
    
    /// <summary> Save AGL-Arcade Data </summary>
    private void SaveIntoJson(){
        // Write the current Data into the /ArcadeData.json Saved Data File
        var data = JsonUtility.ToJson(Data);
        File.WriteAllText(Application.persistentDataPath + "/ArcadeData.json", data);
    }

}
