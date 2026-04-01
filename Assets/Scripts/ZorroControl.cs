using UnityEngine;
using UnityEngine.InputSystem;

public class ZorroControl : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float velocidadCorrer = 10f;
    public float fuerzaSalto = 6f;
    public float velocidadRotacion = 720f;

    [Header("Sincronizacion de Salto")]
    public float retrasoParaImpulso = 0.15f; 
    
    private Rigidbody rb;
    private Animator anim;
    private Vector2 direccionInput;
    private bool estaEnElSuelo;
    private bool saltando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void OnMove(InputValue value)
    {
        direccionInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (estaEnElSuelo && !saltando)
        {
            saltando = true;
            estaEnElSuelo = false;
            
            anim.SetBool("isRunning", false);
            
            anim.ResetTrigger("jump");
            anim.SetTrigger("jump"); 

            Invoke(nameof(EjecutarImpulsoFisico), retrasoParaImpulso);
        }
    }

    void EjecutarImpulsoFisico()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    [Header("Interacciones y Sonido")]
    public AudioSource audioSourceLocal;
    public AudioClip sonidoRisa;
    
    public void OnLaugh()
    {
        Reir();
    }

    void Reir()
    {
        anim.SetTrigger("laugh");
        
        if (audioSourceLocal != null && sonidoRisa != null)
        {
            audioSourceLocal.PlayOneShot(sonidoRisa);
        }
    }

    void FixedUpdate()
    {
        MoverYRotar();
    }

    void MoverYRotar()
    {
        Vector3 movimiento = new Vector3(direccionInput.x, 0, direccionInput.y).normalized;

        if (movimiento.magnitude >= 0.1f)
        {
            Vector3 nuevaPosicion = rb.position + movimiento * velocidadCorrer * Time.fixedDeltaTime;
            
            rb.MovePosition(nuevaPosicion);

            Quaternion rotacionObjetivo = Quaternion.LookRotation(movimiento);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime));
            
            if (estaEnElSuelo && !saltando)
            {
                anim.SetBool("isRunning", true);
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.y > 0.1f || collision.gameObject.name.Contains("Suelo") || collision.gameObject.CompareTag("Suelo"))
        {
            estaEnElSuelo = true;
            saltando = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coleccionable"))
        {
            other.gameObject.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.RecogerObjeto();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        estaEnElSuelo = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        estaEnElSuelo = false;
    }
}