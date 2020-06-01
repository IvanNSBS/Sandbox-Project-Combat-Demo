using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class GlobalTurnManager : MonoBehaviour
{

    [SerializeField] private float _turnDuration = 3f; // time is in seconds
    [SerializeField] private Image _uiProgressBar;
    [SerializeField] private TMPro.TextMeshProUGUI _uiProgressTxt;
    public float turnDuration { get => _turnDuration; }
    public float nextTurnProgress { get => _currentTurnProgress / _turnDuration; } //percent remaining to next turn

    private float _currentTurnProgress = 0f;
    private List<IParticipant> _subscribers = new List<IParticipant>();

    /// <summary>
    /// Function to notify observers that a turn has passed and they can now perform actions
    /// </summary>
    public void TurnPassed()
    {
        foreach(IParticipant participant in _subscribers)
        {
            participant.OnTurnPassed();
        }
    }

    public void Subscribe(IParticipant participant)
    {
        _subscribers.Add(participant);
    }
    public bool Unsubscribe(IParticipant participant)
    {
        return _subscribers.Remove(participant);
    }

    public bool HasParticipant(IParticipant participant)
    {
        return _subscribers.Contains(participant);
    }
    void UpdateUI()
    {
        _uiProgressBar.fillAmount = nextTurnProgress;
        _uiProgressTxt.text = _currentTurnProgress.ToString("F1");
    }

    void Update()
    {
        _currentTurnProgress += Time.deltaTime;
        if (_currentTurnProgress >= _turnDuration)
        {
            TurnPassed();
            _currentTurnProgress = 0f;
        }
        UpdateUI();
    }
}
