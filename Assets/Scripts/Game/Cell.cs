using System;
using UnityEngine;

public class Cell : BaseEntity
{
    public CellState State = CellState.Normal;
    public Map Map;
    public LevelEntity LevelItem;

    private bool _isNextState;
    private CellState _nextState;

    public void SetState(CellState state)
    {
        if (state == CellState.Warning && State == CellState.Disabled) return;
        State = state;
        RefreshGraphics();
    }

    private void RefreshGraphics()
    {
        switch (State){
            case CellState.Normal:
                Graphics.SetSprite("NormalCell");
                break;
            case CellState.Disabled:
                Graphics.SetSprite("DisabledCell");
                break;
            case CellState.Warning:
                Graphics.SetSprite("WarningCell");
                break;
            case CellState.Damage:
                Graphics.SetSprite("DamageCell");
                break;
            default:
                throw new ArgumentOutOfRangeException("state");
        }
    }

    public LevelEntity AddItem(GameObject prefab)
    {
        LevelEntity item = Instantiate(prefab).GetComponent<LevelEntity>();
        item.transform.parent = MyTransform.parent;
        item.Init(this);
        LevelItem = item;

        if (item is Plate)
        {
            Map.Plates.Add(item as Plate);
        }
        return item;
    }

    public void OnMoveOn(MoveDirection dir)
    {
        if (LevelItem != null) {
            LevelItem.OnPlayerEnter(dir);
        }
    }

    public void OnStay() {
        if (LevelItem != null) {
            LevelItem.OnPlayerStay();
        }
    }

    public void OnMoveFrom()
    {
        if (LevelItem != null)
        {
            LevelItem.OnPlayerLeave();
        }
    }

    public void OnGameBeat(int counter)
    {
        if (LevelItem != null)
        {
            LevelItem.OnGameBeat(counter);
        }


        if (_isNextState)
        {
            _isNextState = false;
            SetState(_nextState);
            return;
        }

        if (State == CellState.Damage)
        {
            SetState(CellState.Normal);
        }
        if (State == CellState.Warning)
        {
            SetState(CellState.Damage);
        }
        
    }

    public void SetNextState(CellState state)
    {
        _isNextState = true;
        _nextState = state;
    }

    public void RemoveItem()
    {
        if (LevelItem != null)
        {
            Destroy(LevelItem.gameObject);
            LevelItem = null;
        }
    }

    public bool CanMoveItem(MoveDirection dir)
    {
        if (LevelItem is Box)
        {
            if (Map.IsCanStep(Coords + Player.GetDirection(dir), dir) &&
                (Map.GetCell(Coords + Player.GetDirection(dir)).LevelItem == null))
            {
                return true;
            }
            return false;
        }
        return true;
    }

    public void MoveItem(MoveDirection dir)
    {
        var delta = Player.GetDirection(dir);
        var nextCell = Map.GetCell(Coords + delta);

        LevelItem.Cell = nextCell;
        nextCell.LevelItem = LevelItem;
        nextCell.LevelItem.Coords = nextCell.Coords;
        nextCell.LevelItem.SetPos();
        LevelItem = null;
    }
}

public enum CellState
{
    Normal = 0,
    Disabled = 1,
    Warning = 2,
    Damage = 3,
}
