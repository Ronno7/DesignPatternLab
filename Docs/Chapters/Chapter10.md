# Chapter 10 - Power-ups with the Visitor pattern

Based on the Chapter 10 excerpt supplied from David Baron's *Game Development Patterns with Unity 2021, Second Edition*.

The current checkout is already wired. Open the Unity project in `DesignPatternLab/`, open `Assets/_Project/Scenes/Main.unity`, and enter Play Mode. Use a Game view of at least **960 x 600** so the chapter panels fit.

## Try it

1. Click **Start Countdown** and wait for the race to start.
2. In the top-center **Power-ups - Chapter 10 (Visitor)** panel, click **PowerUp Shield**. Shield health changes from 50 to 100; Chapter 9 bike health remains separate.
3. Click **PowerUp Engine**. Turbo boost changes from 25 to 50. Toggle Turbo with the existing button or release **W** to see its effect on speed and camera shake.
4. Click **PowerUp Weapon**. Range changes from 5 to 10, and strength from 25 to 31 (the book rounds the 25% increase).
5. Click **PowerUp Combo** to apply shield repair, engine boost, range, and strength together. Repeated upgrades stop at turbo 200, range 25, and strength 50.
6. Alternatively, drive straight through the colored cubes: green shield at Z=12, blue engine at Z=22, orange weapon at Z=32, purple combo at Z=42. Each pickup applies once and disappears for that run. Use A/D to change lanes or avoid them.
7. Click **Stop Race**, then **Start Replay**. Stats reset to their starting values before the recorded power-ups and damage run again. Pickups disappear at their recorded collection times. Repeating a replay does not stack upgrades from the previous run.
8. A new countdown restores the starting stats and all pickups.

Buttons are available while recording a race. Effects do not expire during a run. Weapon firing remains the chapter's logging-only skeleton; this chapter does not add combat.

## Pattern roles

| Role | Implementation |
| --- | --- |
| Visitor interface | `Systems/PowerUps/IVisitor.cs`, with a `Visit` overload for each element |
| Visitable interface | `Systems/PowerUps/IBikeElement.cs`, exposing `Accept(IVisitor)` |
| Concrete visitor | `PowerUp`, a configurable ScriptableObject |
| Visitable elements | `BikeShield`, `BikeEngine`, and `BikeWeapon` |
| Object structure | Existing `BikeController`, forwarding `Accept` to its three elements |
| Client | `ClientVisitor` buttons and `Pickup` trigger collection |

The call sequence is `BikeController.Accept(powerUp)` -> each element's `Accept(visitor)` -> the matching `PowerUp.Visit(element)` overload. The visitor holds the upgrade operation; the elements hold the bike's runtime values. Shared PowerUp assets are not changed when collected.

## Author power-ups

Use **Assets > Create > DesignPatternLab > PowerUp**. Configure the new asset in the Inspector:

- **Heal Shield** restores shield health to 100.
- **Turbo Boost** adds to the current boost, up to the engine's maximum.
- **Weapon Range** adds range, up to the weapon's maximum.
- **Weapon Strength** is a percentage of current strength, rounded and capped.
- **Powerup Name / Description** identify the asset. **Powerup Prefab** is an authoring reference; it does not automatically spawn anything.

Sample assets live in `Assets/_Project/ScriptableObjects/PowerUps/`. Duplicate a pickup prefab from `Assets/_Project/Prefabs/PowerUps/`, give its `Pickup.powerup` field your new asset, and drag the prefab onto the track. The trigger collider and the bike's kinematic Rigidbody provide the physics setup described in [Unity's trigger documentation](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Collider.OnTriggerEnter.html).

## Add it to your existing project

These steps are for transferring the changes to another Chapter 9 checkout. **They have already been applied here.**

1. Add these scripts with their `.meta` files:
   - `Scripts/Systems/PowerUps/`: `IVisitor`, `IBikeElement`, `PowerUp`, `Pickup`.
   - `Scripts/Characters/Bike/`: `BikeShield`, `BikeEngine`, `BikeWeapon`.
   - `Scripts/UI/ClientVisitor.cs`.
   - `Scripts/Systems/Replay/Commands/ApplyPowerUp.cs`.
