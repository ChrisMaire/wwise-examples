using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour {
    public bool Undo = false;
    Rigidbody body;
    bool set = false;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	void OnCollisionEnter(Collision collision)
    {
        if(set)
        {
            return;
        }
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
        set = true;
        StopAllCoroutines();
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 t = new Vector3(body.position.x, -.145f, body.position.z);
        while (body.position.y < -.15f)
        {
            transform.position = Vector3.Lerp(body.position, t, Time.deltaTime);
            yield return null;
        }
        transform.position = t;
        set = false;
    }
}
