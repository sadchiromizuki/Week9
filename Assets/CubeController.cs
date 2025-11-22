using UnityEngine;

public class CubeController : MonoBehaviour
{
    public LineGen cube;
    public Platform platform;

    public float gravity = -9.8f;
    public float velocityY = 0f;
    public float jumpForce = 6f;

    private bool isGrounded;

    void Update()
    {
        HandleJump();
        ApplyGravity();
        ApplyMovement();
        DetectCollision();
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocityY = jumpForce;
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        velocityY += gravity * Time.deltaTime;
    }

    void ApplyMovement()
    {
        cube.cubePos.y += velocityY * Time.deltaTime;
    }

    void DetectCollision()
    {
        if (platform.IsColliding(cube))
        {
            cube.cubePos.y = platform.position.y + platform.size.y + cube.cubeSize;

            velocityY = 0f;
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
    }
}
