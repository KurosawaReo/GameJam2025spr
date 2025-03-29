using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UiAnimationManager : MonoBehaviour
{
    [SerializeField] private Renderer target;
    [SerializeField] private float cycle = 1;

    public Material material;
    public double time;

    public float ySpeed;

    // Start is called before the first frame update
    public void Start()
    {
        ySpeed = 1f;
        Material mat = this.GetComponent<Renderer>().material;
        mat.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector2(0f, ySpeed);

        var repeatValue = Mathf.Repeat((float)time, cycle);

        var color = material.color;
        color.a = repeatValue >= cycle * 0.5f ? 1 : 0;
        material.color = color;
    }

    private void Awake()
    {
        material = target.material;
    }
    private void OnDestroy()
    {
        Destroy(material);
    }
}
