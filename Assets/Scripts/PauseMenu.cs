using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    private DefaultInputAction playerInputAction;

    private bool paused  = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Pause.started += OnPause;

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
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
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void MainMenuButton(){
        SceneManager.LoadSceneAsync("home_screen_scene");
    }
}
