using System;
using System.Collections.Generic;

[Serializable]
public class LeaderData
{
    public int score;
    public string name;
}

[Serializable]
public class LeaderDataCollection
{
    public List<LeaderData> collection;
}