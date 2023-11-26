using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerCharacter : MonoBehaviour
{
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Slider _healthBarSmall;
    [SerializeField] private Slider _healthBarBig;
    [SerializeField] private Animator _animator;
    private ManagerJS _mngJoyStick;

    [SerializeField] private int _rotationSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _fillSpeed;

    private int movementId = Animator.StringToHash("MovementSpeed");
    private int kickId = Animator.StringToHash("Kick");
    private int danceId = Animator.StringToHash("Dance");

    private float _calculatedSpeed = 0;
    private float _horizontalInput;
    private float _verticalInput;

    private float _healthPercents = 1;
    void Start()
    {
        _mngJoyStick = GameObject.Find("JSBackground").GetComponent<ManagerJS>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = _mngJoyStick.inputHorizontal();
        _verticalInput = _mngJoyStick.inputVertical();

        Vector3 movement = new Vector3(_horizontalInput, 0, _verticalInput);
        movement.Normalize();
        
        float new_CalculatedSpeed = 0;
        if (movement != Vector3.zero)
        {
            new_CalculatedSpeed = 2 * _speedSlider.value;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_horizontalInput, 0, _verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 
                                        Time.deltaTime*_rotationSpeed);
        }
        else _calculatedSpeed = (_calculatedSpeed < 0.01) ? 0: _calculatedSpeed;
        
        _calculatedSpeed = Mathf.Lerp(_calculatedSpeed, new_CalculatedSpeed, Time.deltaTime * _moveSpeed);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movement, 
                                        Time.deltaTime * _calculatedSpeed);
                                        
        _animator.SetFloat(movementId, _calculatedSpeed);
        
        _healthBarSmall.value = Mathf.Lerp(_healthBarSmall.value, _healthPercents, Time.deltaTime * _fillSpeed);
        _healthBarBig.value = _healthBarSmall.value;
    }

    public void KickJump()
    {
        _animator.SetTrigger(kickId);
    }

    public void WinDance()
    {
        _animator.SetTrigger(danceId);
    }

    public void MakeDamage(float damage)
    {
        _healthPercents -= damage/100;
        if (_healthPercents < 0)
        {
            _healthPercents = 0;
        }
    }
}
