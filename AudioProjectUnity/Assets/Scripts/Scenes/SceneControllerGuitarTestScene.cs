using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;
using DG.Tweening;

public class SceneControllerGuitarTestScene: SceneController_Base 
{
	#region inspector hooks
	public bool debugMe = true;

	public RJWS.Audio.UI.GuitarSettingsPanel guitarSettingsPanel;
	public RectTransform buttonsPanel;
	
	public float tweenDuration = 1f;
	private Vector2 buttonPanelOffPos;

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

	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.GuitarTestScene;
	}

	override protected void PostStart()
	{
	}

	override protected void PostAwake()
	{
		guitarSettingsPanel.gameObject.SetActive( false );
		buttonPanelOffPos = new Vector2(buttonsPanel.rect.width,0f);
	}

	#endregion SceneController_Base

}
