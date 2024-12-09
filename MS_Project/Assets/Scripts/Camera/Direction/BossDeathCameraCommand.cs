using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathCameraCommand : ICameraEffectCommand
{
    private readonly Vector3 _bossPosition;
    private readonly Vector3[] _cameraPositions;
    private int _currentPositionIndex = 0;
    private float _elapsedTime = 0;
    private const float POSITION_DURATION = 0.5f;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Quaternion _startRotation;
    private Quaternion _targetRotation;

    public BossDeathCameraCommand(Vector3 bossPosition)
    {
        _bossPosition = bossPosition;
        _cameraPositions = new[]
        {
           _bossPosition + new Vector3(5, 2, 5),
           _bossPosition + new Vector3(-5, 2, 5),
           _bossPosition + new Vector3(0, 5, -5)
       };
    }

    public void Execute(Camera camera)
    {
        if (_currentPositionIndex == 0)
        {
            InitializeFirstPosition(camera);
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= POSITION_DURATION)
        {
            MoveToNextPosition(camera);
            return;
        }

        var t = _elapsedTime / POSITION_DURATION;
        camera.transform.position = Vector3.Lerp(_startPosition, _targetPosition, t);
        camera.transform.rotation = Quaternion.Slerp(_startRotation, _targetRotation, t);
    }

    private void InitializeFirstPosition(Camera camera)
    {
        _startPosition = camera.transform.position;
        _targetPosition = _cameraPositions[0];
        _startRotation = camera.transform.rotation;
        _targetRotation = Quaternion.LookRotation(_bossPosition - _targetPosition);
    }

    private void MoveToNextPosition(Camera camera)
    {
        _currentPositionIndex++;
        _elapsedTime = 0;

        if (_currentPositionIndex < _cameraPositions.Length)
        {
            _startPosition = camera.transform.position;
            _targetPosition = _cameraPositions[_currentPositionIndex];
            _startRotation = camera.transform.rotation;
            _targetRotation = Quaternion.LookRotation(_bossPosition - _targetPosition);
        }
    }

    public bool IsComplete()
    {
        return _currentPositionIndex >= _cameraPositions.Length;
    }

    public void Terminate()
    {
        _currentPositionIndex = _cameraPositions.Length;
    }
}