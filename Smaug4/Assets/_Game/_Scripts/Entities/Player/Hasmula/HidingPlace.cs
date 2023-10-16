using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    #region Global variables
    [SerializeField] private bool playerColliding = false;
    [SerializeField] private bool keyPressed = false;
    [SerializeField] public static bool isHidden = false;
    private GameObject player;

    //Components
    [HideInInspector] public PlayerMagroMov _playerMagroMov;
    private Rigidbody2D _playerRb;
    private Renderer _playerRenderer;
    private Animator _animator;
    private PlayerGameOver _playerGameOver;

    private Vector2 _exitPosition;
    #endregion

    #region Unity Functions
    void Start()
    {
        //Puxando muitos componentes do player
        isHidden = false;
        player = GameObject.FindWithTag("Player");
        _playerRenderer = player.GetComponent<Renderer>();
        _playerMagroMov = player.GetComponent<PlayerMagroMov>();
        _playerRb = player.GetComponent<Rigidbody2D>();
        _animator = player.GetComponent<Animator>();
        _playerGameOver = player.GetComponent<PlayerGameOver>();

        //Posi��o que o player vai sair do objeto
        _exitPosition = new Vector2(this.transform.position.x, this.transform.position.y - 0.76f);
    }

    void Update()
    {
        if (_playerGameOver.GameEnded) return;
        
        //detecta quando aperta o espa�o
        if (Input.GetKeyDown(KeyCode.Space))
            keyPressed = true;
        else
            keyPressed = false;

        HidePlayer();
        ShowPlayer();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //caso entre na �rea do arm�rio
        if (collision.gameObject.CompareTag("Player"))
        {
            playerColliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //cas� saia da �rea do arm�rio
        if (collision.gameObject.CompareTag("Player"))
        {
            playerColliding = false;
        }
    }
    #endregion

    #region Personal Functions
    public void HidePlayer()
    {
        //some o player
        if (playerColliding == true && keyPressed == true && isHidden == false)
        {
            _playerRenderer.enabled = false;
            isHidden = true;
            keyPressed = false;
            _playerRb.constraints = RigidbodyConstraints2D.FreezePosition;
            _playerMagroMov.CanMove = false;
            _animator.SetFloat("Horizontal", 0f);
            _animator.SetFloat("Vertical", 0f);
            _animator.SetBool("IsWalking", false);
        }
    }

    public void ShowPlayer()
    {
        if (playerColliding == true && keyPressed == true && isHidden == true)
        {
            _playerRenderer.enabled = true;
            isHidden = false;
            keyPressed = false;
            player.transform.position = _exitPosition;
            _playerMagroMov.CanMove = true;
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _animator.SetFloat("Horizontal", 0f);
            _animator.SetFloat("Vertical", 0f);
            _animator.SetBool("IsWalking", false);
        }
    }
    #endregion
}
