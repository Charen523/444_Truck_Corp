using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomContentSizeFitter : MonoBehaviour
{
    public enum FitMode { Unconstrained, PrefferedSize };
    public FitMode HorizontalFit;
    public FitMode VerticalFit;
    public float HorizontalPadding;
    public float VerticalPadding;
    private RectTransform rectTransform;

    [SerializeField] private RectTransform targetRect;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorApplication.delayCall += UpdateFit;
    }
#endif

    public void UpdateFit()
    {
        if (this == null) return;
        if (HorizontalFit == FitMode.PrefferedSize) UpdateFitting(RectTransform.Axis.Horizontal);
        if (VerticalFit == FitMode.PrefferedSize) UpdateFitting(RectTransform.Axis.Horizontal);
    }

    private void UpdateFitting(RectTransform.Axis axis)
    {
        rectTransform = rectTransform != null ? rectTransform : (RectTransform)transform;
        float size = Mathf.Ceil(LayoutUtility.GetPreferredSize(targetRect, (int)axis) - 0.4f + HorizontalPadding);
        rectTransform.SetSizeWithCurrentAnchors(axis, size);
    }
}