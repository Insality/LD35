
public class Spikes : LevelEntity {
    public override void OnPlayerStay()
    {
        base.OnPlayerStay();
        if (Cell.State == CellState.Normal)
        {
            Cell.SetNextState(CellState.Warning);
        }
    }
}
