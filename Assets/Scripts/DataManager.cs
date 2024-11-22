using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Data MainData;
    public Dictionary<int, Character> Characters;

    protected override void Awake()
    {
        base.Awake();
        Characters = new Dictionary<int, Character>();
    }
}

[Serializable]
public class Data
{
    public float Time;
    public int Gold;
}

[Serializable]
public class Status
{
    public int STR; // 근력
    public int DEX; // 민첩
    public int INT; // 지능
    public int CON; // 건강
    public int LUK; // 행운

    public Status(int str, int dex, int intel, int con, int luk)
    {
        STR = str;
        DEX = dex;
        INT = intel;
        CON = con;
        LUK = luk;
    }
}

public class EventLocation
{
    public DirectionType direction;
}

public enum DirectionType
{
    Left,
    Right,
    Up,
    Down,
}

public class CharacterManager
{

}

public class CharacterAI : Poolable
{
    public float MoveSpeed;
    public float MoveCooldown;
    public readonly float GridSize = 16.0f;
    public LayerMask obstacleLayer;

    [SerializeField] private Animator animator;
    [SerializeField] private bool isMoving;

    private void Start()
    {
        
    }

    private IEnumerator RandomMoveRoutine()
    {
        while (true)
        {
            if (!isMoving)
            {
                Vector2Int direction = GetRandomDirection();
            }
        }
    }

    private Vector2Int GetRandomDirection()
    {
        return Vector2Int.down;
    }
}

