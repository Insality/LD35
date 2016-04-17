

public class Lock : LevelEntity
{

    private bool _isEnabled;

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);

        _isEnabled = true;
        if (GameController.GetInstance().Player.Level >= 6)
        {
            _isEnabled = false;
        }

        Graphics.gameObject.SetActive(_isEnabled);
        CanStepOn = !_isEnabled;
    }
}
