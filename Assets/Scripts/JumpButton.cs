using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour {
    public bool Undo = false;
    Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(Undo)
            {
                AudioController.Instance.ResetMusicBus();
            } else
            {
                AudioController.Instance.MusicLowPass();
            }
        }

        body.MovePosition(new Vector3(body.position.x, -.45f, body.position.z));

        StopAllCoroutines();
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 t = new Vector3(body.position.x, -.15f, body.position.z);
        while (body.position.y < -.15f)
        {
            transform.position = Vector3.Lerp(body.position, t, Time.deltaTime);
            yield return null;
        }
        transform.position = t;
    }
}
