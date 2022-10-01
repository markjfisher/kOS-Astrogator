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

// Values are doubles that can be used in calculations:
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

TransferModel(
          destination: ITargetable(name: Kerbin, orbit: ORBIT of Sun)
  transferDestination: ITargetable(null)
       transferParent: CelestialBody(null)
   retrogradeTransfer: False
               origin: ITargetable(name: minmus science space station, orbit: ORBIT of Minmus)
         ejectionBurn: BurnModel(attime: 11134548.9941572, prograde: 159.254365966869, normal: 0, radial: 0, totaldv: 159.254365966869, duration: 22.9794811408494)
      planeChangeburn: BurnModel(attime: 0, prograde: 0, normal: 0, radial: 0, totaldv: 0, duration: NaN)
 ejectionBurnDuration: 22.9794811408494
)


// Create a transfer node.
// This is functionally equivalent to "addons:astrographer:create(Kerbin, false)"
kerbTF:ejectionBurn:toNode.

```

## Help

This will print a link to this page.
```
addons:astrogator:help.
```

## Transfers

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `create(body, [shouldCreatePlanarChangeNode = true])` | Node | Creates up to 2 Maneuver nodes for a tranfer to the target body |
| `calculateBurns(body)` | List<BurnModel> | Calculates burn data (no nodes) for transfer to body |


## Physics

The following functions are exposed from Astrogator's PhysicsTools

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|


|`deltaVToOrbit(body)` | double | Calculate deltav to get a sensible orbit around body.|
|`speedAtPeriapsis(body, apo, peri)` | double | Generically calculate the speed at periapsis around given body with this apo/peri values in its orbit.|
|`speedAtApoapsis(body, apo, peri)` | double | Generically calculate the speed at apoapsis around given body with this apo/peri values in its orbit.|
|`shipSpeedAtPeriapsis()` | double | Ship speed at periapsis around current body.|
|`shipSpeedAtApoapsis()` | double | Ship speed at apoapsis around current body.|


## Structures

### BurnModel

This is a class representing the Burn without having to create a node.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `attime` | double | When the burn would occur in UT |
| `prograde` | double | The prograde element of the burn |
| `normal` | double | The normal element of the burn |
| `radial` | double | The radial element of the burn |
| `totalDV` | double | The magnitude of the burn |
| `duration` | double | The burn time |
| `toNode` | Node | Creates a maneuver node with given values |

### TransferModel

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `destination` | ITargetable | The body we're transferring to. |
| `transferDestination` | ITargetable | The SOI that we're aiming at, possibly an ancestor of our ultimate destination if the user targeted a distant moon. |
| `transferParent` | CelestialBody | The reference body of the transfer portion of our route. |
| `retrogradeTransfer` | bool |  True if the transfer portion of this trajectory is retrograde, false otherwise. So for a retrograde Kerbin orbit, this is true for Mun and false for Duna. |
| `origin` | ITargetable | The body we're transferring from. |
| `ejectionBurn` | BurnModel | Representation of the initial burn to start the transfer. |
| `planeChangeBurn` | BurnModel | Representation of the burn to change into the destination's orbital plane. |
| `ejectionBurnDuration` | double | Number of seconds to complete this burn for current vessel |
| `calculateEjectionBurn` | void | Calculates the ejectionBurn data for this transfer (runs when initialised, shouldn't need to run this) |
| `calculatePlaneChangeBurn` | void | Calculates the planeChangeBurn data for this transfer (runs when initialised, shouldn't need to run this) |
| `getDuration` | double | Calculate ejection burn duration |
| `haveEncounter` | bool | Check whether the current vessel currently has an encounter with this transfer's destination. |
| `checkIfNodesDisappeared` | void | Check whether the user opened any manuever node editing gizmos since the last tick |
| `createManeuvers` | void | Turn this transfer's burns into user visible maneuver nodes. Same as Astrogator's UI node icons. |
| `warpToBurn` | void | Warp to (near) the burn. Various results depending on where you are in time relative to the node. Cancels a warp if one is already going |


### AstrogationModel

This is a class representing the same data that can be found in the Astrogator main UI.

| **Command** | **Return Type** | **Description** |
|--------|--------|--------|
| `origin` | ITargetable | Usually the ship, the origin of the astrogation data. |
| `transfers` | Lexicon(string, TransferModel) | A lexicon of body names to TransferModels |
| `erorCondition` | bool | Whether there's an issue with this model, e.g. inclination too high, or hyperbolic trajectory not on an inbound trajectory|
| `badInclination` | bool | Does this have a bad inclination? Very large inclinations will not calculate |
| `retrogradeOrbit | bool | Is the orbit retrograde? |
| `inbound` | bool | Are we on an inbound hyperbolic orbit? |
| `hyperbolicorbit` | bool | Is the trajectory hyperbolic?  |
| `reset(ITargetable)` | void | Re-initialises the model for the given origin |
| `checkIfNodesDisappeared` | void  | Checks if either of the created nodes have been altered |
| `getDuration` | void | Tells all transfers to refresh their durations |
| `activeTransfer` | TranseferModel | Find the transfer that currently has an ejection burn instantiated as a real maneuver node, if any. |
| `activeEjectionBurn` | BurnModel | Find the ejection burn that's currently instantiated as a real maneuver node, if any. |
| `activePlaneChangeBurn` | BurnModel | Find the plane change burn that's currently instantiated as a real maneuver node, if any.  |

### CelestialBody

This is a mostly complete wrapper of the KSP CelestialBody type.

All methods that return simple types or Orbits/Vectors have the same name as their counterpart on the real type.

See https://anatid.github.io/XML-Documentation-for-the-KSP-API/class_celestial_body.html for full documentation of the wrapped class.

This is only used in TransferModel.transferParent.

