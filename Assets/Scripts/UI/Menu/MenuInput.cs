using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour
{
    public InputActionReference CloseAction;
    public Animator transitionWindow;
    private MenuStateManager stateManager => MenuStateManager.Instance;
    private MenuState currentState => stateManager.GetCurrentState;

    private void Update()
    {
        //if (CloseAction.action.WasPerformedThisFrame())
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if((currentState == MenuState.Settings || currentState == MenuState.Credits) && transitionWindow.GetBool("IsOpen"))
            {
                Debug.Log($"{currentState} => {MenuState.MainMenu}");
                stateManager.ChangeState(MenuState.MainMenu); 

                if (!transitionWindow.GetBool("IsOpen"))
                {
                    transitionWindow.SetBool("IsOpen", false);
                    transitionWindow.SetTrigger("Close");
                }
            }
        }
    }
}
