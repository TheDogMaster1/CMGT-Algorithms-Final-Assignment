using System.Collections.Generic;

public class DungeonGraph<T>
{
    private Dictionary<T, List<T>> roomGraph;

    public DungeonGraph()
    {
        roomGraph = new Dictionary<T, List<T>>();
    }
}
