using System.Collections.Generic;

public class MainSceneManager : Singleton<MainSceneManager>
{
    private List<EventLocation> locationList;

    public EventLocation GetEmptyLocation()
    {
        return new EventLocation();
    }
}

