using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class FreezeOnA : MonoBehaviour
{
    public InputActionReference rightA;   // botón A (mano derecha)
    public InputActionReference leftX;    // botón X (mano izquierda)

    XRGrabInteractable grab;
    Rigidbody rb;

    InputActionReference currentButton;   // botón de la mano que lo sostiene
    bool freeze;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rightA.action.Enable();
        leftX.action.Enable();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentButton = args.interactorObject.handedness == InteractorHandedness.Left ? leftX : rightA;
        freeze = false; // al agarrarlo otra vez, se descongela
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentButton = null;
        StartCoroutine(ApplyAfterRelease());
    }

    void Update()
    {
        // Solo cuenta si lo estoy sosteniendo
        if (currentButton != null && currentButton.action.WasPressedThisFrame())
        {
            freeze = !freeze;
            Debug.Log(freeze ? "Se quedará flotando al soltar" : "Caerá al soltar");
        }
    }

    // XRI restaura el Rigidbody justo después de selectExited,
    // por eso aplicamos nuestro estado un momento después.
    IEnumerator ApplyAfterRelease()
    {
        yield return new WaitForEndOfFrame();

        if (freeze)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}