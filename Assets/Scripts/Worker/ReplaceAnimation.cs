using System;
using DG.Tweening;
using UnityEngine;

namespace WorkerStates
{
    public class ReplaceAnimation
    {
        private readonly Transform _target;
        private readonly float _durationTime;
        private readonly float _jumpPower;
        private readonly int _jumpsCount;

        public ReplaceAnimation(Transform target)
        {
            _target = target;
            _durationTime = 2f;
            _jumpPower = 2f;
        }

        public void Replace(Transform target, Action onComplete = null)
        {
            target.DORotate(Vector3.zero, _durationTime);
            target.DOMove(_target.position, _durationTime)
                .SetEase(Ease.OutQuad).OnComplete(() => onComplete?.Invoke());
        }
    }
    
}