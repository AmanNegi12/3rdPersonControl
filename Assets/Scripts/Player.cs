using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    enum PlayerAnimState{WithGun,WithoutGun}
    enum AimState {None,Aim}
    Vector3 movementInput;
    [SerializeField] Transform Camforawrd;
    [SerializeField] GameObject AimCamera;
    [SerializeField] GameObject crosshair;
    [SerializeField] Animator Anim;
    [SerializeField] Transform MainCamera;
    [SerializeField] Transform CamRotationTransform;
    [SerializeField] float PlayerMoveSpeed=5f;
    [SerializeField] float PlayerRotSpeed=5f;
    [SerializeField] float CameraRotSpeed=5f;
                     float Chorizontal;
                     float Cvertical;
                     float AnimSpeed;
                     float horizontal;
                     float vertical;
    [SerializeField] float yspeed;
    [SerializeField] float jump;
                     bool Isground;
                     bool camdedtection;
    Quaternion rot;
      
    [SerializeField] Vector3 Offset;

    [SerializeField] LayerMask Glayer;
    CharacterController characterController;
    PlayerAnimState PlayerState;
    AimState aimstate;
    [SerializeField]Rig rig;
    GunScript Gunsc;
  

    private void Awake()
    {
        PlayerState = PlayerAnimState.WithoutGun;
        aimstate = AimState.None;
        Cursor.visible = false;
        Cursor.lockState=CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        Anim = GetComponent<Animator>();
        Gunsc = GetComponentInChildren<GunScript>();

    }
    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        CamRotation();
        Gcheck();
        AnimCode();
        GunController();
        AimController();
        aimState();
    }

    void CharacterMove()
    {
         horizontal = Input.GetAxis("Horizontal");
         vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal,0f,vertical);
        movementInput.Normalize();



        if (movementInput.magnitude > 0f)
        {   
          movementInput = Quaternion.AngleAxis(MainCamera.rotation.eulerAngles.y, Vector3.up) * movementInput;
           rot= Quaternion.LookRotation(movementInput,Vector3.up);
          rot.Normalize();
          transform.rotation = Quaternion.RotateTowards(transform.localRotation,rot,PlayerRotSpeed*Time.deltaTime);
          
        }

        if (!Isground)
        {
            yspeed = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            yspeed = -10f * Time.deltaTime;
        }

        if (camdedtection==true)
        {
            Vector3 playerRot = MainCamera.eulerAngles;
            playerRot.x = 0;
            playerRot.z = 0;

            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(playerRot), 3000f * Time.deltaTime);

        }
        movementInput.y = yspeed;
        characterController.Move(movementInput*PlayerMoveSpeed*Time.deltaTime);
    }
    void CamRotation()
    {
        Chorizontal += Input.GetAxis("Mouse X") * CameraRotSpeed * Time.deltaTime;
        Cvertical += Input.GetAxis("Mouse Y") * CameraRotSpeed * Time.deltaTime;
        Cvertical = Mathf.Clamp(Cvertical,-20f,50f);
        Vector3 Rotater = new Vector3(Cvertical,Chorizontal,0f);
        CamRotationTransform.rotation = Quaternion.Euler(Rotater);
    }
   
    void Gcheck()
    {
        Isground = Physics.CheckSphere(transform.TransformPoint(Offset), 0.2f, Glayer);
    }
    void AnimCode()
    {
      

            if (movementInput.magnitude>0f && !Input.GetKey(KeyCode.LeftShift) || aimstate == AimState.Aim)
            {
                float CV = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                AnimSpeed = Mathf.Clamp01(CV);
                Anim.SetFloat("speed",AnimSpeed);
                PlayerMoveSpeed = 5f;
            }
            if (movementInput.magnitude>0f&&Input.GetKey(KeyCode.LeftShift)&&aimstate==AimState.None)
            {
                Anim.SetFloat("speed", 2, 0.1f, Time.deltaTime);
                PlayerMoveSpeed = 10f;

            }
        
        
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.TransformPoint(Offset), 0.2f);
    //}
    void GunController()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PlayerState==PlayerAnimState.WithoutGun)
            {
                PlayerState = PlayerAnimState.WithGun;
                Anim.SetBool("HasGun",true);
                 Anim.SetBool("Intran",true);
            }
            else
            {
                PlayerState = PlayerAnimState.WithoutGun;
                Anim.SetBool("HasGun",false);
                Anim.SetBool("Intran",true);

            }
        }
    }
    void InTransDisabler()
    {
       Anim.SetBool("Intran",false);

    }
    void AimController()
    {
        if (Input.GetKey(KeyCode.Mouse1) && PlayerState== PlayerAnimState.WithGun)
        {
            AimCamera.SetActive(true);
            crosshair.SetActive(true);
            camdedtection = true;
            aimstate=AimState.Aim;
            rig.weight = 1f;
           

        }
        else
        {
            AimCamera.SetActive(false);
            crosshair.SetActive(false);
            camdedtection = false;
            aimstate=AimState.None;
            rig.weight = 0f;

        }
        if (Input.GetKey(KeyCode.Mouse1)&&Input.GetKeyDown(KeyCode.Mouse0)&&PlayerState==PlayerAnimState.WithGun)
        {
            Gunsc.StartFiring();
          
        }
     
        if (Gunsc.Isfiring)
        {
            Gunsc.UpdateFiring(Time.deltaTime);
        }
        if ( Input.GetKeyUp(KeyCode.Mouse0)|| !Input.GetKey(KeyCode.Mouse1)&& PlayerState == PlayerAnimState.WithGun)
        {
            Gunsc.StopFiring();

        }

    }
    void aimState()
    {
        if (aimstate == AimState.None)
        {
            Anim.SetBool("Aim",false);
        }
        else
        {
            
            Anim.SetBool("Aim",true);
        }
    }
}
