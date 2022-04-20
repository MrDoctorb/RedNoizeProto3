using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancer : MonoBehaviour
{
    public float xVal = 1;
    public float yVal = 1;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;

        xVal = this.gameObject.transform.localScale.x / 1.84375f / 3f;
        yVal = this.gameObject.transform.localScale.y / 1.34840125f;

        material.mainTextureScale = new Vector2(Mathf.Max(transform.localScale.x,
            transform.localScale.z) / xVal, transform.localScale.y / yVal);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
