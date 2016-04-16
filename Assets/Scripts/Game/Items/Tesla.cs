public class Tesla : LevelEntity {
    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        if ((int) (Coords.x + Coords.y + counter)%2 == 0)
        {
            if (Cell.State == CellState.Normal || Cell.State == CellState.Damage)
            {
                Cell.SetNextState(CellState.Warning);
            }
        }
    }
}
