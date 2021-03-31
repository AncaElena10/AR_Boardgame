﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    
    [SerializeField]
    private RoomListing _roomListings;

    private List<RoomListing> _listings = new List<RoomListing>();
    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public override void OnJoinedRoom()
    {
        _roomsCanvases.CurrentRoomCanvas.Show();
        // destroy the children for room listings when I leave the room
        _content.DestroyChildren();
        // clear up the listings
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList) {
            // removed from rooms list
            if (info.RemovedFromList) {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // added to rooms list
            else {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index == -1) {
                    RoomListing listing = Instantiate(_roomListings, _content);

                    if (listing != null) {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                } else {
                    // modify listing
                    // _listings[index].whatever
                }
            }
        }
    }
}
