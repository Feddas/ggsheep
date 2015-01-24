using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Roam : MonoBehaviour
{
    public float Speed = 10;
    public float SecondsInDirection;

    private PlayerEnum touchingPlayer;
    private CharacterController cachedController;
    private float timeInThisDirection;
    private Vector3 currentDirection;

    void Start()
    {
        cachedController = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (touchingPlayer == PlayerEnum.None)
        {
            roam();
        }
    }

    private void roam()
    {
        timeInThisDirection += Time.deltaTime;
        if (timeInThisDirection > SecondsInDirection)
        {
            timeInThisDirection = 0;
            currentDirection = Random.insideUnitSphere * Speed;
        }
        cachedController.SimpleMove(currentDirection);
    }
}