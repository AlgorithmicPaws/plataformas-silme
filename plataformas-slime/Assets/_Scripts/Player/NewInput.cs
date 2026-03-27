
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInput : MonoBehaviour
{
    // Se declaran las variables
    private PlayerInput playerInput;
    [HideInInspector] public float inputX;
    private PlayerJump _playerJump;

    // Al inicio de juego
    private void Start()
    {
        // Se almacena en la variable el componente acorde de Unity
        playerInput = GetComponent<PlayerInput>();
        _playerJump = GetComponent<PlayerJump>();
    }

    // Cada frame
    private void Update()
    {
        // Se llama al m�todo para que funcione
        GetInput();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Tres momentos: cuando se presiona, cuando se est� presionando y cuando se deja de presionar
        if (context.started)
        {
            // Forma de escribir en consola
            _playerJump.Jump();
        }
    }

  
    public void GetInput()
    {
        // Almacena en la variable el varlor del axis del archvio de PlayerActions
        inputX = playerInput.actions["Move"].ReadValue<float>();

        // Forma con concatenaci�n
       // Debug.Log("Movimiento: " + inputX);

        // Forma con formato
       // Debug.Log(string.Format("Movimiento: {0}", inputX));
    }
}
