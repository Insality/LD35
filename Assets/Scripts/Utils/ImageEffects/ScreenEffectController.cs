using UnityEngine;

public class ScreenEffectController : MonoBehaviour
{
    [SerializeField] public ScreenShineEffect ShineEffect;

    public void ShineScreen(float intensity, float time)
    {
        ShineEffect.enabled = true;
        Tween.TweenFloat(f => ShineEffect.intensity = f, intensity, 0, time, EasingType.Linear, ()=> ShineEffect.enabled = false);
    }

    public void SlowShineScreen(float intensity, float time)
    {
        ShineEffect.enabled = true;
        Tween.TweenFloat(f => ShineEffect.intensity = f, 0, intensity, time/6f, EasingType.Linear, () =>
            Tween.TweenFloat(f => ShineEffect.intensity = f, intensity, 0, time, EasingType.Linear, () => ShineEffect.enabled = false));
    }

    public void RiseFromDark(float time)
    {
        ShineEffect.enabled = true;
        Tween.TweenFloat(f => ShineEffect.intensity = f, -0.9f, 0, time, EasingType.Linear, () => ShineEffect.enabled = false);
    }

    public void RiseToDark(float time)
    {
        ShineEffect.enabled = true;
        Tween.TweenFloat(f => ShineEffect.intensity = f, 0, -0.9f, time, EasingType.Linear, () => ShineEffect.enabled = false);
    }
}
