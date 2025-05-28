using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject textBox;
    
    public void OnPointerEnter(PointerEventData _) { textBox.SetActive(true); }
    
    public void OnPointerExit(PointerEventData _) { textBox.SetActive(false); }
    
}
