using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;
    
    [SerializeField]
    Vector3 _delta = new Vector3(0, 6, -5);
    
    [SerializeField]
    GameObject _player = null;
    
    void Start()
    {
        
    }

    void LateUpdate() // PlayerController 와 여기 둘다 update 에서 하면 순서 보장 X 그래서 덜덜거림
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude,
                    LayerMask.GetMask("Wall")))
            {
                // 카메라와 플레이어 사이에 물체가 있으면 카메라 앞당기기
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                // 플레이어 기준으로 특정 위치 - 플레이어 가져오는 방법은 태그로 가져올 수도 있고 드래그 드롭으로 가져올 수도 있음
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);     
            }
            
                   
        }
    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
