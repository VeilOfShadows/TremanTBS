using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float floorY;
    public float ceilingY;
    public float currentY;
    public float distance;
    public float speed;
    public float endY;
    public float buffer;
    public bool isMovingDown;
    public bool isMovingUp;
    public CameraController camSwapper;
    public int camIndex;

    void Update()
    {
        camIndex = camSwapper.IndexCount();

        //checking for forward scroll
        //if (controller.IndexCount() == 0 || controller.IndexCount() == 2)
        if (Input.mouseScrollDelta.y < 0)
        {
            //currentY = transform.position.y; - change refs of transform.position.y to currentY to revert change
            endY = transform.position.y - distance;
            buffer = endY + 0.005f;
            isMovingDown = true;
            isMovingUp = false;
            //if endY is within floorY +1 then set the endY to floorY -- to allow smooth transition into the lowest camera point
            if(endY <= floorY + 1f)
            {
                endY = floorY;
                buffer = endY + 0.005f;
            }
        }

        //checking for backward scroll
        if (Input.mouseScrollDelta.y > 0)
        {
            //currentY = transform.position.y; - change refs of transform.position.y to currentY to revert change
            endY = transform.position.y + distance;
            isMovingUp = true;
            isMovingDown = false;
            buffer = endY - 0.005f;
            //if endY is within floorY -1 then set the endY to floorY -- to allow smooth transition into the highest camera point
            if (endY >= ceilingY - 1f)
            {
                endY = ceilingY;
                buffer = endY - 0.005f;
            }
        }

        //checks if the currentY is > the floorY and if so set the position to floorY -- run every frame maybe?
        if (transform.position.y < floorY)
        {
            transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
        }

        //checks if the currentY is > the ceilingY and if so set the position to ceilingY -- run every frame maybe?
        if (transform.position.y > ceilingY)
        {
            transform.position = new Vector3(transform.position.x, ceilingY, transform.position.z);
        }

        if (isMovingUp || isMovingDown)
        {
            StartCoroutine(Zoom(endY));
            if (transform.position.y <= buffer && isMovingDown || transform.position.y >= buffer && isMovingUp)
            {
                StopCoroutine(Zoom(endY));
                isMovingUp = false;
                isMovingDown = false;
            }
        }
    }

    public IEnumerator Zoom(float endY)
    {
        //currentY = transform.position.y; - change refs of transform.position.y to currentY to revert change
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, endY, speed * Time.deltaTime), transform.position.z);
        yield return null;
    }
}
