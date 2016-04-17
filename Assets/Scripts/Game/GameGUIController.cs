using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class GameGUIController: MonoBehaviour
{
    public GameController GameController;
    public ScreenEffectController ScreenEffectController;
    public Shaker Shaker;
    public Shaker PermanentShaker;
    public VignetteAndChromaticAberration Vignette;
    public Fisheye Fishye;

    public TextMeshController HintText;
    public TextMeshController TimerText;
    public Transform StartScreen;

    private float _targetAbberation = 0.2f;
    private float _targetFishEye = 0.3f;

    void Start()
    {
        GameController.GameStep += GuiBeat;
        PermanentShaker.Shake(99999f);
    }

    void GuiBeat(int i)
    {
        Vignette.chromaticAberration = i%4==0? -15 : -12;
        if (i%4 == 0)
        {
            Fishye.strengthX = 0.4f;
            Fishye.strengthY = 0.4f;
        }

        if (!GameController.Player.IsDead)
        {
            ScreenEffectController.ShineScreen(-0.05f, 0.1f);
        }
    }

    public void SetPlayerHealth(int hp)
    {
        Vignette.intensity = 0.4f + (Constants.PlayerHp - hp)*0.03f;

        Fishye.strengthX = 0.35f;
        Fishye.strengthY = 0.35f;
    }

    void Update()
    {
        if (Vignette.chromaticAberration < _targetAbberation)
        {
            Vignette.chromaticAberration += Time.deltaTime*20;
        }
        else
        {
            Vignette.chromaticAberration = _targetAbberation;
        }

        if (Fishye.strengthX > _targetFishEye) {
            Fishye.strengthX  -= Time.deltaTime * 0.2f;
        } else {
             Fishye.strengthX = _targetFishEye;
        }
        if (Fishye.strengthY > _targetFishEye) {
            Fishye.strengthY  -= Time.deltaTime * 0.2f;
        } else {
             Fishye.strengthY = _targetFishEye;
        }
    }

    public void ShotHint(string text)
    {
        HintText.SetText(text);

        var objTransform = HintText.transform;
        objTransform.localPosition = new Vector3(0, -600, 0);
        Tween.MoveEntity(objTransform, objTransform.position + new Vector3(0, 300, 0), 0.2f, null);
    }

    public void HideHint()
    {
        var objTransform = HintText.transform;
        objTransform.localPosition = new Vector3(0, -300, 0);
        Tween.MoveEntity(objTransform, objTransform.position - new Vector3(0, 300, 0), 0.2f, null);
    }

    public void HideStartScreen() {
        Tween.MoveEntity(StartScreen, StartScreen.position + new Vector3(0, 1000, 0), 0.3f, null);
    }
}
