public class Arrow : LevelEntity {
    public MoveDirection Dir = MoveDirection.Up;
    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);


        if (Cell.Map.IsCanStep(Coords + Player.GetDirection(Dir), Dir))
        {
            GameController.GetInstance().Player.Move(Dir);
        }
    }
}
