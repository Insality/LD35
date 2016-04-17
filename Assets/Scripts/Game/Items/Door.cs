

public class Door : LevelEntity
{

    private bool _isEnabled;

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);

        _isEnabled = false;
        foreach (var item in Cell.Map.Plates)
        {
            if (!item.IsEnabled)
            {
                _isEnabled = true;
            }
        }

        Graphics.gameObject.SetActive(_isEnabled);
        CanStepOn = !_isEnabled;
    }
}
