using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomAnimator
{
    private static readonly Dictionary<DirectionType, int> directionDictionary = new Dictionary<DirectionType, int>()
    {
        { DirectionType.Up, 0 },
        { DirectionType.Left, 1 },
        { DirectionType.Down, 2 },
        { DirectionType.Right, 3 },
    };

    public Action OnEndAnimation;

    private bool isPlaying;
    private int direction;
    private int currentFrame;
    private float frameDuration;
    private float framePerSecond;
    private float currentDuration;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int maxFrame;

    public CustomAnimator(string path, int framePerSecond, bool isDirectional)
    {
        sprites = Resources.LoadAll<Sprite>(path);
        frameDuration = 1.0f / framePerSecond;
        this.framePerSecond = framePerSecond;
        maxFrame = isDirectional ? (sprites.Length >> 2) : sprites.Length;
    }

    public void SetDirection(DirectionType directionType)
    {
        direction = directionDictionary[directionType];
    }

    public void SetPlaying(bool value)
    {
        if (isPlaying != value)
        {
            currentFrame = 0;
        }
        isPlaying = value;
    }

    public Sprite GetSprite(float deltaTime)
    {
        currentDuration += deltaTime;
        int addFrame = (int)(currentDuration * framePerSecond);
        if (addFrame > 0)
        {
            currentFrame += addFrame;
            currentFrame %= maxFrame;
            currentDuration -= addFrame * frameDuration;
        }
        currentFrame = (isPlaying) ? currentFrame : 0;
        return sprites[direction * maxFrame + currentFrame];
    }
}
