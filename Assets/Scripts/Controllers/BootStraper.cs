using UnityEngine;

public class BootStraper : MonoBehaviour
{

    private LevelDesginer levelDesginer;
    private MapController mapController;
    [SerializeField]private MapView mapView;
    private GameState state;
    private InputReader inputs;


    void Awake()
    {
        levelDesginer = new();
        inputs = new();

        state = levelDesginer.GetState();
        mapController = new(state, mapView,inputs);

        inputs.Moved += Test;
    }

    private void Test(Vector2 dir)
    {
        Debug.Log(dir);
    }
}
