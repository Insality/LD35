public class Hint : LevelEntity
{
    public string HintText;

    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        GameController.GetInstance().ShowHint(HintText);
    }

    public override void OnPlayerLeave()
    {
        base.OnPlayerLeave();
        GameController.GetInstance().HideHint();
    }
}
