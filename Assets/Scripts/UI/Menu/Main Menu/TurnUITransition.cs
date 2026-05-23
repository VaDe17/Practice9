using UnityEngine;

public class TurnUITransition : MonoBehaviour
{
    public static TurnUITransition Instance {get; private set;}
    [SerializeField] private RectTransform transitionWindow;
    private Animator animator => transitionWindow.GetComponent<Animator>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartTransition()
    {
        if (!animator.GetBool("IsOpen"))
        {
            Vector2 windowPos = transitionWindow.position;
            windowPos.y = GetComponent<RectTransform>().position.y;

            transitionWindow.position = windowPos;
            
            animator.SetTrigger("Open");
        }
    }
    public void CloseTransition()
    {
        if (animator.GetBool("IsOpen"))
        {
            animator.SetTrigger("Close");
        }
    }

    public void SetClosed()
    {
        animator.SetBool("IsOpen", false);
    }
    public void SetOpen()
    {
        animator.SetBool("IsOpen", true);
    }
}
