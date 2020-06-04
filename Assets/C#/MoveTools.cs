using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveTools {
    [Serializable]
    internal class MoveDimension {
        [SerializeField]
        Transform charTransform;
        [SerializeField]
        float maxVelocity;

        float speed;
        internal void UpdateVelocity(float value) {
            speed = value * Time.deltaTime;
        }

        internal void SetPosition(Vector3 _direction) {
            charTransform.position += _direction * speed;
        }

    }

    [Serializable]
    internal class PhysicsMoveDimension : MoveDimension {


    }


    [Serializable]
    internal class RotationDimension {

    }

    [Serializable]
    internal class PhysicsRotationDimension : RotationDimension {

    }
}
