﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;
    public float runSpeed = 12f;
    public float gravityModifier;
    public float jumpPower;
    public CharacterController characterController;
    public Transform camTransform;

    public float mouseSensitivity = 2;
    public bool invertX;
    public bool invertY;

    private Vector3 moveInput;

    private bool canJump;
    private bool canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGrounded;

    public Animator animator;

    public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> guns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>(); 
    public int currentGunIndex;

    public Transform aimingPoint;
    public Transform gunHolder;
    private Vector3 gunStartPosition;
    public float aimingSpeed = 2f;

    public GameObject muzzleFlash; 

    void Awake()
    {
        instance = this;
    }

    void Start() 
    {
        SwitchGun();

        gunStartPosition = gunHolder.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // store y velocity
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = vertMove + horiMove;
        moveInput.Normalize();
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            moveInput = moveInput * runSpeed;
        } else 
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime; 
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGrounded).Length > 0;

        if (canJump)
        {
            canDoubleJump = false;
        }

        // Handle Jumping
        if (Input.GetKeyDown(KeyCode.Space) && canJump) 
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        } else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }

        characterController.Move(moveInput * Time.deltaTime);

        // controll camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + mouseInput.x,
            transform.rotation.eulerAngles.z
        );

        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        muzzleFlash.SetActive(false);

        // Handle shooting
        // Single shot
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f)) 
            {
                if (Vector3.Distance(camTransform.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            } else 
            {
                firePoint.LookAt(camTransform.position + (camTransform.forward * 30f));
            }
            //Instantiate(bullet, firePoint.position, firePoint.rotation);
            FireShot();
        }

        // Repeats shots
        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter <= 0)
            {
                FireShot();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.zoomIn(activeGun.zoomAmount);
        }

        if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.zoomOut();
        }

        // check if is right mouse button is held
        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, aimingPoint.position, aimingSpeed * Time.deltaTime);
        } 
        else {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPosition, aimingSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }

        animator.SetFloat("moveSpeed", moveInput.magnitude);
        animator.SetBool("onGround", canJump);
    }

    public void FireShot() 
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            muzzleFlash.SetActive(true);
        }
    }

    private void SwitchGun()
    {
        activeGun = guns[currentGunIndex];
        if (activeGun.gameObject.activeSelf)
        {
            activeGun.gameObject.SetActive(false);
            currentGunIndex++;
            if (currentGunIndex >= guns.Count)
            {
                currentGunIndex = 0;
            }
            activeGun = guns[currentGunIndex];
        }
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firePoint.position;
    }

    public void AddGun(string gunToAdd) 
    {
        Debug.Log("Adding " + gunToAdd);
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0) 
        {
            for (int i = 0; i < unlockableGuns.Count; i++) 
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;
                    guns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }
        }
        Debug.Log("gunUnlocked = " + gunUnlocked);
        if (gunUnlocked) {
            currentGunIndex = guns.Count - 2;
            SwitchGun();
        }
    }
}
