using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DialogueTreeUtilities;
using System.Net.NetworkInformation;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogue;

    private PlayerStateMachine playerStateMachine;

    private void Start()
    {
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();

        playerStateMachine.JokingState.OnEnter += () => dialogue.text = playerStateMachine.JokeList[playerStateMachine.SelectedJoke].Sentence;

        playerStateMachine.JokingState.OnExit += () => dialogue.text = "";
    }
}
