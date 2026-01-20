using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 180f;

    public TextMeshProUGUI textObject;

    void Update()
    {
        float forward = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) forward += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) forward -= 1f;

        float turn = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) turn -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) turn += 1f;

        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider planet)
    {
        if (textObject)
        {
            textObject.gameObject.SetActive(true);
            textObject.text = planet.gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (textObject)
        {
            textObject.gameObject.SetActive(false);
            textObject.text = "";
        }
    }
}