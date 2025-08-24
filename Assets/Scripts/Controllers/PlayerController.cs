using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    private bool _moveToDest = false;
    Vector3 _destPos;
    
    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard; // 두번 호출되지 않도록 이미 있는 것 정리
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        if (_moveToDest)
        {
            Vector3 dir = _destPos - transform.position;
            if (dir.magnitude < 0.0001f) // 크기 빼기하면 정확히 0 안나옴
            {
                _moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude); // 이동할 거리가 갈 방향보다 넘어가면 끝지점에서 바둥댐
                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            }
        }
        
        Animator animator = GetComponent<Animator>();
        if (_moveToDest)
        {
            animator.Play("RUN");    
        }
        else
        {
            animator.Play("WAIT");            
        }
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f); // slerp 값은 delta time 을 넣는 것이 권장됨
            transform.position += Vector3.forward * (Time.deltaTime * _speed); // position += transform.TransformDirection
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * (Time.deltaTime * _speed); // 고개를 돌리니까 앞으로
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * (Time.deltaTime * _speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * (Time.deltaTime * _speed);
        }
        _moveToDest = false;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        LayerMask mask = LayerMask.GetMask("Wall");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point;
            _moveToDest = true;
            Debug.Log($"Raycast Camera @ {hit.collider.gameObject.tag}");
        }
    }
}
