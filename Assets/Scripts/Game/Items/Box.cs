public class Box : LevelEntity {
    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        if (Cell.CanMoveItem(dir))
        {
            Cell.MoveItem(dir);
        }
    }
}
