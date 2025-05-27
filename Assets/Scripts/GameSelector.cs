using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Image = UnityEngine.UI.Image;

public class GameSelector : MonoBehaviour
{

    [SerializeField] private GameData defaultGame;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TMP_InputField url;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        SelectGame(defaultGame);
    }

    public void SelectGame(GameData game)
    {
        if(SystemInfo.operatingSystem )
        var gamePath = Path.Combine(url.text, "Contents/MacOS", );
    }
}
