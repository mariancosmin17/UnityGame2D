using UnityEngine;

public class PlayerMovement:MonoBehaviour

{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    [SerializeField] private AudioClip jumpSound;

    //ia referinte de la obiect
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //roteste player cand se misca stanga dreapta
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;

        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);       
        
        //seteaza param la animator
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump logic
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //jump height ajustabil
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reseteaza coyote counter cand e jos
                jumpCounter = extraJumps; //Reseteaza jump counter la extra jump value
            }
            else
                coyoteCounter -= Time.deltaTime; 
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        //Daca coyote counter este 0 sau mai putin si nu e pe perete si nu are extra jumps nu fa nimic

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //daca nu e pe jos si coyote counter mai amre decat 0 fa un jump normal
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //daca avem extra jumps atunci jump si scade counter-ul la jump
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter la 0 pt a evita dublu jump
            coyoteCounter = 0;
        }
    }


    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0,Vector2.down,0.1f,groundLayer);
        return raycastHit.collider!=null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
