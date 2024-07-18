using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject bulletprefab;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //0判断左键是否按下，1判断右键是否按下
        {
            // 根据prefabs创建实例 也可以根据游戏物体进行克隆
            GameObject bullet = GameObject.Instantiate(bulletprefab,transform.position,transform.rotation);
            Rigidbody rd = bullet.GetComponent<Rigidbody>();
            // rd.AddForce(Vector3.forward * 100); 施加力的方式没法体现在速度上
            rd.linearVelocity = Vector3.forward * 30;
        }
    }
}
