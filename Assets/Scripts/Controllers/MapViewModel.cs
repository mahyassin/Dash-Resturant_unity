public class MapViewModel
{
     public string[] DecodeState(GameState state)
    {
        string[] output = new string[state.MapHieght];
        for(int j = 0; j < state.MapHieght; j++)
        {
            for(int i = 0; i < state.MapWidth; i++)
            {
                var occupier = state.Map[new(i, j)].Ocuppier;

                string baseCell;
                string ontile;
                
                if (occupier is PlayerState player)
                {
                    baseCell = "▼";
                    ontile = DecodeCarriable(player.onPlayer);

                }
                else if (occupier is Wall)
                {
                    baseCell = "W";
                    ontile = "W ";
                }
                else
                {
                    baseCell = ".";
                    ontile = ". ";
                }

                output[j] +=  $"{baseCell}{ontile} ";

            }
        }

        return output;
    }

    public string DecodeCarriable(ICarriable carriable)
    {
        return carriable switch
        {
            _ => ". ",
        };
    }
}