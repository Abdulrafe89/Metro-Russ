using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 Offset = new Vector3(0f, 5.2f, -12f);

    private void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        Vector3 desiredPosition = target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position + Vector3.up * 2f - transform.position), Time.deltaTime * 4f);
    }
}
