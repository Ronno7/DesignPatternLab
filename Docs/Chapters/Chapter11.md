# Chapter 11 - Drone maneuvers with the Strategy pattern

Based on the supplied Chapter 11 excerpt from David Baron's *Game Development Patterns with Unity 2021, Second Edition*.

The current checkout is already wired. Open the Unity project in `DesignPatternLab/`, open **Main**, and enter Play Mode. A Game view of at least **960 x 600** fits the chapter panels.

## Try it

1. In the center **Drone maneuvers - Chapter 11** panel, select **Bob**, **Weave**, or **Fall**, then click **Spawn Drone**. You can test while the bike is stopped.
2. **Bob** moves vertically between the spawn height and `maxHeight`, pausing one second at each endpoint.
3. **Weave** moves horizontally between `-weavingDistance` and `+weavingDistance` on this straight track, pausing one second at each endpoint.
4. **Fall** retreats along positive Z by `fallbackDistance`, then holds its final position. The laser points back toward the approaching bike, so positive Z is the retreat direction.
5. While that drone is active, choose another maneuver and click **Apply to latest drone**. Only the newly selected movement continues.
6. Choose **Random** to let the client select one of the three strategies at runtime. The existing Chapter 8 **Spawn Drones** button uses the selected choice too, choosing independently for each drone when Random is selected.
7. Watch the pool counters. Main gives drones **eight seconds** of life so their movements are easier to observe. They return to the pool, and subsequent spawns reuse the existing objects and strategy components.
8. To see the diagnostic laser, switch to **Scene view** during Play Mode and enable **Gizmos**. The ray points down at 45 degrees, is blue when clear, and green when it hits a non-trigger collider. It has a maximum length of 15 units.

As in the chapter's skeleton code, the laser is a debug raycast. It does not damage the bike or shield, render a production laser effect, or implement game-over behavior. Debug rays are not visible in a WebGL build. The drones' movement is visible in Game view and builds.

## Pattern roles

| Role | Implementation |
| --- | --- |
| Strategy interface | `IManeuverBehaviour.Maneuver(Drone)` |
| Context | Existing pooled `Drone`, using `ApplyStrategy(IManeuverBehaviour)` |
| Concrete strategies | `BoppingManeuver`, `WeavingManeuver`, `FallbackManeuver` |
| Client | `ClientStrategy`, selecting a strategy and passing it to the drone |

The book spells its bobbing class **BoppingManeuver**; this project preserves that class name. The UI calls it **Bobbing**.

Each concrete strategy owns its movement algorithm. Drone knows only the interface and handles coroutine lifetime. The client knows which concrete implementations are available. The drone does not use a switch statement to choose its movement or change strategies based on internal state.

## Settings

Select a spawned drone in the Hierarchy during Play Mode to inspect its settings:

- **Speed** retains the book's field name but means **seconds per movement segment**, not units per second. Lower values move faster. The default is one second; values are clamped to at least 0.01 seconds when a maneuver begins.
- **Max Height** is the bobbing upper world-space Y coordinate, default 5. A drone spawned above it stays at its starting height rather than moving farther upward.
- **Weaving Distance** is the horizontal endpoint magnitude around world X=0, default 1.5. Keep it within the road's half-width of 5.
- **Fallback Distance** is a relative positive-Z retreat distance, default 20.
- **Ray Distance** is the diagnostic laser's maximum length, default 15.

Movement settings are sampled when the strategy starts. After adjusting them, click **Apply to latest drone** to restart the maneuver with those values. Returning to the pool resets health and cancels movement; spawning resets position, rotation, lifetime, and strategy selection. Inspector tuning on an existing drone remains on that reused instance.

Select **DronePool** to adjust **Drone Lifetime**. Its class default remains three seconds; Main sets it to eight. Changing strategies does not restart or cancel that timer.

## Add it to your existing project

These transfer steps have **already been applied in this checkout**:

1. Add `Scripts/Characters/Drone/Strategies/` with `IManeuverBehaviour.cs`, `BoppingManeuver.cs`, `WeavingManeuver.cs`, `FallbackManeuver.cs`, and all `.meta` files.
2. Add `Scripts/UI/ClientStrategy.cs` and its `.meta` file.
3. Merge updates to `Scripts/Characters/Drone/Drone.cs`, `Scripts/Systems/Pooling/DroneObjectPool.cs`, and `Scripts/UI/ClientObjectPool.cs`, preserving existing metadata and local changes.
4. On the existing **DronePool** GameObject in Main, add **ClientStrategy**. Keep **DroneObjectPool** and **ClientObjectPool**. Set **Selection** to **Random** and the pool's **Drone Lifetime** to **8**.
5. Save Main. No drone prefab or manual strategy-component wiring is required: the client adds each of the three strategy components once per pooled drone and reuses them on later spawns.

Script paths are relative to `Assets/_Project/`. Do not attach `IManeuverBehaviour` as a component.

## Explicit adaptations from the book

- Integrates with Chapter 8's pool and the existing Drone instead of creating a second drone implementation. The pool's `Spawned` event lets the client choose behavior after each activation without making the pool depend on concrete strategies or UI classes.
- `Drone` runs the strategy's coroutine through `StartManeuver`. Switching strategies stops just that coroutine; returning to the pool stops all drone coroutines. This prevents old strategies from fighting over the transform or resuming after reuse, while preserving the separate self-destruct timer.
- Each movement explicitly reaches its endpoint after interpolation, and movement durations are guarded against zero.
- Weaving alternates between both signed endpoints around this project's straight track. Fallback uses a distance relative to the current position instead of the sample's absolute Z coordinate, so it still retreats after the bike has traveled down the track.
- Spawn X is clamped inside the road and spawn height is narrowed so bobbing is easy to see. Main uses a longer lifetime for this demonstration.
- The debug laser direction is recomputed from the drone's current orientation, and raycasts ignore pickup triggers. It remains independent of which maneuver is selected.
- The client offers explicit selection and runtime swapping in addition to the book's random spawn selection. Strategy components are reused rather than repeatedly added.
- Drone spawning and random strategy selection remain a separate demonstration from the bike's command replay. They do not apply damage or change the recorded bike actions.

## Regression checks

- Bobbing changes only Y; weaving changes only X; fallback changes only Z and stops after its movement duration.
- Reapply a strategy or change to another while moving: the old coroutine stops and only one maneuver controls the drone.
- After returning to the pool, the drone stops moving, clears its current strategy, and starts fresh on reuse.
- Repeated spawns keep one component of each concrete strategy on each drone.
- Switching maneuvers does not extend the drone's lifetime.
- Existing Chapter 10 pickup collection, power-up caps, replay resets, and Chapter 9 lethal-damage replay still work.

The existing WebGL export has not been rebuilt. Build a fresh export to update the hosted demo.

## Validation performed

Runtime scripts compiled against Unity 6000.3.24f1. An isolated copy of Main, using a minimal package manifest, passed **71 automated Play Mode assertions** with **zero gameplay errors**. These included the previous Chapter 10 suite plus movement endpoints, strategy replacement, coroutine cancellation on pool return, component reuse, random selection, lifetime preservation, a zero-duration guard, laser direction, and safe expiry of an unpooled drone.

The headless editor still reported the native `fidA != fidB` startup assertion and `UnityEditor.Search.SearchDatabase` indexing exception previously reproduced on unchanged Chapter 9. These baseline diagnostics are tracked separately from gameplay failures. Interactive Game-view layout and a new WebGL build have not been verified.
