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

    [SerializeField] private bool isPlaying;
    [SerializeField] private bool isLooping;
    [SerializeField] private int direction;
    [SerializeField] private int currentFrame;
    [SerializeField] private float frameDuration;
    [SerializeField] private float framePerSecond;
    [SerializeField] private float currentDuration;
    [SerializeField] private Action onEndAnimation;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int maxFrame;

    public CustomAnimator(string path, int framePerSecond, bool isDirectional, bool isLooping, Action onEndAnimation)
    {
        this.framePerSecond = framePerSecond;
        this.isLooping = isLooping;
        this.onEndAnimation = onEndAnimation;
        frameDuration = 1.0f / framePerSecond;
        direction = 0;
        sprites = Resources.LoadAll<Sprite>(path);
        if (sprites == null) return;
        maxFrame = isDirectional ? (sprites.Length >> 2) : sprites.Length;
    }

    public void SetOnEndAnimation(Action action)
    {
        onEndAnimation = action;
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
            currentDuration = 0;
        }
        isPlaying = value;
    }

    public Sprite GetSprite(float deltaTime)
    {
        if (sprites == null) return null;

        currentDuration += deltaTime;
        int addFrame = (int)(currentDuration * framePerSecond);
        if (addFrame > 0)
        {
            currentFrame += addFrame;
            if (currentFrame >= maxFrame)
            {
                Debug.Log("호출");
                onEndAnimation?.Invoke();
                if (isLooping)
                {
                    currentFrame %= maxFrame;
                }
                else
                {
                    currentFrame = 0;
                    isPlaying = false;
                }
            }
            currentDuration -= addFrame * frameDuration;
        }
        currentFrame = (isPlaying) ? currentFrame : 0;
        //Debug.Log($"currentFrame : {currentFrame}\n 현재프레임 : {direction * maxFrame + currentFrame}\n 방향 : {direction} \n 최대프레임 : {maxFrame}");
        return sprites[direction * maxFrame + currentFrame];
    }
}