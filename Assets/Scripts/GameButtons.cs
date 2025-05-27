using System;
using System.Diagnostics;
using System.IO;
using Cysharp.Threading.Tasks;
using TMPro;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using File = UnityEngine.Windows.File;

[HideMonoScript]
public class GameButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField url;
    public string GameLink { get; set; }
    
    public void StartGame()
    {
        if (File.Exists(url.text)) { Process.Start(url.text); }
    }

    public void DownloadLink() { Application.OpenURL(GameLink); }

    public void LocateFile()
    {
        
    }

    public void DeleteFile()
    {
        
    }
    
}
