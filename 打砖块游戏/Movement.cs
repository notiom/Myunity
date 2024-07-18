using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int speed = 3;
    void Start()
    {
        // transform.Translate(Vector3.left);
    }

    // Update is called once per frame
    void Update()
    {
        // 执行次数不确定
        Debug.Log("FPS : " + 1 / Time.deltaTime); // FPS
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h,v,0) * Time.deltaTime * speed); //速度 * 时间 = 距离
    }

    private void FixedUpdate()
    {
        // 每s执行的次数
        // 差不多每s调用50次
    }
}
