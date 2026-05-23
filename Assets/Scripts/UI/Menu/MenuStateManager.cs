using UnityEngine;
using UnityEngine.UI;

public class MenuStateManager : MonoBehaviour
{
    public static MenuStateManager Instance { get; private set; }

    private TurnUITransition transitionWindow => TurnUITransition.Instance;
    private MenuState currentState;
    public MenuState GetCurrentState => currentState;

    [Header("Windows")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject creditsWindow;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;

    private void Awake()
    {
        Instance = this;
        currentState = MenuState.MainMenu;
    }
    private void OnEnable()
    {
        settingsButton.onClick.AddListener(() => ChangeState(MenuState.Settings));
        creditsButton.onClick.AddListener(() => ChangeState(MenuState.Credits));
    }
    private void OnDisable()
    {
        settingsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(() => ChangeState(MenuState.Settings));
    }

    private void Update()
    {
        DoCurrentState();
    }

    public void ChangeState(MenuState newState)
    {
        ExitCurrentState();

        currentState = newState;

        EnterCurrentState();
    }

    private void EnterCurrentState()
    {
        switch (currentState)
        {
            case MenuState.MainMenu:
                MainMenuStateEnter();
                break;
            case MenuState.Settings:
                SettingsStateEnter();
                break;
            case MenuState.Credits:
                CreditsStateEnter();
                break;
            default:
                return;
        }
    }
    private void DoCurrentState()
    {
        switch (currentState)
        {
            case MenuState.MainMenu:
                MainMenuStateDo();
                break;
            case MenuState.Settings:
                SettingsStateDo();
                break;
            case MenuState.Credits:
                CreditsStateDo();
                break;
            default:
                return;
        }
    }

    private void ExitCurrentState()
    {
        switch (currentState)
        {
            case MenuState.MainMenu:
                MainMenuStateExit();
                break;
            case MenuState.Settings:
                SettingsStateExit();
                break;
            case MenuState.Credits:
                CreditsStateExit();
                break;
            default:
                return;
        }
    }

    private void MainMenuStateEnter()
    {
        newGameButton.interactable = true;
        ContinueButton.interactable = true;
        settingsButton.interactable = true;
        creditsButton.interactable = true;
    }
    private void MainMenuStateDo()
    {
        
    }
    private void MainMenuStateExit()
    {
        newGameButton.interactable = false;
        ContinueButton.interactable = false;
        settingsButton.interactable = false;
        creditsButton.interactable = false;
    }

    private void SettingsStateEnter()
    {
        //settingsWindow.SetActive(true);
        transitionWindow.StartTransition();
    }
    private void SettingsStateDo()
    {
        
    }
    private void SettingsStateExit()
    {
        transitionWindow.CloseTransition();
    }

    private void CreditsStateEnter()
    {
        transitionWindow.StartTransition();
    }
    private void CreditsStateDo()
    {
        
    }
    private void CreditsStateExit()
    {
        transitionWindow.CloseTransition();
    }
}
