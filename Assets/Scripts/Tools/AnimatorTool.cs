using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorTool
{
    private static Dictionary<GameObject, (Coroutine coroutine, Vector2 restingPos)> _activeCoroutines = new();

    public static void JumpObject(GameObject go, int jumpMagnitude, float duration)
    {
        Debug.Log("Jump object called on " + go.name);
        RectTransform rt = go.GetComponent<RectTransform>();
        MonoBehaviour mb = go.GetComponent<MonoBehaviour>();

        if (_activeCoroutines.TryGetValue(go, out var existing))
        {
            mb.StopCoroutine(existing.coroutine);
            rt.anchoredPosition = existing.restingPos;
            _activeCoroutines.Remove(go);
        }

        Vector2 startPos = rt.anchoredPosition;
        float targetY = startPos.y + (rt.rect.height * (jumpMagnitude / 100f));
        Vector2 topPos = new Vector2(startPos.x, targetY);

        Coroutine c = mb.StartCoroutine(BounceCoroutine(rt, startPos, topPos, duration));
        _activeCoroutines[go] = (c, startPos);
    }

    private static IEnumerator BounceCoroutine(RectTransform rt, Vector2 startPos, Vector2 topPos, float duration)
    {
        float riseDuration = duration * 0.3f;
        float fallDuration = duration * 0.7f;
        float elapsed = 0f;

        // Phase 1: linear up
        while (elapsed < riseDuration)
        {
            float t = elapsed / riseDuration;
            rt.anchoredPosition = Vector2.Lerp(startPos, topPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rt.anchoredPosition = topPos;

        // Phase 2: ease out down
        elapsed = 0f;
        while (elapsed < fallDuration)
        {
            float t = elapsed / fallDuration;
            float eased = 1f - Mathf.Pow(1f - t, 2);
            rt.anchoredPosition = Vector2.Lerp(topPos, startPos, eased);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = startPos;
        _activeCoroutines.Remove(rt.gameObject);
    }
}