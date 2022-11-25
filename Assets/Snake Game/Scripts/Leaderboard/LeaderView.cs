using System;
using TMPro;
using UnityEngine;

public class LeaderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textScore;

    public void WriteData(LeaderData leader, int index)
    {
        _textName.text = $"{index}. {leader.name} -";
        _textScore.text = leader.score.ToString();
    }

    public void WriteDefaultData(int index)
    {
        _textName.text = $"{index}. empty";
        _textScore.text = "";
    }
}