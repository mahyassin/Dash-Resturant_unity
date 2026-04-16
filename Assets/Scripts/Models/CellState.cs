public readonly struct CellState
{
    public IOcuppier Ocuppier {get;}
   
    public bool IsWalkable => Ocuppier == null;

    public CellState(IOcuppier ocuppier)
    {
        Ocuppier = ocuppier;
    }

}