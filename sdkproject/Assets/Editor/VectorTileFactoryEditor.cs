﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.ComponentModel;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.MeshGeneration.Interfaces;
using NodeEditorNamespace;

namespace Mapbox.NodeEditor
{
	[CustomEditor(typeof(VectorTileFactory))]
	public class VectorTileFactoryEditor : UnityEditor.Editor
	{
		private VectorTileFactory _factory;
		private MonoScript script;

		private void OnEnable()
		{
			script = MonoScript.FromScriptableObject((VectorTileFactory)target);
			_factory = target as VectorTileFactory;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			GUI.enabled = false;
			script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Visualizers");
			var facs = serializedObject.FindProperty("Visualizers");
			for (int i = 0; i < facs.arraySize; i++)
			{
				var ind = i;
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginVertical();
				GUILayout.Space(5);
				GUI.enabled = false;
				EditorGUILayout.BeginHorizontal();
				if (_factory.Visualizers[i] != null)
				{
					_factory.Visualizers[i].Key = EditorGUILayout.TextField(_factory.Visualizers[i].Key, GUILayout.MaxWidth(100));
				}
				//facs.GetArrayElementAtIndex(ind).objectReferenceValue = EditorGUILayout.ObjectField(facs.GetArrayElementAtIndex(i).objectReferenceValue, typeof(LayerVisualizerBase)) as ScriptableObject;
				if (_factory.Visualizers[i] == null)
					EditorGUILayout.TextField("null");
				else
					EditorGUILayout.ObjectField(_factory.Visualizers[i], typeof(LayerVisualizerBase));
				EditorGUILayout.EndHorizontal();
				GUI.enabled = true;
				EditorGUILayout.EndVertical();

				if (GUILayout.Button(NodeBasedEditor.magnifierTexture, (GUIStyle)"minibuttonleft", GUILayout.Width(30)))
				{
					Mapbox.NodeEditor.ScriptableCreatorWindow.Open(typeof(LayerVisualizerBase), facs, ind);
				}
				if (GUILayout.Button(new GUIContent("-"), (GUIStyle)"minibuttonright", GUILayout.Width(30), GUILayout.Height(22)))
				{
					facs.DeleteArrayElementAtIndex(ind);
				}
				EditorGUILayout.EndHorizontal();
			}

			if (GUILayout.Button(new GUIContent("Add New")))
			{
				Mapbox.NodeEditor.ScriptableCreatorWindow.Open(typeof(LayerVisualizerBase), facs);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}