using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    private const string LEADERBOARD_KEY = "LEADERBOARD";

    public void AddLeader(LeaderData leader)
    {
        List<LeaderData> leaderboard = LoadLeaderboard();
        var founded = leaderboard.FirstOrDefault(x => x.name == leader.name);
        if (founded is not null)
        {
            if (leader.score > founded.score)
            {
                founded.score = leader.score;
            }
        }
        else
        {
            leaderboard.Add(leader);
        }
        SaveLeaderboard(leaderboard);
    }

    public List<LeaderData> GetTopLeaderboard(int count)
    {
        List<LeaderData> leaderboard = LoadLeaderboard()
            .OrderBy(x => -x.score)
            .ToList();

        if (leaderboard.Count <= count) return leaderboard;
        else return leaderboard.Take(count).ToList();
    }


    private void SaveLeaderboard(List<LeaderData> leaders)
    {
        string data = JsonUtility.ToJson(new LeaderDataCollection() { collection = leaders });
        PlayerPrefs.SetString(LEADERBOARD_KEY, data);
        PlayerPrefs.Save();
    }

    private List<LeaderData> LoadLeaderboard()
    {
        if (!PlayerPrefs.HasKey(LEADERBOARD_KEY)) return new();
        string data = PlayerPrefs.GetString(LEADERBOARD_KEY);
        if (string.IsNullOrEmpty(data)) return new();
        var list = JsonUtility.FromJson<LeaderDataCollection>(data);
        return list.collection;
    }
}