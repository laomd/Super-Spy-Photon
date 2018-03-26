using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MiniMapCameraManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler{
	Transform minimap, point;
	public ScaleController scaleController;

	[HideInInspector]
	public int flag = 1;

	Vector3 cameraOffset;

    private Vector3 mapSize;

    private Vector2 miniMapSize;

	private Transform target;

    private PolygonCollider2D polygonCollider2D;

	private AudioSource audios;

    void Start()
    {
        point = transform.Find( "Mask/Point" );
		minimap = scaleController.minimap;
        polygonCollider2D = transform.Find( "Mask/Bg" ).GetComponent<PolygonCollider2D>();

        mapSize = MiniMapView.Instance.mapSize;
        miniMapSize = MiniMapView.Instance.miniMapSize;
		audios = GetComponent<AudioSource> ();
		point.localPosition = Vector3.zero;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
		if (!polygonCollider2D.OverlapPoint( eventData.position ) )
        {
            return;
        }
		point.position = eventData.position;
		if (minimap.localScale.x == 1) {
			var canvas = GameObject.Find ("Canvas");
			var joystick = canvas.GetComponentInChildren<ETCJoystick> ();
			target = joystick.cameraLookAt;
			cameraOffset = joystick.followOffset;
			joystick.cameraLookAt = null;
			Vector2 pos = point.localPosition;
			pos.x *= flag;
			pos.y *= flag;
			SetCameraPosition( pos );
		} else {
			if (!audios.isPlaying) {
				audios.Play ();
			}
		}
    }
	public void OnPointerUp( PointerEventData eventData ) {
		if (minimap.localScale.x == 1) {
			var controller = GameObject.FindWithTag ("GameController");
			var joystick = controller.GetComponentInChildren<ETCJoystick> ();
			joystick.cameraLookAt = target;
		}
	}
    public void OnDrag( PointerEventData eventData )
    {
        if ( !polygonCollider2D.OverlapPoint( eventData.position ) )
        {
            return;
        }

        if (Vector2.Distance(point.position, eventData.position) < 10)
        {
            return;
        }

        point.position = eventData.position;
		Vector2 pos = point.localPosition;
		pos.x *= flag;
		pos.y *= flag;
		SetCameraPosition( pos );
    }

    Tweener tweener;
    void SetCameraPosition(Vector2 vec)
    {
        Vector3 targetPosition = new Vector3( mapSize.x * vec.x / miniMapSize.x, 0, mapSize.z * vec.y / miniMapSize.y ) + cameraOffset;
        if ( tweener != null && tweener.IsPlaying() )
        {
            tweener.Kill( false );
        }

		tweener = DOTween.To( () => Camera.main.transform.localPosition, value => Camera.main.transform.localPosition = value, targetPosition, 0.3f ).SetEase(Ease.OutQuad);
    }
}
