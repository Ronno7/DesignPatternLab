Events contains Chapter 6's RaceEventType, RaceEventBus, and CountdownTimer.
It also contains Chapter 9's Subject and Observer abstract classes. The concrete
BikeController subject and HUD/camera observers live with their owning features.
Replay contains Chapter 7's Invoker and the Command, TurnLeft, TurnRight,
and ToggleTurbo classes in its Commands subfolder. InputHandler lives in UI.
Chapter 9 adds DamageBike so the new damage test is recorded and replayed too.
Pooling contains Chapter 8's DroneObjectPool. The pooled Drone lives in
Characters/Drone; ClientObjectPool lives in UI.

Create future folders when their implementations begin, such as PowerUps
or Weapons. Keep the shared GameManager in Scripts/Core
and bike behavior in Scripts/Characters/Bike.

The chapter-to-folder plan is in Docs/Roadmap.md at the project root.
