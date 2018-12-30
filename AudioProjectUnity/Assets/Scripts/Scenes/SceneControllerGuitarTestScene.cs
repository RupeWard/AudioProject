using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;
using DG.Tweening;
using RJWS.Core.Maths.PosRotExtensions;

public class SceneControllerGuitarTestScene: SceneController_Base 
{
	#region inspector hooks
	public bool debugMe = true;

	public RJWS.Audio.UI.GuitarSettingsPanel guitarSettingsPanel;
	public RectTransform buttonsPanel;
	
	public float tweenDuration = 1f;
	private Vector2 buttonPanelOffPos;

	public RJWS.Audio.GuitarModel guitarModel;
	public RJWS.Audio.GuitarView guitarView;

	public Transform cameraTransform;

	public RJWS.Core.SOVariables.PosRotVariable farCameraPos;
	public RJWS.Core.SOVariables.PosRotVariable bridgeCameraPos;

	public enum ECamerPos
	{
		Far,
		Bridge
	}

	private ECamerPos _cameraPos = ECamerPos.Far;
	public ECamerPos startCameraPos = ECamerPos.Far;

	#endregion inspector hooks

	#region event handlers

	public void HandleQuitButtonPressed( )
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

	public void HandleOnButton()
	{
		buttonsPanel.DOKill( );
		buttonsPanel.DOAnchorPos( Vector2.zero, tweenDuration ).SetEase( Ease.InOutQuad );
	}

	public void HandleOffButton( )
	{
		buttonsPanel.DOKill( );
		buttonsPanel.DOAnchorPos( buttonPanelOffPos, tweenDuration ).SetEase( Ease.InOutQuad );
	}

	public void HandleGuitarSettingsButton( )
	{
		guitarSettingsPanel.Init( );
	}

	public void HandlePosButton()
	{
		if (_cameraPos == ECamerPos.Bridge)
		{
			SetCameraPos( ECamerPos.Far );
		}
		else if (_cameraPos == ECamerPos.Far)
		{
			SetCameraPos( ECamerPos.Bridge );
		}
	}

	private void SetCameraPos(ECamerPos pos)
	{
		if (pos == ECamerPos.Far)
		{
			cameraTransform.SetWorldPose( farCameraPos.Value );
			_cameraPos = ECamerPos.Far;
		}
		else if (pos == ECamerPos.Bridge)
		{
			cameraTransform.SetWorldPose( bridgeCameraPos.Value );
			_cameraPos = ECamerPos.Bridge;
		}
	}

	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.GuitarTestScene;
	}

	override protected void PostStart()
	{
		guitarModel.Init( RJWS.Core.Audio.AudioConsts.s_standardGuitarTuning );
		guitarView.Init( guitarModel );
		SetCameraPos( startCameraPos );
	}

	override protected void PostAwake()
	{
		guitarSettingsPanel.gameObject.SetActive( false );
		buttonPanelOffPos = new Vector2(buttonsPanel.rect.width,0f);
	}

	#endregion SceneController_Base

}
