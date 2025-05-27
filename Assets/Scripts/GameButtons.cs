using System.Diagnostics;
using System.IO;
using SFB;
using TMPro;
using TriInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using File = UnityEngine.Windows.File;

[HideMonoScript]
public class GameButtons : MonoBehaviour
{
    [SerializeField] private GameSelector selector;
    [SerializeField] private TMP_InputField directory;
    public string GameLink { get; set; }
    
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
        selector.UpdatePage();
    }

    public void DeleteFile()
    {
        
        var extensions = new [] {
            new ExtensionFilter("Application", "app" ),
        };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];
        if (path.IsUnityNull()) return;
        var pathName = Path.GetFileName(path);
        var trashPath = Path.Combine(Application.persistentDataPath, "Trash", pathName);
        Directory.Move(path, trashPath);
        Directory.Delete(trashPath);
    }
    
}
