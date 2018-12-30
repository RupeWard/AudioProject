using UnityEditor;
using UnityEngine;
using RJWS.Core.TransformExtensions;
using RJWS.Core.Maths.PosRotExtensions;

namespace RJWS.Core.SOVariables
{
	[CustomEditor (typeof (PosRotSet))]
	public class PosRotSetEditor : Editor
	{
		private SerializedObject obj;

		public void OnEnable( )
		{
			obj = new SerializedObject( target );
		}

		public override void OnInspectorGUI( )
		{
            DropAreaGUI();
            EditorGUILayout.Space();
            DrawDefaultInspector( );
		}

		const string WORLD_LEGEND = "World Space";
		const string LOCAL_LEGEND = "Local Space";

		public void DropAreaGUI( )
		{
			DropArea( true );
			DropArea( false );
		}

		private void DropArea( bool worldSpace )
		{
			Event evt = Event.current;
			Rect drop_area = GUILayoutUtility.GetRect( 0.0f, 50.0f, GUILayout.ExpandWidth( true ) );
			GUI.Box( drop_area, "DropTransform for children's "+((worldSpace)?(WORLD_LEGEND):(LOCAL_LEGEND))+" poses" );

			switch (evt.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!drop_area.Contains( evt.mousePosition ))
						return;

					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					if (evt.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag( );

						if (DragAndDrop.objectReferences.Length != 1)
						{
							Debug.LogWarningFormat( "{0} object references!", DragAndDrop.objectReferences.Length );
						}
						else
						{
							GameObject go = DragAndDrop.objectReferences[0] as GameObject;
							if (go == null)
							{
								Debug.LogWarningFormat( "{0} not a GameObject! ", DragAndDrop.objectReferences[0].name);
							}
							else
							{
								PosRotSet prs = target as PosRotSet;
								if (prs != null)
								{
                                    int num = go.transform.childCount;

                                    prs.posrots = new Maths.PosRot[num];

                                    for (int i = 0; i < num; i++)
                                    {
                                        prs.posrots[i] = Maths.PosRot.Identity;
                                        prs.posrots[i].SetTo(go.transform.GetChild(i), worldSpace);
                                    }

									EditorUtility.SetDirty( target );
									AssetDatabase.SaveAssets( );

                                    /*
									SerializedProperty positionProperty = obj.FindProperty( "Value.position" );
									SerializedProperty rotationProperty = obj.FindProperty( "Value.rotation" );

									positionProperty.vector3Value.Set( go.transform.position.x, go.transform.position.y, go.transform.position.z );
									rotationProperty.quaternionValue.Set( go.transform.rotation.x, go.transform.rotation.y, go.transform.rotation.z, go.transform.rotation.w );

									Debug.LogFormat( "Set {0} using {1} values in {2}: P = {3} R = {4}",
										obj.targetObject.name,
										go.transform.GetPathInHierarchy( ),
										((worldSpace) ? (WORLD_LEGEND) : (LOCAL_LEGEND)),
										positionProperty.vector3Value.ToString( "F4" ),
										rotationProperty.quaternionValue.eulerAngles.ToString( "F4" ) );
                                        */
								}
								else
								{
									Debug.LogError( "No PosRotSet" );
								}
							}
						}
					}
					break;
			}
		}
	}
}
