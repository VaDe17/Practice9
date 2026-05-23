using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    private Camera thisCamera;
    public Camera GetCamera => thisCamera;

    private TransitionCamera lastCameraPosition;
    private TransitionCamera currentCameraPosition;

    [SerializeField] private GameObject positionPrefab;
    [SerializeField] private Transform positionParent;

    [ContextMenu("Create Camera Position")]
    private void CreateCameraPosition()
    {
        GameObject newCamPos = Instantiate(positionPrefab, transform.position, Quaternion.identity, positionParent);

        newCamPos.GetComponent<CameraPosition>().CameraSize = GetComponent<Camera>().orthographicSize;
        newCamPos.GetComponent<CameraPosition>().TransitionDuration = 1.5f;
        newCamPos.GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<Camera>().orthographicSize/9*16 * 2, GetComponent<Camera>().orthographicSize * 2);
    }
    
    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }

    public void DoTransition(TransitionCamera transitionCamera)
    {
        StopAllCoroutines();
        StartCoroutine(StartTransition(transitionCamera));
    }

    public void CheckExit(TransitionCamera transitionCamera)
    {
        //DEBUG_LogCameraPositios();
        //Debug.Log("Trigger Exit");
        if(currentCameraPosition.Equals(transitionCamera) && !lastCameraPosition.Equals(default))
        {
            //Debug.Log("CameraNotNull");
            StopAllCoroutines();
            StartCoroutine(StartTransition(lastCameraPosition));
        }
    }

    private IEnumerator StartTransition(TransitionCamera transitionCamera)
    {
        if (currentCameraPosition.Equals(transitionCamera))
            yield return null;

        float elapsed = 0;
        
        lastCameraPosition = currentCameraPosition;
        currentCameraPosition = transitionCamera;

        while (elapsed < currentCameraPosition.TransitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / currentCameraPosition.TransitionDuration);

            transform.position = Vector3.Lerp(transform.position, (Vector3)currentCameraPosition.WorldPosition + new Vector3(0, 0, -10), t);
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, currentCameraPosition.CameraSize, t);
            yield return null;
        }
    }

    private void DEBUG_LogCameraPositios()
    {
        Debug.Log($"New Camera Position: {currentCameraPosition.WorldPosition} | Last Camera Position: {lastCameraPosition.WorldPosition}");
    }
}
