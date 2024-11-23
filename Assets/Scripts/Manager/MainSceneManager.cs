using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    [SerializeField] private TileMapData wallTileMap;
    [SerializeField] private TileMapData locationTileMap;
    [SerializeField] private Transform heroParent;
    [SerializeField] private GameObject entryPointObject;

    private Vector2Int doorPosition;
    private AStar astar;
    private List<Vector2Int> locations;
    private Dictionary<int, HeroPresenter> heroes;

    protected override void Awake()
    {
        base.Awake();
        astar = new AStar();
        doorPosition = new Vector2Int((int)entryPointObject.transform.localPosition.x, -(int)entryPointObject.transform.localPosition.y);
        locations = new List<Vector2Int>();
        heroes = new Dictionary<int, HeroPresenter>();
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

    //private void FixedUpdate()
    //{
    //    //Debug.Log($"이동할 위치는 {location.x}, {location.y}");
    //    //string routeString = "";
    //    //for (int index = 0; index < route.Count; index++)
    //    //{
    //    //    routeString += $"{route[index].x}, {route[index].y} => ";
    //    //}
    //    //Debug.Log($"루트는 {routeString}");
    //}

    public void OnHeroEntered(HeroData heroData)
    {
        // 없다면 새로 지정
        if (!heroes.TryGetValue(heroData.id, out HeroPresenter hero))
        {
            hero = PoolManager.Instance.Get<HeroPresenter>("Prefabs/HeroPresenter", heroParent, entryPointObject.transform.localPosition);
            string path = DataManager.Instance.GetCharacterSheetPath(heroData.spriteIdx);
            hero.Initialize(path, null);
            heroes[heroData.id] = hero;
        }

        hero.gameObject.SetActive(true);
        Vector2Int location = GetEmptyLocation();
        if (location.x == 0 && location.y == 0) return;
        List<Vector2Int> route = astar.GetRouteMovementValue(doorPosition, location);
        hero.SetMoveCommand(route);
    }

    public void OnHeroExit(HeroData heroData)
    {
        if (!heroes.TryGetValue(heroData.id, out HeroPresenter hero))
        {
            return;
        }
        List<Vector2Int> route = astar.GetRouteMovementValue(hero.Position, doorPosition);
        hero.SetMoveCommand(route, () => hero.gameObject.SetActive(false));
    }
}