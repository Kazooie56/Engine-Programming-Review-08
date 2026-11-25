using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    // PUT THIS ON ANY IMAGE / SPRITE THAT NEEDS TO ALWAYS FACE THE SCREEN
    public Transform cameraTransform;

    private void LateUpdate()
    {
        if (cameraTransform == null)
        {
            return;
        }

        // if the camera is tilted 20 degrees downward, so will whatever this is attached to.
        // if the camera is rotated 30 degrees to the right, so is whatever this is attached to.
        transform.rotation = cameraTransform.rotation;
    }
}