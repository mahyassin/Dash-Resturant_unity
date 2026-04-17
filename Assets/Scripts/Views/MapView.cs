using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class MapView : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI mapPrinter; 
    [SerializeField]private TextMeshProUGUI progress;


    public void DisplayMap(string[] mapCode)
    {
        string text = "";
        foreach(var line in mapCode)
        {
            text =  "\n \n" + line + text ;
        };

        string coloredText = $"<color=red> {text} </color>";

        mapPrinter.text = $"<mspace=14> {coloredText} </mspace>";
    }

    public void DisplayClock(int clock)
    {

        progress.text = $"Clock: {new string('.', clock)}";
    }
}

public struct CellView
{
    public string Incon {get;}
    public Vector2Int Pos {get;}
}