2. Merge the updated `Scripts/Characters/Bike/BikeController.cs`, preserving its existing `.meta` file.
3. Add `BikeShield`, `BikeEngine`, and `BikeWeapon` to the **Bike prefab root**. Use shield 50; turbo boost 25 / maximum 200; weapon range 5 / maximum 25; strength 25 / maximum 50.
4. Add a **Rigidbody** to that root: **Use Gravity off**, **Is Kinematic on**. Keep the existing Body child collider and all earlier chapter components.
5. Copy the `ScriptableObjects/PowerUps`, `Prefabs/PowerUps`, and four `*Pickup.mat` materials, with their metadata.
6. Add `ClientVisitor` to the Bike instance in Main. Assign the matching engine, shield, weapon, and combo assets to its four fields. Keep `Invoker`, `InputHandler`, `ClientState`, and `ClientObserver`.
7. Place pickup prefabs along the track at Y=0.75. Their BoxColliders must have **Is Trigger** enabled.
8. Save Main and run the checks above. Do not attach interfaces, `PowerUp`, or `ApplyPowerUp` as components.

All script paths above are relative to `Assets/_Project/`.

## Explicit adaptations from the book

- Uses the existing project namespaces and BikeController; it does not create a second controller or replace the earlier patterns.
- Bike elements are serialized prefab components, enforced by `RequireComponent`, so their settings can be edited in the Inspector. BikeController visits those existing components rather than adding duplicates in `Start`.
- The existing controller remains the single owner of the Turbo toggle. The book's 300 mph baseline is scaled to this demo's `maxSpeed`: with Turbo on, movement speed is `maxSpeed * (1 + turboBoost / 300)`. With Turbo off, engine upgrades leave normal speed unchanged.
- Shield health is a separate Chapter 10 demonstration value. Chapter 9's **Damage Bike** still damages bike health directly, turns Turbo off, and stops the race at zero. Shield damage absorption is not implemented by the supplied chapter skeleton.
- Stats appear together in `ClientVisitor` instead of overlapping `OnGUI` labels on each element.
- Buttons and pickups use an `ApplyPowerUp` command so Chapter 7 records them. Editing PowerUp assets during a recording is not supported: commands retain their asset references.
- A consumed pickup hides its renderer and disables its collider instead of destroying the GameObject. It stays subscribed to race events so it can return on countdown/replay. Live trigger collection is blocked during replay to avoid applying effects twice.
- New recordings and replays restore the initial element values along with bike health and pose. Power-ups stack without a timer within each run.

## Regression checks

- Existing countdown, turn controls, stop, Turbo HUD/camera feedback, and drone pooling still work.
- Damage once while Turbo is on: bike health falls to 85, Turbo and shake stop, and speed returns to its normal value.
- Seven damage hits stop the race at zero health. Replay includes the final hit and stops again.
- Collecting a pickup with the Body child collider works once; replay does not collect it a second time through physics.
- Repeated new runs/replays reset all four element values and restore pickup visuals.

The existing WebGL export at the repository root is not rebuilt by these source changes. Build and publish a fresh export when you are ready to update the hosted demo.

## Validation performed

The runtime scripts compiled against the installed Unity 6000.3.24f1 assemblies. An isolated copy of Main was imported and exercised in Unity Play Mode using a minimal package manifest. Automated gameplay checks covered asset/component wiring, combo effects and caps, unchanged shared assets, Turbo speed, child-collider pickup collection, duplicate collection prevention, repeated replay and countdown resets, Chapter 9 lethal damage, and Chapter 8 pool reuse. Asset checks found no duplicate GUIDs or missing project GUID references.

The headless Unity editor also emitted a native `fidA != fidB` startup assertion and a `UnityEditor.Search.SearchDatabase` indexing exception. Both reproduced in a separate, unchanged Chapter 9 checkout and are reported separately from gameplay checks. A clean interactive-editor console, Game-view layout, and a new WebGL build have not been verified.
