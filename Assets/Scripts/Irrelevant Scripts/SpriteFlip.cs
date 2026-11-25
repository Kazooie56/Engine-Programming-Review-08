using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    // mirrors the direction changing logic if false
    public bool artFacesLeft = true;

    // Sideways speed needed before the sprite flips. Prevents flickering when slowing down
    // or when moving straight toward/away from the camera."
    public float deadZone = 0.5f;

    public Transform cameraTransform; 
    private SpriteRenderer spriteRenderer;
    private CharacterController controller;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        if (controller == null || cameraTransform == null)
            return;

        // controller.velocity uses the CharacterController's built in information that stores exactly how fast you're moving
        Vector3 velocity = controller.velocity;
        // y is made zero so that jumping and falling don't count.
        velocity.y = 0f;

        // Turns the camera's velocity which is normally in world units, and converts it to local units.
        // x is how fast we move to the right/left
        float sideways = cameraTransform.InverseTransformDirection(velocity).x;

        // if she's moving to the right faster than our deadzone
        if (sideways > deadZone)
        {
            spriteRenderer.flipX = artFacesLeft;      // flip sprite
                                                      // ONLY WHILE BOOL IS ACTIVE
        }
        // if she's moving to the right faster than our deadzone
        else if (sideways < -deadZone)
        {
            spriteRenderer.flipX = !artFacesLeft;     // don't flip
        }
    }
}