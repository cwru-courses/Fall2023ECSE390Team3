using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ConfigurableBindings: (Add more actions to bind here)
// Dash = 0, Ability = 1


public class InputRebind : MonoBehaviour
{
    [SerializeField] private InputActionAsset allInputs;
    [SerializeField] private InputActionReference actionRef;

    private TMP_Text bindBtnText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        bindBtnText = GetComponent<TMP_Text>();
        GetComponent<Button>().onClick.AddListener(OnStartBinding);
    }

    private void OnEnable()
    {
        bindBtnText.text = InputControlPath.ToHumanReadableString(actionRef.action.bindings[0].effectivePath);
    }

    public void OnStartBinding()
    {
        PlayerStats._instance.playerInput.SwitchCurrentActionMap("UI");
        PlayerMovement._instance.OnPause(true);

        bindBtnText.text = "Press Key To Bind";

        if (actionRef)
        {
            rebindingOperation = actionRef.action.PerformInteractiveRebinding()
                .WithCancelingThrough("<keyboard>/escape")
                .WithControlsHavingToMatchPath("<Keyboard>")
                .WithControlsExcluding("<keyboard>/w")
                .WithControlsExcluding("<keyboard>/a")
                .WithControlsExcluding("<keyboard>/s")
                .WithControlsExcluding("<keyboard>/d")
                .WithControlsExcluding("<keyboard>/upArrow")
                .WithControlsExcluding("<keyboard>/downArrow")
                .WithControlsExcluding("<keyboard>/leftArrow")
                .WithControlsExcluding("<keyboard>/rightArrow")
                .WithControlsExcluding("<keyboard>/anyKey");

            foreach (InputAction action in allInputs.actionMaps[0].actions)
            {
                rebindingOperation = rebindingOperation.WithControlsExcluding(action.bindings[0].effectivePath);
            }

            rebindingOperation = rebindingOperation
                .OnCancel(operation => OnRebindCompletion())
                .OnComplete(operation => OnRebindCompletion())
                .Start();
        }
    }

    private void OnRebindCompletion()
    {
        bindBtnText.text = InputControlPath.ToHumanReadableString(actionRef.action.bindings[0].effectivePath);

        rebindingOperation?.Dispose();

        PlayerStats._instance.playerInput.SwitchCurrentActionMap("Player");
        PlayerMovement._instance.OnPause(false);
    }
}
