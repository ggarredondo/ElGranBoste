using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DialogueTreeUtilities;
using System.Net.NetworkInformation;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private float betweeCharsTime;

    [Header("Sounds")]
    [SerializeField] private string soundName;

    private bool exit;

    private void Start()
    {
        playerStateMachine.JokingState.OnEnter += () => dialogue.text = playerStateMachine.JokeList[playerStateMachine.SelectedJoke].Sentence;

        playerStateMachine.JokingState.OnExit += () => dialogue.text = "";
    }
}
