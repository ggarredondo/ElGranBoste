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
        playerStateMachine.JokingState.OnEnter += () => StartCoroutine(TypeLine(playerStateMachine.JokeList[playerStateMachine.SelectedJoke].Sentence,
                                                                                playerStateMachine.JokeList[playerStateMachine.SelectedJoke].TimeToPerform));

        playerStateMachine.JokingState.OnExit += () => { exit = true; dialogue.text = ""; };
    }

    private IEnumerator TypeLine(string currentText, float duration)
    {
        var text = currentText;

        exit = false;

        for (int i = 0; i < text.Length; i++)
        {
            dialogue.text += text[i];
            GameManager.Audio.Play(soundName);

            if (exit)
            {
                dialogue.text = "";
                yield break;
            }

            yield return new WaitForSecondsRealtime(duration / text.Length);
        }
    }
}
