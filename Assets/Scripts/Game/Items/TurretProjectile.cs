﻿public class TurretProjectile : LevelEntity
{

    public MoveDirection Direction;
    private int _currentLifeTime;

    public void Init(MoveDirection direction)
    {
        Direction = direction;
        GameController.GetInstance().Map.GetCell(Coords).SetState(CellState.Warning);
        
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        _currentLifeTime++;

        if (counter%2 == 1)
        {
            Move();
        }

        if (_currentLifeTime > 6)
        {
            GameController.GetInstance().Map.DestroyItem(this);
        }
    }

    private void Move()
    {
        var nextCoords = Coords + (Player.GetDirection(Direction)*2);
        Coords = nextCoords;
        GameController.GetInstance().Map.GetCell(Coords).SetState(CellState.Warning);
        GameController.GetInstance().Map.GetCell(Coords - Player.GetDirection(Direction)).SetState(CellState.Warning);
        SetPos();
    }
}
