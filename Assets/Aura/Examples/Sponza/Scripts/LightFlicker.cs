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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AuraAPI
{
    //[ExecuteInEditMode]
    public class LightFlicker : MonoBehaviour
    {
        private float _currentFactor = 1.0f;
        private Vector3 _currentPos;
        private float _deltaTime;
        private Vector3 _initPos;
        private float _targetFactor;
        private Vector3 _targetPos;

        private float _time;
        private float _timeLeft;
        public Color baseColor;
        public float maxFactor = 1.2f;
        public float minFactor = 1.0f;
        public float moveRange = 0.1f;
        public float speed = 0.1f;

        private void Start()
        {
            Random.InitState((int)transform.position.x + (int)transform.position.y);
        }

        //

        private void OnEnable()
        {
            _initPos = transform.localPosition;
            _currentPos = _initPos;
        }

        //

        private void OnDisable()
        {
            transform.localPosition = _initPos;
        }

        //

#if !UNITY_EDITOR
    private void Update()
    {
        _deltaTime = Time.deltaTime;
#else
        void OnRenderObject()
        {
            float currentTime = (float)EditorApplication.timeSinceStartup;
            _deltaTime = currentTime - _time;
            _time = currentTime;
#endif

            if (_timeLeft <= _deltaTime)
            {
                _targetFactor = Random.Range(minFactor, maxFactor);
                _targetPos = _initPos + Random.insideUnitSphere * moveRange;
                _timeLeft = speed;
            }
            else
            {
                float weight = _deltaTime / _timeLeft;
                _currentFactor = Mathf.Lerp(_currentFactor, _targetFactor, weight);

                //GetComponent<AuraLight>().overridingColor = baseColor * _currentFactor;
                GetComponent<Light>().color = baseColor * _currentFactor;
                _currentPos = Vector3.Lerp(_currentPos, _targetPos, weight);
                transform.localPosition = _currentPos;
                _timeLeft -= _deltaTime;
            }
        }
    }
}
