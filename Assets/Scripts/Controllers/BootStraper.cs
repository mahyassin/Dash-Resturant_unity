using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class BootStraper : MonoBehaviour
{

    private LevelDesginer levelDesginer;
    private MainOrchistrator mainOrchestrator;
    private GameState state;
    private InputReader inputs;
    private ViewsRigistry viewsRegistry;
    private Identfier identfier;
    private EntitiesFactory stateFactory;

    [SerializeField] private Timer timer;
    [SerializeField] private ViewManager mapView;
    [SerializeField] private ViewFactory viewFactory;



    void Awake()
    {
        viewsRegistry     = new();
        levelDesginer     = new();
        inputs            = new();
        identfier         = new();
        stateFactory      = new(identfier);

        state = levelDesginer.GetState(stateFactory, identfier);
        BuidMap(state);

        FactoryContext context = new(viewFactory, stateFactory, identfier, viewsRegistry);

        mainOrchestrator = new(state, mapView, inputs, timer, context);

        inputs.Moved += Test;
    }

    public void BuidMap(GameState state)
    {
        Dictionary<Vector2Int, CellView> map = new();

        foreach(var cellstate in state.Map)
        {
            var cell = cellstate.Value;
            var pos = cellstate.Key;
            var worldpos = mapView.ToWorldPos(pos);

            var occupier = cell.Ocuppier;

            

            ITileView tileView = viewFactory.CreateView(occupier, worldpos);
            if (occupier != null && occupier is IIdentifialbe identifialbe) 
            {
                viewsRegistry.AddView(identifialbe.Id, tileView);
            }


            if(occupier is ICarrier carrier)
            {
                if(carrier.OnCarrier is not IIdentifialbe knowCarriable) goto Out;
               
                viewsRegistry.AddView(knowCarriable.Id, viewFactory.CreateCarriable(carrier.OnCarrier, tileView.Anchor.transform)); 
            }

            Out:;

            map[pos] = new CellView(tileView, pos);
        }

        mapView.InitializeCells(map);
    }

    private void Test(Vector2 dir)
    {
        // Debug.Log(dir);
    }
}
