using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    #region VARIAVEIS
    [SerializeField] bool inate;

    [SerializeField] float posX;
    [SerializeField] float posY;
    [SerializeField] float posZ;

    [SerializeField] float rotX;
    [SerializeField] float rotY;
    [SerializeField] float rotZ;

    [SerializeField] float delay;

    Vector3 posIni;
    Vector3 rotIni;
    #endregion

    void Start()
    {
        posIni = transform.localPosition;
        rotIni = transform.localEulerAngles;

        if (inate) StartCoroutine(Routine());
    }

    public void Shake_Start(float _duration = -1f)
    {
        StartCoroutine(Routine());

        if (_duration != -1f) Invoke(nameof(Shake_Stop), _duration);
    }

    public void Shake_Stop()
    {
        StopAllCoroutines();
    }

    IEnumerator Routine()
    {
        while (true)
        {
            transform.localPosition = posIni + new Vector3(Random.Range(-posX, posX), Random.Range(-posY, posY), Random.Range(-posZ, posZ));
            transform.localEulerAngles = rotIni + new Vector3(Random.Range(-rotX, rotX), Random.Range(-rotY, rotY), Random.Range(-rotZ, rotZ));

            yield return new WaitForSeconds(delay);
        }
    }
}