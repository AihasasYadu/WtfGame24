namespace SnipOuts.Grid
{
    public interface ICell
    {
        public CellTypeEnum CellType { get; }
        public OccupantObjectTypeEnum OccupantObjectType { get; }
        public void SetupCell (CellTypeEnum cellTypeEnum, int columnIndex, int rowIndex);
        public void SetupOccupantObject (OccupantObjectTypeEnum occupant, OccupantPositionEnum positionEnum);
        public void OnCellTapped ();
    }
}