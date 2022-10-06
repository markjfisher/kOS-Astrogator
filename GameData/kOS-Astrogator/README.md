# kOS-Astrogator

[Astrogator](https://github.com/HebaruSan/Astrogator) functionality in kOS.

This mod is concerned with allowing kOS scripts to use Astrogator data. 

From your kOS scripts you can create maneuvers, or inspect the transfer calculations that would take
you to a target destination. 

The results are excellent for an initial set of vectors and times of ejection to get your ship near
or in the SOI of the target.

Astrogator does not guarantee you an SOI intercept, particularly for very distant bodies, but it
will provide something that can then be used in hill climbing algorithms to refine the targets
from your scripts.

It also does not currently ensure that the transfer is not influenced by other bodies on the way.
For instance, transfers to Minmus may be intercepted by the Mun, so you should check any nodes created
do reach the intended SOI and make adjustments if not from within your scripts.

This mod is a wrapper to Astrogator''s functionality, and does not do any changes to the calculated
burns or nodes.

## Example usage

### Simple transfer nodes
```
// create nodes to transfer to Minmus
addons:astrogator:create(Minmus).

// perform the transfers then once in the SOI of minmus,
// the following will create a circular orbit at Periapsis
addons:astrogator:create(Minmus).
```

### Calculating burns to specific target without nodes

```
// calculate the burns to Dres
set bms to addons:astrogator:calculateBurns(Dres).

print bms.
LIST of 2 items:
[0] = "BurnModel(attime: 22003501.7260593, prograde: 1668.01710042658, normal: 0, radial: 0, totaldv: 1668.01710042658, duration: -3)"
[1] = "BurnModel(attime: 26711517.2636837, prograde: -11.3082556944201, normal: -379.003641740976, radial: 6.3296734686348, totaldv: 379.225133484046, duration: 52.803860349922)"

// Create a Maneuver node from the first BurnModel:
set n0 to bms[0]:toNode.

// inspecting in the map view will note the DV requirement is too great for the ship being used, this is indicated by duration = -3.

// Values are standard kOS scalable values that can be used in calculations:
print bms[0]:totalDV / 1024.
```

### Using the Astrogation view data!

The data on the main UI interface to Astrogator can be accessed through the `astrogation` command.

```
// This example was taken from a ship in orbit of Minmus
set a to addons:astrogator:astrogation.
set tfs to a:transfers.
print tfs:keys.

LIST of 8 items:
[0] =
  ["value"] = "Kerbin"
[1] =
  ["value"] = "Mun"
[2] =
  ["value"] = "Moho"
[3] =
  ["value"] = "Eve"
[4] =
  ["value"] = "Duna"
[5] =
  ["value"] = "Dres"
[6] =
  ["value"] = "Jool"
[7] =
  ["value"] = "Eeloo"
```
Now we can interact with the data returned.

```
// because this is a Lexicon, we can access the keys by the names
set kerbTF to tfs:Kerbin.
print kerbTF.

TransferModel("Kerbin")

// Create a transfer node.
// This is functionally equivalent to "addons:astrographer:create(Kerbin, false)"
kerbTF:ejectionBurn:toNode.
```

#### Transfers to other vessels

Using the astrogation data, you can also create transfers to targets other than Celestial bodies.

Here's an example session doing that, from a ship in an orbit with a planet that has another target ship.
this should produce a very acurate transfer with a good close approach to the target.

Although it does also work if the target is orbiting another body, in that case you will have to adjust the
node as you will get a transfer that enters the SOI of the target's body, and will pass by the vessel, but
probably on an exiting trajectory.

```
set target to "rescue craft 1".
set a to addons:astrogator:astrogation.
set t to a:transfers["rescue craft 1"].
t:ejectionBurn:toNode.
```

## Help

This will print a link to this page.
```
addons:astrogator:help.
```

## Transfers

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `create(body, [shouldCreatePlanarChangeNode = true])` | Node | Creates up to 2 Maneuver nodes for a transfer to the target body |
| `calculateBurns(body)` | List<BurnModel> | Calculates burn data (no nodes) for transfer to body |

These are the primary interface for calculating and creating nodes to transfer to a target body. Astrogator will usually generate 2 items representing an initial DV burn to exit the current body and target the destination, with an additional smaller "plane change" to adjust for plane changes (if required). Thus you may need to execute both nodes to achieve the best results, or cater for them in any subsequent calculations you perform.

## Physics

The following functions are exposed from Astrogator's PhysicsTools

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
|`deltaVToOrbit(body)` | ScalarDoubleValue | Calculate deltav to get a sensible orbit around body.|
|`speedAtPeriapsis(body, apo, peri)` | ScalarDoubleValue | Generically calculate the speed at periapsis around given body with this apo/peri values in its orbit.|
|`speedAtApoapsis(body, apo, peri)` | ScalarDoubleValue | Generically calculate the speed at apoapsis around given body with this apo/peri values in its orbit.|
|`shipSpeedAtPeriapsis()` | ScalarDoubleValue | Ship speed at periapsis around current body.|
|`shipSpeedAtApoapsis()` | ScalarDoubleValue | Ship speed at apoapsis around current body.|


## Structures

### BurnModel

This is a class representing the Burn without having to create a node.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `attime` | ScalarDoubleValue | When the burn would occur in UT |
| `prograde` | ScalarDoubleValue | The prograde element of the burn |
| `normal` | ScalarDoubleValue | The normal element of the burn |
| `radial` | ScalarDoubleValue | The radial element of the burn |
| `totalDV` | ScalarDoubleValue | The magnitude of the burn |
| `duration` | ScalarDoubleValue | The burn time |
| `toNode` | Node | Creates a maneuver node with given values |

As kOS cannot return null, NAN, or infinity, negative values are used to indicate certain error conditions from Astrogator when reading the duration field.

| **Return Value** | **Meaning** |
|--------|--------|
| -1 | DeltaV calculation not available |
| -2 | Cannot perform burns |
| -3 | Not enough fuel to perform transfer |

### TransferModel

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `destination` | TransferTarget | Vessel or Body we're transferring to. |
| `transferDestination` | TransferTarget | Vessel or Body where the destination is. Targetting another vessel in the ship's current orbit will return the target vessel, else the target's body it is around |
| `transferParent` | Body | The reference body of the transfer portion of our route. |
| `retrogradeTransfer` | bool |  True if the transfer portion of this trajectory is retrograde, false otherwise. So for a retrograde Kerbin orbit, this is true for Mun and false for Duna. |
| `ejectionBurn` | BurnModel | Representation of the initial burn to start the transfer. |
| `planeChangeBurn` | BurnModel | Representation of the burn to change into the destination's orbital plane. |
| `ejectionBurnDuration` | ScalarDoubleValue | Number of seconds to complete this burn for current vessel |
| `calculateEjectionBurn` | None | Calculates the ejectionBurn data for this transfer (runs when initialised, shouldn't need to run this) |
| `calculatePlaneChangeBurn` | None | Calculates the planeChangeBurn data for this transfer (runs when initialised, shouldn't need to run this) |
| `getDuration` | ScalarDoubleValue | Calculate ejection burn duration |
| `haveEncounter` | bool | Check whether the current vessel currently has an encounter with this transfer's destination. |
| `checkIfNodesDisappeared` | None | Check whether the user opened any manuever node editing gizmos since the last tick |
| `createManeuvers` | None | Turn this transfer's burns into user visible maneuver nodes. Same as Astrogator's UI node icons. |
| `warpToBurn` | None | Warp to (near) the burn. Various results depending on where you are in time relative to the node. Cancels a warp if one is already going |

#### An example of a Transfer Model and its Trasfer Targets.

An example targetting Eve's moon Gilly from a craft in orbit of Kerbin:

```
> set target to "Gilly".
> set a to addons:astrogator.astrogation.
> set t to a:transfers:Gilly.

> print t:destination.
BodyTransferTarget("Gilly")

> print t:transferDestination.
BodyTransferTarget("Eve")

> print t:transferParent.
BODY("Sun").
```

### AstrogationModel

This is a class representing the same data that can be found in the Astrogator main UI.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `transfers` | Lexicon(string, TransferModel) | A lexicon of body names to TransferModels |
| `errorCondition` | bool | Whether there's an issue with this model, e.g. inclination too high, or hyperbolic trajectory not on an inbound trajectory|
| `badInclination` | bool | Does this have a bad inclination? Very large inclinations will not calculate |
| `retrogradeOrbit` | bool | Is the orbit retrograde? |
| `inbound` | bool | Are we on an inbound hyperbolic orbit? |
| `hyperbolicorbit` | bool | Is the trajectory hyperbolic?  |
| `reset(ITargetable)` | None | Re-initialises the model for the given origin |
| `checkIfNodesDisappeared` | None | Checks if either of the created nodes have been altered |
| `getDuration` | None | Tells all transfers to refresh their durations |
| `activeTransfer` | TranseferModel | Find the transfer that currently has an ejection burn instantiated as a real maneuver node, if any. |
| `activeEjectionBurn` | BurnModel | Find the ejection burn that's currently instantiated as a real maneuver node, if any. |
| `activePlaneChangeBurn` | BurnModel | Find the plane change burn that's currently instantiated as a real maneuver node, if any.  |

### TransferTarget

These are the targets of transfers and either contain a Body or a Vessel.

The `isVessel` arguement can be used to determine which it is, rather than using typename.

#### BodyTransferTarget

In a scenario where you are plotting transfers to bodies, this represents those targets.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `name` | String | The body's Name |
| `displayName` | String | The body's Display Name |
| `isVessel` | bool | Always False. This is to easily tell if the TransferTarget is a Body or Vessel rather than looking at typename |
| `body` | BodyTarget | The body representing the target of the transfer. |

#### VesselTransferTarget

In a scenario where you are plotting transfers to other vessels, this represents those targets. e.g. Calculating a tranfer to a target probe in this or another body's orbit.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `name` | String | The vessel's Name |
| `displayName` | String | The vessel's Display Name |
| `isVessel` | bool | Always True. This is to easily tell if the TransferTarget is a Body or Vessel rather than looking at typename |
| `vessel` | VesselTarget | The vessel representing the target of the transfer. |
