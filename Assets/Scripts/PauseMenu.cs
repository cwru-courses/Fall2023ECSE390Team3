using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    //private DefaultInputAction playerInputAction;

    private bool paused  = false;

    // Start is called before the first frame update
    void Awake()
    {
        //playerInputAction = new DefaultInputAction();
        //playerInputAction.UI.Pause.started += (InputAction.CallbackContext ctx) => { OnPause(); };

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    //private void OnEnable()
    //{
    //    playerInputAction.UI.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerInputAction.UI.Disable();
    //}

    public void OnPause()
    {
        Debug.Log(1);
        paused = !paused;

        PlayerStats._instance.playerInput.SwitchCurrentActionMap(paused ? "UI" : "Player");
        PlayerMovement._instance.OnPause(paused);
        //PlayerAttack._instance.OnPause(paused);
        //PhaseShift._instance.OnPause(paused);
        //PlayerStats._instance.OnPause(paused);

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
