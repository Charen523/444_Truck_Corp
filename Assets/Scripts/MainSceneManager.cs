using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    [SerializeField] private TileMapData wallTileMap;
    [SerializeField] private TileMapData locationTileMap;
    [SerializeField] private Transform heroParent;
    [SerializeField] private GameObject entryPointObject;

    private float time;
    private Vector2Int entryPoint;
    private List<Vector2Int> locations;
    private AStar astar;

    protected override void Awake()
    {
        base.Awake();
        astar = new AStar();
        entryPoint = new Vector2Int((int)entryPointObject.transform.localPosition.x, -(int)entryPointObject.transform.localPosition.y);
        locations = new List<Vector2Int>();
    }

    private void Start()
    {
        astar.SetTiles(wallTileMap.Tiles);
        locations = locationTileMap.GetTileVectorList();
        Debug.Log($"할당된 위치는 {locations.Count}개");
        string locationString = "";
        for (int index = 0; index < locations.Count; index++)
        {
            locationString += $"({locations[index].x}, {locations[index].y}), ";
        }
        Debug.Log($"{locationString}");
    }

    public Vector2Int GetEmptyLocation()
    {
        int count = locations.Count;
        if (count > 0)
        {
            int random = Random.Range(0, count);
            Vector2Int location = locations[random];
            locations.RemoveAt(random);
            return location;
        }
        return new Vector2Int(0, 0);
    }

    private void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.fixedDeltaTime;
            return;
        }

        time = 10.0f;
        var location = GetEmptyLocation();
        if (location.x == 0 && location.y == 0) return;

        var hero = PoolManager.Instance.Get<SampleHeroPresenter>("Prefabs/SampleHero", heroParent, entryPointObject.transform.localPosition);
        Debug.Log($"이동할 위치는 {location.x}, {location.y}");
        var route = astar.GetRouteMovementValue(entryPoint, location);
        string routeString = "";
        for (int index = 0; index < route.Count; index++)
        {
            routeString += $"{route[index].x}, {route[index].y} => ";
        }
        Debug.Log($"루트는 {routeString}");
        hero.SetMoveCommand(route);
    }
}