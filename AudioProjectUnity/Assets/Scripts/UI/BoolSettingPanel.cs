using UnityEngine;
using System.Collections;

namespace RJWS.UI
{
	public class BoolSettingPanel : MonoBehaviour
	{
		#region inspector hooks

		public UnityEngine.UI.InputField inputField;
		public UnityEngine.UI.Text titleText;
		public UnityEngine.UI.Text messageText;
		public UnityEngine.UI.Text minText;
		public UnityEngine.UI.Text maxText;

		#endregion inspector hooks

		#region inspector data

		public float messageDuration = 5f;

		#endregion inspector data

		#region actions

		public System.Action<bool> onValueChangedAction;

		#endregion actions

		#region private data

		private bool oldSetting_;
		private bool currentSetting_;

		#endregion private data

		private void Awake( )
		{
			gameObject.SetActive( false );
		}

		public void Init( string title, bool current, System.Action<bool> changeAction )
		{
			titleText.text = title;
			oldSetting_ = current;
			currentSetting_ = current;

			onValueChangedAction = changeAction;

			messageText.text = string.Empty;
			messageText.gameObject.SetActive( false );

			SetValue( oldSetting_ );

			gameObject.SetActive( true );
		}

		private void SetValue( bool b )
		{
//			inputField.text = f.ToString( );
		}

		/*
		private void SetMessage( string m )
		{
			SetMessage( m, messageDuration );
		}

		private void SetMessage( string m, float delay )
		{
			StartCoroutine( SetMessageCR( m, delay ) );
		}

		private IEnumerator SetMessageCR( string m, float delay )
		{
			messageText.gameObject.SetActive( true );
			messageText.text = m;

			System.DateTime startTime = System.DateTime.UtcNow;
			float elapsed = 0f;
			do
			{
				System.TimeSpan span = System.DateTime.UtcNow - startTime;
				elapsed = (float)span.TotalSeconds;
				yield return null;
			} while (elapsed < delay);
			messageText.gameObject.SetActive( false );
		}
		*/

		#region handlers

		/*
		public void OnInputFieldEndEdit( string s )
		{
			int i;
			if (int.TryParse( inputField.text, out i ))
			{
				if (range_[0] != int.MinValue && i < range_[0])
				{
					SetMessage( "Too low!" );
					SetValue( currentSetting_ );
				}
				else if (range_[1] != int.MaxValue && i > range_[1])
				{
					SetMessage( "Too high!" );
					SetValue( currentSetting_ );
				}
				else
				{
					currentSetting_ = i;
					if (onValueChangedAction != null)
					{
						onValueChangedAction( currentSetting_ );
					}
					SetValue( currentSetting_ );
				}
			}
			else
			{
				SetMessage( "Not an integer!" );
				SetValue( currentSetting_ );
			}
		}
		*/

		public void onDoneButtonClicked( )
		{
			gameObject.SetActive( false );
		}

		public void onResetButtonClicked( )
		{
			currentSetting_ = oldSetting_;
			SetValue( oldSetting_ );
			if (onValueChangedAction != null)
			{
				onValueChangedAction( oldSetting_ );
			}
		}

		#endregion handlers

		#region factory

		static public IntSettingPanel CreateFromRefab( )
		{
			return Resources.Load<GameObject>( "Prefabs/UI/IntSettingPanel" ).GetComponent<IntSettingPanel>( );
		}
		#endregion factory

	}

}
