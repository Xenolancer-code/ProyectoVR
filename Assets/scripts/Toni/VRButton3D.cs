using UnityEngine;
using System;

public class VRButton3D : MonoBehaviour
{
    [Header("Movimiento")]
    public float pressDistance = 0.02f; // Distancia que se hunde
    public float speed = 10f;           // Velocidad del lerp

    [Header("Acción")]
    public Action onPressAction;        // Acción que se ejecutará al presionar

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
    }

    void Update()
    {
        // Lerp suave hacia la posición objetivo
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * speed);
    }

    public void Press()
    {
        targetPos = startPos - new Vector3(0, 0, pressDistance); // Hacia abajo en local Y
        onPressAction?.Invoke(); // Ejecuta la acción asociada
    }

    public void Release()
    {
        targetPos = startPos; // Vuelve a la posición original
    }
}
