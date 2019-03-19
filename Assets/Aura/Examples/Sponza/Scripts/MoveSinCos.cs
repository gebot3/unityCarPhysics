///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace AuraAPI
{
    public class MoveSinCos : MonoBehaviour
    {
        private Vector3 _initialPosition;
        public float cosAmplitude; // = Random.Range(0.5f, 2.0f);

        public Vector3 cosDirection = Vector3.right;
        public float cosOffset; // = Random.Range(-Mathf.PI, Mathf.PI);
        public float cosSpeed; // = Random.Range(2.0f, 3.5f);
        public float sinAmplitude; // = Random.Range(0.5f, 2.0f);
        public Vector3 sinDirection = Vector3.up;
        public float sinOffset; // = Random.Range(-Mathf.PI, Mathf.PI);
        public float sinSpeed; // = Random.Range(2.0f, 3.5f);

        public Space space = Space.Self;

        private void Start()
        {
            _initialPosition = transform.position;
        }

        private void Update()
        {
            Vector3 sinVector = sinDirection.normalized * Mathf.Sin(Time.time * sinSpeed + sinOffset) * sinAmplitude;
            Vector3 cosVector = cosDirection.normalized * Mathf.Cos(Time.time * cosSpeed + cosOffset) * cosAmplitude;

            sinVector = space == Space.World ? sinVector : transform.localToWorldMatrix.MultiplyVector(sinVector);
            cosVector = space == Space.World ? cosVector : transform.localToWorldMatrix.MultiplyVector(cosVector);

            transform.position = _initialPosition + sinVector + cosVector;
        }
    }
}
