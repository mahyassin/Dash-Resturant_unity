using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class MapView : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI mapPrinter; 
    [SerializeField]private TextMeshProUGUI progress;

    private string _stoveProgress, _chopingProgress, _clock;


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

    public void UpdateClock(int clock)
    {

        _clock = $"\nClock: {new string('.', clock)}";
        DisplayProgress();
    }

    public void UpdateStove(int[] progress)
    {
        _stoveProgress = "";
        foreach(int value in progress)
        {
            _stoveProgress += "\nStove: " + new string('.', value);
        }
        DisplayProgress();
    }

    public void UpdateChoppingBoard(int[] progress)
    {
        _chopingProgress = "";
        foreach(int value in progress)
        {
            _chopingProgress += "\nChopping board: " + new string('.', value);
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
