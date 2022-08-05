using CodeMonkey.Utils;
using LGamesDev.Core.CharacterRenderer;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class PlayerAutoWalk : MonoBehaviour
    {
        [SerializeField] private bool cameraPositionWithMouse;

        [SerializeField] private LayerMask platformsLayerMask;

        public float moveSpeed = 20f;
        private BoxCollider2D boxCollider2d;
        private PlayerHandler playerHandler;
        private Rigidbody2D rigidbody2d;

        private void Awake()
        {
            playerHandler = gameObject.GetComponent<PlayerHandler>();
            rigidbody2d = transform.GetComponent<Rigidbody2D>();
            boxCollider2d = transform.GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            /*Camera.main.gameObject.GetComponent<CameraFollow>().Setup(GetCameraPosition, () => 60f, true, true);

        playerHandler.PlayAnimMove(Vector2.right);*/
        }

        private void Update()
        {
            /*if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                float jumpVelocity = 100f;
                rigidbody2d.velocity += Vector2.up * jumpVelocity;
            }
        }

        //move forward
        HandleMovement();

        // Set Animations
        if (IsGrounded())
        {
            if (rigidbody2d.velocity.x == 0)
            {
                playerHandler.PlayAnimIdle();
            }
            else
            {
                playerHandler.PlayAnimMove(new Vector2(rigidbody2d.velocity.x, 0f));
            }
        }
        else
        {
            playerHandler.PlayAnimJump(rigidbody2d.velocity);
        }*/
        }

        private bool IsGrounded()
        {
            var raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down,
                1f, platformsLayerMask);
            return raycastHit2d.collider != null;
        }

        private void HandleMovement()
        {
            rigidbody2d.velocity = new Vector2(1f * moveSpeed, rigidbody2d.velocity.y);
        }

        private Vector3 GetCameraPosition()
        {
            if (cameraPositionWithMouse)
            {
                var mousePosition = UtilsClass.GetMouseWorldPosition();
                var playerToMouseDirection = mousePosition - transform.position;
                return transform.position + playerToMouseDirection * .3f;
            }

            return transform.position + new Vector3(30f, 35f);
        }
    }
}