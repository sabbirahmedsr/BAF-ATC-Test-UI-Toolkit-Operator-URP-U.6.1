
> #### TODO
* add two variable to path namely

```csharp
bool isVisibleInRadarMap;
bool isVisibleInSurfaceMap;
```

and remove from spawn info

```csharp
public struct NetworkAirplane_SpawnInfo{
    internal ushort startLineIndex_radarMap;
    internal ushort startLineIndex_surfaceMap;
}
```

* Create Extension for Riptide Message to replace this long spawn info message add
```csharp
public void AddToMessage(Message msg) {
    msg.AddUShort((ushort)this.callSign);
    msg.AddUShort((ushort)this.movementType);
    msg.AddVector3(this.pos);
    msg.AddVector3(this.fwd);
    lastNT.AddToMessage(msg);
    nextNT.AddToMessage(msg);
    msg.AddUShort(this.startLineIndex_radarMap);
    msg.AddUShort(this.startLineIndex_surfaceMap);
    msg.AddBool(this.hasRadarMap);
    msg.AddBool(this.hasSurfaceMap);
    //
    msg.AddUShort((ushort)this.pathHistory.Length);
    for (int i = 0; i < pathHistory.Length; i++) {
        Path path = pathHistory[i];
        path.AddToMessage(msg);
    }
    msg.AddUShort((ushort)this.allArrivalCommandID.Length);
    for (int i = 0; i < allArrivalCommandID.Length; i++) {
        msg.AddUShort((ushort)allArrivalCommandID[i]);
    }
    msg.AddUShort((ushort)this.allDepartureCommandID.Length);
    for (int i = 0; i < allDepartureCommandID.Length; i++) {
        msg.AddUShort((ushort)allDepartureCommandID[i]);
    }
}
```

* There could be a "Auto Complete" button to complete the current phase of airplane immediately or "Next Phase" button to jump to next phase immediately

