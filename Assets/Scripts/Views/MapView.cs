using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class MapView : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI mapPrinter; 

    
    public void DisplayMap(string[] mapCode)
    {
        string text = "";
        foreach(var line in mapCode)
        {
            text += "\n" + line ;
        };

        string coloredText = $"<color=red> {text} </color>";

        mapPrinter.text = $"<mspace=10> {coloredText} </mspace>";
    }
}

public struct CellView
{
    public string Incon {get;}
    public Vector2Int Pos {get;}
}
