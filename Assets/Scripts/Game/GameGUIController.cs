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

    private float _targetAbberation = 0.2f;
    private float _targetFishEye = 0.3f;

    void Start()
    {
        GameController.GameStep += GuiBeat;
        PermanentShaker.Shake(99999f);
    }

    void GuiBeat(int i)
    {
        Vignette.chromaticAberration = i%4==0? -12 : -9;
    }

    public void SetPlayerHealth(int hp)
    {
        Vignette.intensity = 0.4f + (Constants.PlayerHp - hp)*0.08f;

        Fishye.strengthX = 0.4f;
        Fishye.strengthY = 0.4f;
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
            Fishye.strengthX  -= Time.deltaTime * 20;
        } else {
             Fishye.strengthX = _targetFishEye;
        }
        if (Fishye.strengthY > _targetFishEye) {
            Fishye.strengthY  -= Time.deltaTime * 2;
        } else {
             Fishye.strengthY = _targetFishEye;
        }
    }
}
