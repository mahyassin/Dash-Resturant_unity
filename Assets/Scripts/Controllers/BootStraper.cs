using UnityEngine;

public class BootStraper : MonoBehaviour
{

    private LevelDesginer levelDesginer;
    private MainOrchistrator mainOrchestrator;
    [SerializeField]private MapView mapView;
    private GameState state;
    private InputReader inputs;
    [SerializeField] private Timer timer;


    void Awake()
    {
        levelDesginer = new();
        inputs = new();

        state = levelDesginer.GetState();

        mainOrchestrator = new(state, mapView, inputs, timer);

        inputs.Moved += Test;
    }

    private void Test(Vector2 dir)
    {
        // Debug.Log(dir);
    }
}
