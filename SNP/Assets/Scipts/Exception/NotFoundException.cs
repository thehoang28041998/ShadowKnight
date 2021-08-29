using System;
using UnityEngine;

namespace Scipts.Exception {
    public class NotFoundException : System.Exception{
        public NotFoundException(Type type, string msg) : base($"Not found {type.FullName} with message: {msg}") {
            Debug.LogError($"Not found {type.FullName} with message: {msg}");
        }
    }
}