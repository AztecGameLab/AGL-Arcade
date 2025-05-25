using System;
using System.Diagnostics;
using System.IO;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameSelector : MonoBehaviour
{
    
    [Tooltip("TextMeshPro for the error message")]
    [SerializeField] private TextMeshProUGUI text;
    /// <summary> True if the error message is already visible </summary>
    private bool error_shown;
    
    public void PlayGame(string fileName)
    {
        // Path to the StreamingAssets folder
        var path = Application.streamingAssetsPath;
        // Get the directory to download the path
        var filePath = Path.Combine(path, "AGL_Games", fileName);
        filePath = Path.Combine(filePath, fileName);
        // Make sure the slashes are the right direction
        filePath = filePath.Replace("\\", "/");
        // Get the operating system, and add the right end tag
        string system = SystemInfo.operatingSystem;
        if (system.Contains("Mac")) { filePath += "Mac"; }
        else if (system.Contains("Linux")) { filePath += "Linux"; }

        filePath += ".app";
        // Try starting the game. If it doesn't work, show an error
        try
        {
            Debug.Log(filePath);
            Process.Start(filePath); }
        catch (Exception _) {
            ShowError();
        }
    }

    private async void ShowError()
    {
        if (error_shown) return;
        error_shown = true;
        text.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        text.CrossFadeAlpha(0, 2, true);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        text.enabled = false;
        text.CrossFadeAlpha(1, 0, true);
        error_shown = false;
    }
}
