using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.IO.Compression;
using TMPro;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class AddonManager : MonoBehaviour
{
    [Header("Addon")]
    [Tooltip("Link to the game download in the Github")] 
    [SerializeField] private string addonName;
    private string addonExt;
    
    [Header("Game Images")]
    [Tooltip("The Image of the game button")]
    [SerializeField] private Image picture;
    [Tooltip("The sprite to show the game has not been downloaded")]
    [SerializeField] private Sprite lockedImage;
    [Tooltip("The sprite to show the game has been downloaded")]
    [SerializeField] private Sprite unlockedImage;

    [Header("Download Button & URLs")]
    [Tooltip("The TextMeshPro of the game download button")]
    [SerializeField] private TextMeshProUGUI text;
    private const string DownloadURL = "https://media.githubusercontent.com/media/AztecGameLab/AGL-Builds/refs/heads/main/";
    private string addonUrl;
    private string extractPath;
    private string operatingSystem;
    
    public void Start(){
        // Extract the addon to StreamingAssets/AGL_Games/[addonName]
       extractPath = Path.Combine(Application.streamingAssetsPath, "AGL_Games", addonName);
       // Create part of the Game's Download URL
       addonUrl = DownloadURL + addonName;
       // Get the operating system of the user
       operatingSystem = SystemInfo.operatingSystem;
       // Add (Mac) or (Linux) if the game is for either of those operating systems
       if (operatingSystem.Contains("Mac")) { addonExt = "Mac"; }
       else if (operatingSystem.Contains("Linux")){ addonExt = "Linux"; }
       // Then add ".zip" to the end of the download link
       addonUrl += addonExt + ".tar.gz";
       // Reload the addons (show which are downloaded and which aren't)
       ReloadAddons();
    }

    /// <summary>
    /// Begin the coroutine to download and load the game addon
    /// </summary>
    public void GetAddon() { StartCoroutine(DownloadAndLoadAddon(addonUrl)); }
    
    /// <summary>
    /// Checks the player's file system to see which games are installed and which aren't, and updates the images
    /// and download button texts as necessary
    /// </summary>
    private void ReloadAddons()
    {
        Debug.Log("Reload Addons Path: " + Path.Combine(extractPath, addonName + addonExt));
        // Check if the file exists at the extract path
        if (File.Exists(Path.Combine(extractPath, addonName + addonExt))){
            // Game is installed (show unlocked image and text)
            picture.sprite = unlockedImage;
            GetComponent<Button>().enabled = false;
            text.text = "Re-Download"; }
        else {   
            // Game is not installed (show locked image and text)
            picture.sprite = lockedImage;
            GetComponent<Button>().enabled = true;
            text.text = "Download"; }
    }
    
    IEnumerator DownloadAndLoadAddon(string url)
    {
        // Get the name of the file to download
        var fileName = Path.GetFileName(url);
        // Path to the download directory
         var downloadPath = Path.Combine(Application.streamingAssetsPath, fileName);

        // UWR to grab the download from the given URL
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        // If the request fails, log an error
        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Download failed: " + request.error); }
        else {
            // Write the requested data to a file in the download path
            File.WriteAllBytes(downloadPath, request.downloadHandler.data);

            // If it's zipped:
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = $"-c \"tar -xzvf '{downloadPath}' -C '{extractPath}'\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();
            //ExtractTarGz(downloadPath, extractPath);
            ReloadAddons();

            /* Then load the AssetBundle
            string bundlePath = Path.Combine(extractPath, "yourbundle");
            var bundle = AssetBundle.LoadFromFile(bundlePath);

            if (bundle != null)
            {
                var prefab = bundle.LoadAsset<GameObject>("YourPrefabName");
                Instantiate(prefab);
                bundle.Unload(false);
            } */
        }
    }
    
    /*public static void ExtractTarGz(string archivePath, string destinationPath)
    {
        using (var archive = ArchiveFactory.Open(archivePath))
        {
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destinationPath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }
    }*/
}
