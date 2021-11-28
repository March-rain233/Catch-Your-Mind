using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovieGuider : MonoBehaviour,IPointerClickHandler
{
    public VideoPlayer VideoPlayer;
    public List<VideoClip> VideoClips;
    public RawImage RawImage;
    public CardView Target;
    public Transform Parent;
    public Transform Point;

    int _current = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!VideoPlayer.isPlaying)
        {
            if (_current == VideoClips.Count - 1)
            {
                Target.transform.parent = Parent;
                Destroy(gameObject);
                return;
            }
            VideoPlayer.clip = VideoClips[++_current];
            VideoPlayer.Play();
        }
    }

    private void Awake()
    {
        _current = 0;
        VideoPlayer.clip = VideoClips[_current];
        VideoPlayer.Play();
        VideoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        Target.transform.position = Point.position;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        if(_current == 5)
        {
            Target.transform.parent = transform;
        }
    }
}
