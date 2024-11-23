using UnityEngine;

public enum GuildLocationEventType
{
    None,
    QuestBoard,
    Chair,
}

public class EventLocation : MonoBehaviour
{
    public DirectionType Direction;
    public GuildLocationEventType EventType;
}