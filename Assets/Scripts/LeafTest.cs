﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts
{
    class LeafTest : MonoBehaviour
    {
        private Light _sun;
        public Color Colour;
        public float Score;
        public Vector3 Forward;
        public Vector3 Right;

        void Start()
        {
            _sun = FindObjectOfType<Light>();
            GetComponent<Renderer>().material.color = Colour;
        }

        void Update()
        {
            Fitness fitnessFunction = new Fitness
            {
                LeafColour = Colour
            };
            transform.localScale = new Vector3(0.00001f, 1f, 1f);
            transform.right = -_sun.transform.forward;
            GetComponent<Renderer>().material.color = Colour;
            Score = fitnessFunction.CalculateColourEnergyFactor(new Vector3(Mathf.Pow(_sun.color.r / (670 / 437.5f) * 4.1f, 2), Mathf.Pow(_sun.color.g / (532.5f / 437.5f) * 3, 2), Mathf.Pow(_sun.color.b * 2.9f, 2)).normalized);
        }
    }
}