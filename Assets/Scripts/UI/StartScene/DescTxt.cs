using System.Collections;
using TMPro;
using UnityEngine;

public class DescTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descTxt;

    private float blinkDuration = 1f;
    private int minAlpha = 0;
    private int maxAlpha = 1; 

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        float elapsed = 0f;
        bool increasing = true;

        while (true)
        {
            float alpha = Mathf.Lerp(
                increasing ? minAlpha : maxAlpha,
                increasing ? maxAlpha : minAlpha,
                elapsed / blinkDuration
            );

            Color color = descTxt.color;
            color.a = alpha;
            descTxt.color = color;

            elapsed += Time.deltaTime;

            if (elapsed >= blinkDuration)
            {
                elapsed = 0f;
                increasing = !increasing;
            }

            yield return null;
        }
    }
}
