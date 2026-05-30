using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DialogueRunner))]
public class DialogKeyAdvance : MonoBehaviour
{
    // Drag and drop the DialogueRunner component here in the Inspector
    private DialogueRunner dialogueRunner;
    private bool wasDialogueRunning;
    private float ignoreAdvanceUntil;

    [SerializeField]
    private float advanceDelayAfterStart = 0.15f;

    void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }

    void Update()
    {
        if (dialogueRunner == null)
        {
            return;
        }

        bool isDialogueRunning = dialogueRunner.IsDialogueRunning;

        if (isDialogueRunning && !wasDialogueRunning)
        {
            ignoreAdvanceUntil = Time.unscaledTime + advanceDelayAfterStart;
        }

        wasDialogueRunning = isDialogueRunning;

        if (!isDialogueRunning || Time.unscaledTime < ignoreAdvanceUntil)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) || Keyboard.current != null && Keyboard.current[Key.E].wasPressedThisFrame)
        {
            dialogueRunner.RequestNextLine();
        }
    }
}
