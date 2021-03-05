using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator anim;
    int curHP;
    int maxHP = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool canTP = true;
    public bool inWater = false;
    public bool isClimbing = false;
    int coins = 0;
    bool canHit = true;
    public GameObject blueGem;
    public GameObject greenGem;
    int gemCount = 0;
    public Inventory inventory;
    public SoudEffect soudEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHP = maxHP;
    }

    void Update()
    {

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soudEffect.PlayJumpSound();
        }

        Flip();


        if (inWater && !isClimbing)
        {

            anim.SetInteger("State", 4);
            isGrounded = false;

            if (Input.GetAxis("Horizontal") != 0)
                Flip();
        }

        else
        {
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
        }
    }


    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);

    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0 && canHit)
        {
            curHP = curHP + deltaHP;
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());          
        }
        if (deltaHP >= 0 && canHit)
        {
            curHP = curHP + deltaHP;        

        }
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        if (curHP <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
        
           
        
    }
    IEnumerator OnHit()
    {
        if (isHit)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        }

        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        }

        if (GetComponent<SpriteRenderer>().color.g == 1f)
        {
            StopCoroutine(OnHit());
            
        }
        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
            canHit = true;
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
        }

        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }

            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }


        if (collision.gameObject.tag == "Coin")
        {          
            Destroy(collision.gameObject);
            coins++;
            soudEffect.PlayCoinSound();
        }


        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
           // RecountHP(1);
            inventory.Add_hp();
        }

        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHP(-1);
        }

        if (collision.gameObject.tag == "GreenGem")
        {
            Destroy(collision.gameObject);
          //  StartCoroutine(SpeedBonus());
            inventory.Add_gg();
        }

        if (collision.gameObject.tag == "BlueGem")
        {
            Destroy(collision.gameObject);
         //   StartCoroutine(NoHit());
            inventory.Add_bg();
        }


    }


    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }

        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        isClimbing = false;
        if (collision.gameObject.tag == "Ladder")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
        {
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        }
        if (collision.gameObject.tag == "Sand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }
    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.2f);
        an.SetBool("isJump", false);
    }

    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);
        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(InviseForGem(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;
        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
    }


    IEnumerator NoHit()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);
        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(InviseForGem(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;
        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }

    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);

        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }

    }

    IEnumerator InviseForGem(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
        {
            StartCoroutine(InviseForGem(spr, time));
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Sand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }


    public int GetCoins()
    {
        return coins;
    }

    public int GetHP()
    {
        return curHP;
    }
    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }

}













