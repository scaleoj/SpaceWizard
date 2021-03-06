﻿using UnityEditor;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;
using UnityEditorInternal;
using _Game.Scripts.Scriptable_Objects;

namespace Editor
{
    public class GridWindow : EditorWindow
    {
        private Object _root;
        private Object _prefab;
        private string _name = "Grid";
        private int _width = 10;
        private int _oldWidth = 10;
        private int _depth = 10;
        private int _oldDepth = 10;
        private float _distanceBetweenPoints = 1f;
        private float _oldDistanceBetweenPoints = 1f;
        private EditorGrid _editorGrid;
        private bool _rdy;
        private bool _prefabCheck;
        private LayerMask _mask;
        private Object _retreat;
        private bool _loaded;
        private Object _scriptOb;
        private GridObject _gridObject;


        [MenuItem("Tools/Grid")]
        public static void ShowWindow()
        {
            GetWindow<GridWindow>("Custom Grid");
        }

        private void OnGUI()
        {
            if (!_loaded)
            {
                
                _scriptOb = EditorGUILayout.ObjectField("Scriptable Object for Saves", _scriptOb,
                    typeof(ScriptableObject), false);
                if (_scriptOb == null) return;
                _gridObject = (GridObject) _scriptOb;
                _loaded = true;
            }
            else
            {
                GUILayout.Label("Build or Scan a Grid!", EditorStyles.label);

                BuildUi();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Build new Grid!"))
                {
                    BuildGridButton();
                }

                if (GUILayout.Button("Scan old Grid!"))
                {
                    ScanGridButton();
                }

                if (GUILayout.Button("Save Grid!"))
                {
                    SaveGrid();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();

                if (_editorGrid == null) return;
                ChangeLogic();
            }
        }

        private void SaveGrid()
        {
            if (_root != null)
            {
                if (_retreat != null)
                {
                    _gridObject.Width = _editorGrid.GetWidth();
                    _gridObject.Depth = _editorGrid.GetDepth();
                    _gridObject.DistanceBetweenPoints = _editorGrid.GetDistance();
                    _gridObject.Retreat = (GameObject) _retreat;
                    _gridObject.Mask = _mask;
                    _scriptOb = _gridObject;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(_gridObject);
                    
                }
                else
                {
                    Debug.Log("No Retreat set");
                }
            }
            else
            {
                Debug.Log("No Grid build or Scanned");
            }
        }

        private void BuildUi()
        {
            _name = EditorGUILayout.TextField("RootName", _name);
            EditorGUILayout.Space();
            _root = EditorGUILayout.ObjectField("OldGridRoot", _root, typeof(GameObject), true);
            EditorGUILayout.Space();
            _prefab = EditorGUILayout.ObjectField("TilePrefab", _prefab, typeof(GameObject), true);
            EditorGUILayout.Space();
            _width = EditorGUILayout.IntSlider("Width", _width, 1, 50);
            EditorGUILayout.Space();
            _depth = EditorGUILayout.IntSlider("Depth", _depth, 1, 50);
            EditorGUILayout.Space();
            _distanceBetweenPoints = EditorGUILayout.FloatField("Scale", _distanceBetweenPoints);
            EditorGUILayout.Space();
            if (_root != null)
            {
                _retreat = EditorGUILayout.ObjectField("RetreatTile", _retreat, typeof(GameObject), true);
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("WalkableLayer");
            _mask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_mask),
                InternalEditorUtility.layers);
            _mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(_mask);
            EditorGUILayout.Space();
        }

        private void BuildGridButton()
        {
            if (_prefab != null)
            {
                _editorGrid = new EditorGrid(_name, _width, _depth, _distanceBetweenPoints,
                    (GameObject) _prefab);
                _editorGrid.BuildGrid();
                _root = _editorGrid.GetParent();
                Selection.SetActiveObjectWithContext(_editorGrid.GetParent(), this);
            }
            else
            {
                Debug.Log("Please enter a Prefab to build a new Grid!");
            }
        }

        private void ScanGridButton()
        {
            if (_root != null)
            {
                _editorGrid = new EditorGrid((GameObject) _root);
                _editorGrid.ScanGrid();
                _name = _editorGrid.GetName();
                _width = _editorGrid.GetWidth();
                _depth = _editorGrid.GetDepth();
                _distanceBetweenPoints = _editorGrid.GetDistance();
                _prefabCheck = false;
                Selection.SetActiveObjectWithContext(_root, this);
            }
            else
            {
                Debug.Log("Please enter a Root Object to scan it for an existing Grid");
            }
        }

        private void ChangeLogic()
        {
            if (_prefab != null && !_prefabCheck)
            {
                _editorGrid.SetPrefab((GameObject) _prefab);
                _prefabCheck = true;
            }

            if (_prefab == null)
            {
                _prefabCheck = false;
            }

            if (_oldWidth != _width && _prefabCheck)
            {
                _editorGrid.UpdateWidth(_width);
                _oldWidth = _width;
            }

            if (_oldDepth != _depth && _prefabCheck)
            {
                _editorGrid.UpdateDepth(_depth);
                _oldDepth = _depth;
            }

            if (_oldDistanceBetweenPoints + 0.1f >= _distanceBetweenPoints &&
                _oldDistanceBetweenPoints - 0.1f <= _distanceBetweenPoints ||
                !_prefabCheck) return;
            _editorGrid.UpdateDistance(_distanceBetweenPoints);
            _oldDistanceBetweenPoints = _distanceBetweenPoints;
        }
    }
}