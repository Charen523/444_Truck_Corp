using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class TileMapManager : Singleton<TileMapManager>
{
    [SerializeField] private TileMapData wallTileMap;
    [SerializeField] private TileMapData locationTileMap;
    [SerializeField] private Transform heroParent;
    [SerializeField] private GameObject entryPointObject;

    private Vector2Int doorPosition;
    private AStar astar;
    private Dictionary<int, HeroPresenter> heroes;
    private List<EventLocation> locations;

    protected override void Awake()
    {
        base.Awake();
        astar = new AStar();
        doorPosition = new Vector2Int((int)entryPointObject.transform.localPosition.x, -(int)entryPointObject.transform.localPosition.y);
        heroes = new Dictionary<int, HeroPresenter>();
    }

    private void Start()
    {
        InitializeEventLocations();
        astar.SetTiles(wallTileMap.Tiles);
    }

    private void InitializeEventLocations()
    {
        locations = FindObjectsByType<EventLocation>(FindObjectsSortMode.None).ToList();
    }

    public EventLocation GetEmptyLocation()
    {
        int count = locations.Count;
        if (count > 0)
        {
            int random = Random.Range(0, count);
            EventLocation location = locations[random];
            locations.RemoveAt(random);
            return location;
        }
        return null;
    }

    public void ReturnLocation(EventLocation location)
    {
        locations.Add(location);
    }

    public List<Vector2Int> GetRoute(Vector2Int start, Vector2Int end)
    {
        return astar.GetRouteMovementValue(start, end);
    }

    public void OnHeroEntered(HeroData heroData)
    {
        // 없다면 새로 지정
        if (!heroes.TryGetValue(heroData.id, out HeroPresenter hero))
        {
            hero = PoolManager.Instance.Get<HeroPresenter>("Prefabs/HeroPresenter", heroParent, entryPointObject.transform.localPosition);
            string path = DataManager.Instance.GetCharacterSheetPath(heroData.spriteIdx);
            hero.Initialize(path);
            heroes[heroData.id] = hero;
        }

        hero.gameObject.SetActive(true);
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