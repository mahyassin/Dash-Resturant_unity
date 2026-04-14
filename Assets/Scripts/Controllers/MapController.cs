using Codice.Client.BaseCommands;
using UnityEngine;

public class MapController
{

    private GameState _gameState;
    private MapView _mapview;
    public MapController(GameState gameState, MapView mapView)
    {
        _gameState = gameState;
        _mapview = mapView;

    }

    
}
