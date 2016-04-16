using UnityEngine;

public class LevelEntity : BaseEntity
{
    [HideInInspector] public Cell Cell;
    public bool CanStepOn = true;

    public void Init(Cell cell)
    {
        Coords = cell.Coords;
        Cell = cell;
        SetPos();
    }

    public virtual void OnPlayerEnter(MoveDirection dir)
    {
    }

    public virtual void OnPlayerLeave()
    {
    }

    public virtual void OnPlayerStay()
    {
    }

    public virtual void OnGameBeat(int counter)
    {
        
    }
}
