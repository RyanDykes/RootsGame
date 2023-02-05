using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance = null;
    [SerializeField] private Transform cameraPivot = null;
    [SerializeField] private Transform cameraRef = null;
    [SerializeField] private Transform cameraSurfaceTreeRef = null;
    [SerializeField] private Transform cameraSkillTreeRef = null;
    [SerializeField] private AnimationCurve transitionCurve = null;
    [SerializeField] private float cameraSpeed = 10f;

    private Coroutine transitionCoroutine = null;
    private bool inTransition = false;
    private bool inSkillTreeView = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cameraRef.position = cameraSurfaceTreeRef.position;
        cameraRef.rotation = cameraSurfaceTreeRef.rotation;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void RotateCamera(float dragAmount)
    {
        if (inTransition || inSkillTreeView)
            return;

        cameraPivot.RotateAround(Vector3.zero, Vector3.up, dragAmount * cameraSpeed * Time.deltaTime);
    }

    public void TransitionToSurfaceTree()
    {
        inSkillTreeView = false;
        PlayerSingleton.Instance.GamePaused = false;
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionCoroutine(cameraSurfaceTreeRef));
    }

    public void TransitionToSkillTree()
    {
        inSkillTreeView = true;
        PlayerSingleton.Instance.GamePaused = true;
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionCoroutine(cameraSkillTreeRef));
    }

    private IEnumerator TransitionCoroutine(Transform target)
    {
        inTransition = true;

        float time = 0f;
        float duration = 1f;
        Vector3 startPosition = cameraRef.position;
        Vector3 targetPosition = target.position;
        Quaternion startRotation = cameraRef.rotation;
        Quaternion targetRotation = target.rotation;

        while (time < duration)
        {
            float T = transitionCurve.Evaluate(time / duration);
            cameraRef.position = Vector3.Lerp(startPosition, targetPosition, T);
            cameraRef.rotation = Quaternion.Lerp(startRotation, targetRotation, T);

            time += Time.deltaTime;
            yield return null;
        }

        inTransition = false;
    }

}
