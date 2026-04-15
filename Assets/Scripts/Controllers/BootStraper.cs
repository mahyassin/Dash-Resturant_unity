using UnityEngine;

public class BootStraper : MonoBehaviour
{

    private LevelDesginer levelDesginer;
    private MapController mapController;
    [SerializeField]private MapView mapView;
    private GameState state;


    void Awake()
    {
        levelDesginer = new();
        state = levelDesginer.GetState();
        mapController = new(state, mapView);

    }
}
