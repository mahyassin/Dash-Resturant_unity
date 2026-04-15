using Codice.Client.BaseCommands;
using JetBrains.Annotations;
using UnityEngine;

public class MapController
{

    private GameState _gameState;
    private MapView _mapview;
    public MapController(GameState gameState, MapView mapView)
    {
        _gameState = gameState;
        _mapview = mapView;

        _mapview.DisplayMap(DecodeState(_gameState));
    }


    public string[] DecodeState(GameState state)
    {
        string[] output = new string[state.MapHieght];
        for(int j = 0; j < state.MapHieght; j++)
        {
            for(int i = 0; i < state.MapWidth; i++)
            {
                var cell = state.Map[new(i, j)];


                char ontile = cell.OnCell switch
                {
                    Code.WALL => 'W',
                    _         => '.',
                };


                char baseCell = cell.BaseCell switch
                {
                    Code.PLAYER => '▼',
                    Code.WALL => 'W',
                    _ => '.',
                };

                output[j] +=  $"{baseCell}{ontile} ";


            }
        }

        return output;
    }
}
