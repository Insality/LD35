
public class Spikes : LevelEntity {
    protected override void Start()
    {
        base.Start();
        Anim.AddClip(AppController.GetInstance().SpikesUp, "Spike");
    }

    public override void OnPlayerStay()
    {
        base.OnPlayerStay();
        if (Cell.State == CellState.Normal)
        {
            Cell.SetNextState(CellState.Warning);
        }
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        if (Cell.State == CellState.Warning)
        {
            Anim.Play("Spike");
        }
    }
}
