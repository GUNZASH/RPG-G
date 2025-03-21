using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ตัวละครที่ให้กล้องติดตาม
    public float smoothSpeed = 5f; // ความเร็วในการติดตาม
    public Vector3 offset; // ระยะห่างจากตัวละคร

    void LateUpdate()
    {
        if (target == null) return; // ถ้าไม่มีเป้าหมายให้หยุดทำงาน

        // ตำแหน่งที่ต้องการให้กล้องไป
        Vector3 desiredPosition = target.position + offset;

        // ใช้ Lerp ให้กล้องเคลื่อนที่อย่างนุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}