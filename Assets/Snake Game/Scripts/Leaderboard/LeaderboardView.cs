using System.Collections.Generic;
using UnityEngine;

class LeaderboardView : MonoBehaviour
{
    [SerializeField] private LeaderboardController _controller;
    [SerializeField] private List<LeaderView> _leaders;

    private void OnEnable()
    {
        var top = _controller.GetTopLeaderboard(_leaders.Count);
        for (int i = 0; i < _leaders.Count; i++)
        {
            if (i >= top.Count)
            {
                _leaders[i].WriteDefaultData(i + 1);
            }
            else
            {
                _leaders[i].WriteData(top[i], i + 1);
            }
        }
    }
}