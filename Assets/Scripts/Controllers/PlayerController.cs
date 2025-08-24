using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;
    
    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private float wait_run_ratio = 0;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    void UpdateDie()
    {
        // 아무것도 못함
    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f) // 크기 빼기하면 정확히 0 안나옴
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude); // 이동할 거리가 갈 방향보다 넘어가면 끝지점에서 바둥댐
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        
        // 애니메이션
        Animator animator = GetComponent<Animator>();
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        animator.SetFloat("wait_run_ratio", wait_run_ratio);
        animator.Play("WAIT_RUN");    
    }

    void UpdateIdle()
    {
        // 애니메이션
        Animator animator = GetComponent<Animator>();
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        animator.SetFloat("wait_run_ratio", wait_run_ratio);
        animator.Play("WAIT_RUN");
    }
    
    PlayerState _state = PlayerState.Idle;
    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        LayerMask mask = LayerMask.GetMask("Wall");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
