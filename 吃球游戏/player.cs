using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    // 定义属性
    public Rigidbody rd;
    private string[] direction = new string[]{"right","left","forward","back","up","down"};

    public int score = 0; // 得分

    public Text scoreText;

    public GameObject winText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 文件名和类名要保持一致
    void Start()
    {
        // Debug.Log("游戏开始了!");
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("游戏正在运行!");
        // 施加一个向右的力
        // rd.AddForce(new Vector3(0,10,0));
        float h = Input.GetAxis("Horizontal"); // 返回-1 和 1 但是是渐变的过程
        float v = Input.GetAxis("Vertical"); // 返回-1 和 1 
        // Debug.Log("h = " + h); 
        rd.AddForce(new Vector3(h,0,v));
    }

    // 碰撞检测

    /*
    private void OnCollisionEnter(Collision collision)
    {
        // 当发生碰撞时，会执行的代码块
        // Collision对象保存碰撞信息
        Debug.Log("发生碰撞了!");
        // 游戏标签
        if(collision.gameObject.tag == "Food")
        {
            // 销毁游戏物体
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionExit(Collision other)
    {
        // 碰撞结束后执行的代码
    }

    void OnCollisionStay(Collision other)
    {
        // 触发检测的代码区域
    }

    */

    void OnTriggerEnter(Collider other)
    {
        // 当进入触发区域时，会执行的代码块
        Debug.Log("OnTriggerEnter " + other.tag);
        // 游戏标签
        if(other.gameObject.tag == "Food")
        {
            // 销毁游戏物体
            Destroy(other.gameObject);
            score++;

            scoreText.text = "分数: " + score;

            if(score == 12)
            {
                winText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        
    }

}
