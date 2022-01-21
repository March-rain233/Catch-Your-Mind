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
    public bool Select = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!VideoPlayer.isPlaying && Select)
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

    public void StartPlay()
    {
        _current = 0;
        Select = true;
        VideoPlayer.clip = VideoClips[_current];
        VideoPlayer.Play();
        VideoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        Target.transform.position = Point.position;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        if(_current == 4)
        {
            Target.transform.parent = transform;
            Select = false;
            GameObject.Find("MakerPanel").GetComponent<MakerPanel>().OnSelect += MovieGuider_OnSelect;
        }
    }

    private void MovieGuider_OnSelect()
    {
        Select = true;
        VideoPlayer.clip = VideoClips[++_current];
        VideoPlayer.Play();
    }
}
