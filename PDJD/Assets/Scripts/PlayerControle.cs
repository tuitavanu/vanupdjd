using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{
    public int coins = 0;

    public TMP_Text coinText;
    public float moveSpeed;
    public float maxVelocity;
    public float rayDistance;
    public LayerMask isgroundedLayer;
    public float JumpForce;
    
    private GameInput _GameInput;
    private PlayerInput _playerControle;
    private Camera _maincamera;
    private Rigidbody _rigidbody;
    private Vector2 _movement;
    
    private bool _isGrounded;

    private void OnEnable()
    {
        //inicializacao de variavel
        _GameInput = new GameInput();
        
        //Referencia dos componentes no nosso objeto da unity
        _playerControle = GetComponent<PlayerInput>();
        _rigidbody = GetComponent < Rigidbody>();
        
        //Referencia para a camera main guardada na clase Camera
        _maincamera = Camera.main;
        
        //delegate do action triggered no player input
        _playerControle.onActionTriggered += OnActionTriggered;
        

    }

    private void OnDisable()
    {
        _playerControle.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        //cpmecando o nome do action que esta chegando com o nome do action de moviment
        if(obj.action.name.CompareTo(_GameInput.gameplay.Movements.name) == 0)
        {
            //atribuir ao moveinput o valor proveniente ao input do jogador Vector3
            _movement = obj.ReadValue<Vector2>();
            

        }
        if (obj.action.name.CompareTo(_GameInput.gameplay.Jump.name) == 0)
        
          
            {
                if(obj.performed) Jump();
            }

    }

    private void Jump()
    {
        if (_isGrounded) _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(origin: transform.position, direction: Vector3.down, rayDistance, isgroundedLayer);
    }

    private void Update()
    {
        
        CheckGround();
    }

    private void Move()
    {
        
        Vector3 camForward = _maincamera.transform.forward;
        camForward.y = 0;
        // calcula o movimento no eixo da camera para o movimento frente/tras
        Vector3 moveVertical = camForward * _movement.y;

        Vector3 camRight = _maincamera.transform.right;
        camRight.y = 0;
        // calcula o movimento no eixo da camera para o movimento esquerda/direita
        Vector3 moverHorizontal = camRight * _movement.x;
        
        // adiciona a força no objeto atraves do rigidbory com intensidade definida por moverSpeed
        _rigidbody.AddForce((moveVertical + moverHorizontal) * moveSpeed * Time.fixedDeltaTime);
        
    }

    private void FixedUpdate()
    {
        Move();
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        // pegar a velocidade doplayer
        Vector3 velocity = _rigidbody.velocity;

        //checar se a velocidade está dentro dos limites nos diferentes eixos
        // limitando o eixo x usando ifs, abs e sign.
        if (Math.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;

        // maxVelocity < velocity.z < maxVelocity
        velocity.z = Mathf.Clamp(value: velocity.z, min:-maxVelocity, maxVelocity);

        //alterar a velocidade do player para ficar dentro dos limites
        _rigidbody.velocity = velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            coinText.text = coins.ToString();
            Destroy(other.gameObject);
        }
    }
}
        
    

   
