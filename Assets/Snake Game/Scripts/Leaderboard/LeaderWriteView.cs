using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class LeaderWriteView : MonoBehaviour
{
    [SerializeField] private LeaderboardController _leaderboardController;
    [SerializeField] private GameController _gameController;

    [SerializeField] private TextMeshProUGUI _infoText;

    private string _name = "";

    private void OnEnable()
    {
        _infoText.text = $"you got {_gameController.Scores} scores";
    }

    public void OnChangedField(TMP_InputField _inputField)
    {
        var new_value = _inputField.text;
        if (new_value.Length > 20 || !Regex.IsMatch(new_value, "^[a-zA-Z]*$"))
        {
            _inputField.text = _name;
        }
        else
        {
            _name = new_value;
        }
    }

    public void SaveToLeaderboard()
    {
        _leaderboardController.AddLeader(new LeaderData { name = _name, score = _gameController.Scores });
    }
}