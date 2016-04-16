public class Key : LevelEntity {

    public override void OnPlayerEnter(MoveDirection dir) {
        base.OnPlayerEnter(dir);
        GameController.GetInstance().Player.Upgrade();
        Cell.RemoveItem();
    }
}
