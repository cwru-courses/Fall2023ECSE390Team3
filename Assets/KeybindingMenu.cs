using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// ConfigurableBindings: (Add more actions to bind here)
// Dash = 0, Ability = 1


public class KeybindingMenu : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private InputActionReference dashActionRfs;
    [SerializeField] private TMP_Text dashBindBtnText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        dashBindBtnText.text = InputControlPath.ToHumanReadableString(dashActionRfs.action.bindings[0].effectivePath);
    }

    public void OnStartBinding(int actionToBind)
    {
        PlayerStats._instance.playerInput.SwitchCurrentActionMap("UI");
        PlayerMovement._instance.OnPause(true);

        InputActionReference rfs = null;
        switch (actionToBind)
        {
            case 0:
                dashBindBtnText.text = "Press Key To Bind";
                rfs = dashActionRfs;
                break;
        }

        if (rfs) rebindingOperation = rfs.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnComplete(operation => OnRebindCompletion())
            .Start();
    }

    private void OnRebindCompletion()
    {
        dashBindBtnText.text = InputControlPath.ToHumanReadableString(dashActionRfs.action.bindings[0].effectivePath);

        rebindingOperation.Dispose();

        PlayerStats._instance.playerInput.SwitchCurrentActionMap("Player");
        PlayerMovement._instance.OnPause(false);
    }
}
