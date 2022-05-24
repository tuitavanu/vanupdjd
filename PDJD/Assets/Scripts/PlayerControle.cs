using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{
    public float movespeed; 
    private GameInput _GameInput;
    private PlayerInput _playerControle;
    private Camera _maincamera;
    private Rigidbody _rigidbody;
    private Vector2 _movement;

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
        
        
    }

    private void Move()
        //calcule o movimento no eixo da camera para o movimrnto frente/tras
    {
        Vector3 moveVertical = _maincamera.transform.forward * _movement.y;
        
        //calcule o movimento no eixo  da camera para o movimento esquerda/direita
        Vector3 moveHorizontal = _maincamera.transform.right * _movement.x;
        
        //adicione a força no objeto atraves do rigidbody, com intensidade definida por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal) * movespeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
}