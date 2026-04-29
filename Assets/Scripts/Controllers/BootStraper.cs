using System.Collections.Generic;
using System.Reflection.Emit;
using Codice.CM.WorkspaceServer;
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
    private EntitiesFactory factory;

    [SerializeField] private Timer timer;
    [SerializeField] private ViewManager mapView;
    [SerializeField] private ViewFactory viewFactory;



    void Awake()
    {
        viewsRegistry     = new();
        levelDesginer     = new();
        inputs            = new();
        identfier         = new();
        factory           = new(identfier);

        state = levelDesginer.GetState(factory, identfier);
        BuidMap(state);

        mainOrchestrator = new(state, mapView, inputs, timer, viewsRegistry);

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




            if (cell.Ocuppier is CharacterState character)
            {
                viewsRegistry.AddCharacter(character.Id, viewFactory.CreateCharacter(worldpos));
            }

            TileCode tileCode;

            tileCode = cell.Ocuppier switch
            {
                Stove => TileCode.Stove,
                Shelf => TileCode.Shelf,
                Wall  => TileCode.Wall,

                _     => TileCode.Empty,
            };

            if(cell.Ocuppier is ICarrier carrier)
            {
                if(carrier.OnCarrier is not IIdentifialbe identifialbe) goto Out;
                var icon = carrier.OnCarrier switch
                {
                    Ingredient i => i.Type switch
                    {
                        IngredientType.TOMATO => Icon.TOMATO,
                        IngredientType.ONION  => Icon.ONION,
                        IngredientType.POTATO => Icon.POTATO,
                    },

                    Pot  => Icon.Pot,
                    Dish => Icon.Dish,
                    _    => Icon.Error
                };
                // viewsRegistry.AddCarriable(identifialbe.Id, viewFactory.CreateCarraible(worldpos, icon, )); TODO()
            }

            Out:;

            map[pos] = new CellView(tileCode, pos);
        }

        mapView.InitializeCells(map);
    }

    private void Test(Vector2 dir)
    {
        // Debug.Log(dir);
    }
}
