public readonly struct CellState
{
    public int  BaseCell {get;}
    public int  OnCell {get;}

    public CellState(int baseCell, int oncell)
    {
        BaseCell = baseCell;
        OnCell = oncell;
    }

}