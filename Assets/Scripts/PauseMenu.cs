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
        playerInputAction.UI.Pause.started += (InputAction.CallbackContext ctx) => { OnPause(); };

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        playerInputAction.UI.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.UI.Disable();
    }

    public void OnPause()
    {
        paused = !paused;

        TimeManager._instance.OnPause(paused);

        if (paused)
        {
            pauseMenuCanvas.SetActive(true);
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    public void MainMenuButton(){
        SceneManager.LoadSceneAsync("home_screen_scene");
    }
}
