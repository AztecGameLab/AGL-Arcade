using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class GameSelector : MonoBehaviour
{
    
    public void PlayGame(string fileName)
    {
        var path = Application.streamingAssetsPath;
        var filePath = Path.Combine(path, "AGL_Games");
        var fileDir = fileName.Split('/');
        foreach(var dir in fileDir){
            filePath = Path.Combine(filePath, dir);
        }
        filePath = filePath.Replace("\\", "/");
        UnityEngine.Debug.Log(filePath);
        try{
            Process.Start(filePath);
        }
        catch (Win32Exception){UnityEngine.Debug.Log("File not Found");}
    }
}
