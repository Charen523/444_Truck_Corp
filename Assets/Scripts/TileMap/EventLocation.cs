using UnityEngine;

public enum GuildLocationEventType
{
    None,
    QuestBoard,
}

public class EventLocation : MonoBehaviour
{
    [SerializeField] private DirectionType direction;
    [SerializeField] private GuildLocationEventType eventType;
}