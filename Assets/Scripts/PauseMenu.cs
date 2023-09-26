using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    private DefaultInputAction playerInputAction;
    public PlayerInput playerInput;

    private bool paused  = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new DefaultInputAction();

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    void LateUpdate(){
        Debug.Log("Pause");
        paused = !paused;
        if(Input.GetKeyDown(KeyCode.Escape))
            if (paused)
            {
                pauseMenuCanvas.SetActive(true);
                Time.timeScale = 0f;
                playerInputAction.Disable();
            }
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("qwe");
        }
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pause");
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            playerInput.SwitchCurrentActionMap("UI");
            pauseMenuCanvas.SetActive(true);
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            Debug.Log("Game Paused");
            Time.timeScale = 0f;
            playerInputAction.Disable();
        }
        else
        {
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap("Player");
            pauseMenuCanvas.SetActive(false);
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            Debug.Log("Game Unpaused");
            // pauseMenuCanvas.SetActive(false);
            // Time.timeScale = 1f;
            // Debug.Log("qwe");
            // playerInputAction.Enable();
        }
    }

    public void ResumeGame(){
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        playerInputAction.Enable();
        Debug.Log("qwe");
    }

    public void MainMenuButton(){
        SceneManager.LoadSceneAsync("home_screen_scene");
    }
}
