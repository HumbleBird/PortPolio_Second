using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Define;

// 날짜 : 2021-01-29 PM 6:18:55
// 작성자 : Rito

namespace Rito.InputBindings
{
    public class Test_InputBinding : MonoBehaviour
    {
        public InputBinding _binding = new InputBinding()
        {
            localDirectoryPath = @"Resources/Data/Input Binding/Presets",
            fileName = "InputBindingPreset",
            extName = "txt",
            id = "1"
        };

        private void Start()
        {
            _binding.LoadFromFile();
        }
    }
}