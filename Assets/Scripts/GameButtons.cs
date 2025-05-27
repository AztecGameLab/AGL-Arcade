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
    [SerializeField] private GameSelector selector;
    [SerializeField] private TMP_InputField directory;
    public string GameLink { get; set; }
    [SerializeField] private Button[] buttons;
    private int buttonIndex;
    private ArcadeData Data;

    public void Start()
    {
        string filePath = Application.persistentDataPath + "/ArcadeData.json";
        if (File.Exists(filePath))
        {
            var data = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<ArcadeData>(data);
        }
        else{Data = new ArcadeData();}
    }


    public void StartGame()
    {
        if (Directory.Exists(directory.text)) { Process.Start(directory.text); }
    }

    public void DownloadLink() { Application.OpenURL(GameLink); }

    public void LocateFile()
    {
        var extensions = new [] {
            new ExtensionFilter("Application", "app" ),
        };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];
        directory.text = path ?? "";
        Data.gameURLs[buttonIndex] = path ?? "";
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

    public void UpdateURL()
    {
        if (Data.IsUnityNull()) return;
        directory.text = Data.gameURLs[buttonIndex];
    }
    
    private void SaveIntoJson(){
        var data = JsonUtility.ToJson(Data);
        File.WriteAllText(Application.persistentDataPath + "/ArcadeData.json", data);
    }

}
