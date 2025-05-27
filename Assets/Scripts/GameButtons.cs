using System.Diagnostics;
using System.IO;
using SFB;
using TMPro;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using File = UnityEngine.Windows.File;

[HideMonoScript]
public class GameButtons : MonoBehaviour
{
    [SerializeField] private GameSelector selector;
    [SerializeField] private TMP_InputField directory;
    public string GameLink { get; set; }
    
    public void StartGame()
    {
        if (File.Exists(directory.text)) { Process.Start(directory.text); }
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
        
    }
    
}
