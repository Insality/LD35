using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A Tweener for simple usage action in time (delayed actions, moving entities and easings)
/// Author: Insality
/// </summary>
public class Tween
{
    #region MoveEntity

    public static void MoveEntity(Transform objTransform, Vector3 targetPos, float time, Action onMoveFinished)
    {
        MoveEntity(objTransform, targetPos, time, onMoveFinished, Vector3.zero);
    }

    public static void MoveEntity(Transform objTransform, Vector3 targetPos, float time, Action onMoveFinished,
        Vector3 startVelocity)
    {
        var obj = AppController.GetInstance();
        obj.StartCoroutine(MoveEntityAsync(objTransform, targetPos, time, startVelocity, onMoveFinished));
    }

    private static IEnumerator MoveEntityAsync(Transform objTransform, Vector3 targetPos, float time,
        Vector3 startVelocity,
        Action onMoveFinished)
    {
        targetPos.z = objTransform.position.z;

        var curVelocity = startVelocity;

        var distance = (objTransform.position - targetPos).magnitude;
        var deltaPos = distance/time;
        while (distance > deltaPos*Time.deltaTime)
        {
            var direction = Vector3.ClampMagnitude(targetPos - objTransform.position, 1);
            direction += curVelocity;
            curVelocity *= 0.95f;
            objTransform.position += direction*Time.deltaTime*deltaPos;
            distance = (objTransform.position - targetPos).magnitude;
            yield return null;
        }
        objTransform.position = targetPos;
        if (onMoveFinished != null)
        {
            onMoveFinished();
        }
    }

    #endregion

    #region DelayAction

    public static void DelayAction(float seconds, Action onDelayAction)
    {
        AppController.GetInstance().StartCoroutine(DelayActionAsync(seconds, onDelayAction));
    }

    private static IEnumerator DelayActionAsync(float seconds, Action onDelayAction)
    {
        yield return new WaitForSeconds(seconds);
        onDelayAction();
    }

    #endregion


    public static void TweenFloat(Action<float> setter, float startValue, float target, float time, EasingType eType = EasingType.Linear,
        Action onTweenEndAction = null)
    {
            AppController.GetInstance()
                .StartCoroutine(TweenFloatAsync(setter, startValue, target, time, eType, onTweenEndAction));
    }

    private static IEnumerator TweenFloatAsync(Action<float> setter, float startValue, float target, float time, EasingType eType, 
        Action onTweenEndAction)
    {
        var easingFunc = Easing.GetEasingByType(eType);
        var startTime = Time.time;
        var endTime = startTime + time;
        var delta = target - startValue;

        while (Time.time < endTime)
        {
            var t = Time.time - startTime;
            setter((float) easingFunc(t, startValue, delta, time));
            yield return null;
        }

        setter(target);
        if (onTweenEndAction != null)
        {
            onTweenEndAction();
        }
    }
}