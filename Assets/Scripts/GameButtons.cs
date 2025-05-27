using System;
using System.Diagnostics;
using System.IO;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using File = UnityEngine.Windows.File;

public class GameButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField url;
    private string GameLink { get; set; }
    
    
    public void StartGame()
    {
        if (File.Exists(url.text))
        {
            Process.Start(url.text);
        }
    }

    public void DownloadLink()
    {
        Application.OpenURL(GameLink);
    }

    public void LocateFile()
    {
        
    }

    public void DeleteFile()
    {
        
    }
}
