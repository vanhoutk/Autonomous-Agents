using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if(horizontal != 0 || vertical != 0)
        {
            GameObject camera = GameObject.Find("Main Camera");
            camera.transform.position = new Vector3(camera.transform.position.x + horizontal, camera.transform.position.y + vertical, -10);
        }
    }
}
