using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class MapView : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI mapPrinter; 
    [SerializeField]private TextMeshProUGUI progress;

    private string _stoveProgress, _chopingProgress, _clock;


    public void DisplayMap(string[] mapCode, string containersContent)
    {
        string text = "";
        foreach(var line in mapCode)
        {
            text =  "\n \n" + line + text ;
        };

        string coloredText = $"<color=red> {text} </color>";

        mapPrinter.text = $"<mspace=14> {coloredText} </mspace>";
        mapPrinter.text += "\n \n" + containersContent;
    }

    public void UpdateClock(int clock)
    {

        _clock = $"\nClock: {new string('.', clock)}";
        DisplayProgress();
    }

    public void UpdateStove((string, int)[] progress)
    {
        _stoveProgress = "";
        foreach((string, int) value in progress)
        {
            _stoveProgress += $"\n{value.Item1} " + new string('.', value.Item2);
        }
        DisplayProgress();
    }

    public void UpdateChoppingBoard((string, int)[] progress)
    {
        _chopingProgress = "";
        foreach((string, int) value in progress)
        {
            _chopingProgress += $"\n{value.Item1}: " + new string('.', value.Item2);
        }
        DisplayProgress();
    }

    public void DisplayProgress()
    {
        progress.text = _clock + _stoveProgress + _chopingProgress;
    }
}

public struct CellView
{
    public string Incon {get;}
    public Vector2Int Pos {get;}
}
