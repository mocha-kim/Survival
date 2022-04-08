using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField]
    private GameObject newGameWindow;
    [SerializeField]
    private GameObject loadGameWindow;
    [SerializeField]
    private GameObject exitGameWindow;

    private bool isWindowOpended = false;
    private GameObject openedWindow;

    private void Start()
    {
        newGameWindow.SetActive(false);
        loadGameWindow.SetActive(false);
        exitGameWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isWindowOpended)
            {
                if (openedWindow != null)
                {
                    openedWindow.SetActive(false);
                }
                isWindowOpended = false;
            }
            else
            {
                OnClickExitGame();
            }
        }
    }

    public void OnClickNewGame()
    {
        newGameWindow.SetActive(true);
        openedWindow = newGameWindow;
        isWindowOpended = true;
    }

    public void OnClickLoadGame()
    {
        loadGameWindow.SetActive(true);
        openedWindow = loadGameWindow;
        isWindowOpended = true;
    }

    public void OnClickExitGame()
    {
        exitGameWindow.SetActive(true);
        openedWindow = exitGameWindow;
        isWindowOpended = true;
    }
}
